using IRSlovensko.Data;
using IRSlovensko.Models;
using Microsoft.EntityFrameworkCore;
using ServiceReference1;
using System.Text.Json;
using KonanieOsobaDb = IRSlovensko.Models.KonanieOsoba;
using SpravcaDb = IRSlovensko.Models.Spravca;
using SudDb = IRSlovensko.Models.Sud;
using WcfOsoba = ServiceReference1.Osoba;
using WcfSpravca = ServiceReference1.Spravca;

namespace IRSlovensko.Services;

public class KonanieImporter(IRDbContext db)
{
    private const int VelkostDavky = 100;
    private readonly KonanieServicePortClient _klient = new();
    private readonly KonanieMapper _mapper = new();
    private HashSet<long> _existujuceKonanieIds = [];

    public async Task ImportPoslednychRokovAsync()
    {
        //await NacitajExistujuceKonanieIdsAsync();
        await ImportSudovAsync();

        int rokOd = DateTime.UtcNow.Year - 9;
        int rokDo = DateTime.UtcNow.Year;
        int CelkoveZpracovanych = 0;

        for (int rok = rokOd; rok <= rokDo; rok++)
        {
            Console.WriteLine($"\nSpracúvam rok {rok}...");
            int stranka = 0;

            while (true)
            {
                var request = new getKonaniePreObdobieRequest
                {
                    DatumOd = new DateTime(rok, 1, 1),
                    DatumDo = new DateTime(rok, 12, 31),
                    Stranka = stranka,
                    VysledkovNaStranku = VelkostDavky
                };

                var response = await _klient.getKonaniePreObdobieAsync(request);
                var seznam = response.getKonaniePreObdobieResponse.KonanieInfoList ?? [];

                foreach (var konanieInfo in seznam)
                {
                    try
                    {
                        await ZpracujKonanie(konanieInfo);
                        CelkoveZpracovanych++;
                        if (CelkoveZpracovanych % 10 == 0)
                            Console.Write($"\r  Spracovaných: {CelkoveZpracovanych}...");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"\n  Chyba pri KonanieId={konanieInfo.Id}: {ex.Message} | Detail: {ex.InnerException?.Message}. Preskakujem.");
                    }
                }

                Console.WriteLine($"\r  Rok {rok}, stránka {stranka}: {seznam.Length} záznamov. Celkovo: {CelkoveZpracovanych}");

                if (seznam.Length < VelkostDavky) break;

                stranka++;
            }
        }

        Console.WriteLine($"\nImport dokončený. Celkovo spracovaných: {CelkoveZpracovanych}");
        await UlozDatumStahovania();
    }

    private async Task UlozDatumStahovania()
    {
        var existing = await db.Statistika.SingleOrDefaultAsync();
        if (existing != null)
            db.Statistika.Remove(existing);
        db.Statistika.Add(new Statistika { DatumStahovania = DateTime.UtcNow });
        await db.SaveChangesAsync();
    }

    private async Task NacitajExistujuceKonanieIdsAsync()
    {
        Console.WriteLine("Načtení již vložených konanie do paměti");
        _existujuceKonanieIds = new HashSet<long>(await db.Konania.Select(k => k.Id).ToListAsync());
        Console.WriteLine($"V paměti je {_existujuceKonanieIds.Count} IDs.");
    }

    private async Task ImportSudovAsync()
    {
        Console.WriteLine("Nacitani zaznamu soudu");

        var response = await _klient.getZoznamSudovAsync(new getZoznamSudovRequest());
        var sudy = response.getZoznamSudovResponse1 ?? [];

        foreach (var wcfSud in sudy)
        {
            var sud = await db.Sudy.FindAsync(wcfSud.id);
            if (sud == null)
                db.Sudy.Add(new SudDb { Id = wcfSud.id, Nazov = wcfSud.nazov });
            else
                sud.Nazov = wcfSud.nazov;
        }

        await db.SaveChangesAsync();
        Console.WriteLine($"Uložených {sudy.Length} súdov.");
    }

    private async Task ZpracujKonanie(KonanieInfo konanieInfo)
    {
        // Kontrola zda již existuje konanie v paměti
        if (_existujuceKonanieIds.Contains(konanieInfo.Id)) return;

        var (wcfKonanie, icoKonanieInfo) = await NajdiKonanieAsync(konanieInfo);
        if (wcfKonanie == null) return;

        if (_existujuceKonanieIds.Contains(wcfKonanie.Id)) return;

        // getKonaniePreObdobie nevracia DlznikIco — skúsime ICO z plného detailu konania
        icoKonanieInfo ??= await NacitajKonanieInfoPodlaIcoAsync(wcfKonanie);

        var infoPreUpdate = icoKonanieInfo ?? konanieInfo;

        // Upsert Súd
        if (wcfKonanie.Sud != null)
            await UpsertSudAsync(wcfKonanie.Sud.id, wcfKonanie.Sud.nazov);

        // Upsert Správca
        string? znackaSpravcu = null;
        if (wcfKonanie.Spravca != null)
            znackaSpravcu = await UpsertSpravcaAsync(wcfKonanie.Spravca);

        var existing = await db.Konania.FindAsync(wcfKonanie.Id);
        if (existing != null)
        {
            _mapper.UpdateFromWcf(existing, wcfKonanie, znackaSpravcu);
            _mapper.UpdateFromKonanieInfo(existing, infoPreUpdate);
        }
        else
        {
            var konanieDb = _mapper.MapFromWcf(wcfKonanie, znackaSpravcu);
            _mapper.UpdateFromKonanieInfo(konanieDb, infoPreUpdate);
            db.Konania.Add(konanieDb);
        }
        await db.SaveChangesAsync();
        _existujuceKonanieIds.Add(wcfKonanie.Id);

        // Osoby s rolami
        await AktualizujKonaniaOsobyAsync(wcfKonanie.Id, wcfKonanie.Dlznik, wcfKonanie.Navrhovatel);
    }

    private async Task<KonanieInfo?> NacitajKonanieInfoPodlaIcoAsync(ServiceReference1.Konanie wcfKonanie)
    {
        string? ico = wcfKonanie.Dlznik switch
        {
            FyzickaOsobaPodnikatel fp when !string.IsNullOrEmpty(fp.Ico) => fp.Ico,
            PravnickaOsoba po when !string.IsNullOrEmpty(po.Ico) => po.Ico,
            _ => null
        };
        if (ico == null) return null;

        var response = await _klient.getKonaniePodlaICOAsync(
            new getKonaniePodlaICORequest { Ico = ico, VysledkovNaStranku = 100 });
        return response?.getKonaniePodlaICOResponse?.KonanieInfoList
            ?.FirstOrDefault(k => k.Id == wcfKonanie.Id);
    }

    private async Task<(ServiceReference1.Konanie?, KonanieInfo?)> NajdiKonanieAsync(KonanieInfo konanieInfo)
    {
        var detailResponse = await _klient.getKonanieDetailPodlaZnackyASuduAsync(
            new getKonanieDetailPodlaZnackyASuduRequest
            {
                KonanieZnacka = konanieInfo.SpisovaZnackaSudu,
                KonanieSud = konanieInfo.Sud
            });

        var wcfKonanie = detailResponse?.getKonanieDetailPodlaZnackyASuduResponse1
            ?.FirstOrDefault(k => k.Id == konanieInfo.Id);

        KonanieInfo? icoKonanieInfo = null;
        if (!string.IsNullOrEmpty(konanieInfo.DlznikIco))
        {
            var icoResponse = await _klient.getKonaniePodlaICOAsync(
                new getKonaniePodlaICORequest { Ico = konanieInfo.DlznikIco, VysledkovNaStranku = 100 });
            icoKonanieInfo = icoResponse?.getKonaniePodlaICOResponse?.KonanieInfoList
                ?.FirstOrDefault(k => k.Id == konanieInfo.Id);
        }

        if (wcfKonanie != null)
            return (wcfKonanie, icoKonanieInfo);

        if (icoKonanieInfo == null)
            return (null, null);

        var fullResponse = await _klient.getKonanieDetailAsync(
            new getKonanieDetailRequest { KonanieId = konanieInfo.Id.ToString() });

        return (fullResponse?.getKonanieDetailResponse?.Konanie, icoKonanieInfo);
    }

    private async Task AktualizujKonaniaOsobyAsync(long konanieId, WcfOsoba? dlznik, WcfOsoba[]? navrhovatelia)
    {
        var stare = await db.KonaniaOsoby.Where(o => o.IdKonania == konanieId).ToListAsync();
        db.KonaniaOsoby.RemoveRange(stare);
        await db.SaveChangesAsync();

        // Dlžník
        if (dlznik != null)
        {
            var osoba = _mapper.MapKonanieOsoba(dlznik);
            osoba.IdKonania = konanieId;
            osoba.RoleId = 1;
            db.KonaniaOsoby.Add(osoba);
        }

        // Navrhovatelia
        foreach (var wcfOsoba in navrhovatelia ?? [])
        {
            var osoba = _mapper.MapKonanieOsoba(wcfOsoba);
            osoba.IdKonania = konanieId;
            osoba.RoleId = 2;
            db.KonaniaOsoby.Add(osoba);
        }

        await db.SaveChangesAsync();
    }

    private async Task<string?> UpsertSpravcaAsync(WcfSpravca wcf)
    {
        if (string.IsNullOrEmpty(wcf.Znacka)) return null;

        var spravca = db.Spravcovia.Local.FirstOrDefault(s => s.Znacka == wcf.Znacka)
            ?? await db.Spravcovia.FirstOrDefaultAsync(s => s.Znacka == wcf.Znacka);

        if (spravca == null)
        {
            spravca = MapSpravca(wcf);
            db.Spravcovia.Add(spravca);
        }
        else
        {
            AktualizujSpravcu(spravca, wcf);
            db.Spravcovia.Update(spravca);
        }

        await db.SaveChangesAsync();
        return spravca.Znacka;
    }

    private static string? N(string? s) => string.IsNullOrEmpty(s) ? null : s;

    private SpravcaDb MapSpravca(WcfSpravca wcf) => new()
    {
        Znacka = wcf.Znacka,
        DatumZapisu = wcf.DatumZapisuSpecified ? wcf.DatumZapisu : null,
        Meno = N(wcf.Meno),
        Priezvisko = N(wcf.Priezvisko),
        TitulPredMenom = N(wcf.TitulPredMenom),
        TitulZaMenom = N(wcf.TitulZaMenom),
        DatumNarodenia = wcf.DatumNarodeniaSpecified ? wcf.DatumNarodenia : null,
        ObchodneMeno = N(wcf.ObchodneMeno),
        Ico = N(wcf.Ico),
        Telefon = N(wcf.Telefon),
        Email = N(wcf.Email),
        Ulica = N(wcf.Adresa?.Ulica),
        SupisneCislo = N(wcf.Adresa?.SupisneCislo),
        OrientacneCislo = N(wcf.Adresa?.OrientacneCislo),
        Obec = N(wcf.Adresa?.Obec),
        Psc = N(wcf.Adresa?.Psc),
        Krajina = N(wcf.Adresa?.Krajina),
    };

    private static void AktualizujSpravcu(SpravcaDb spravca, WcfSpravca wcf)
    {
        spravca.DatumZapisu = wcf.DatumZapisuSpecified ? wcf.DatumZapisu : null;
        spravca.Meno = N(wcf.Meno);
        spravca.Priezvisko = N(wcf.Priezvisko);
        spravca.TitulPredMenom = N(wcf.TitulPredMenom);
        spravca.TitulZaMenom = N(wcf.TitulZaMenom);
        spravca.DatumNarodenia = wcf.DatumNarodeniaSpecified ? wcf.DatumNarodenia : null;
        spravca.ObchodneMeno = N(wcf.ObchodneMeno);
        spravca.Ico = N(wcf.Ico);
        spravca.Telefon = N(wcf.Telefon);
        spravca.Email = N(wcf.Email);
        spravca.Ulica = N(wcf.Adresa?.Ulica);
        spravca.SupisneCislo = N(wcf.Adresa?.SupisneCislo);
        spravca.OrientacneCislo = N(wcf.Adresa?.OrientacneCislo);
        spravca.Obec = N(wcf.Adresa?.Obec);
        spravca.Psc = N(wcf.Adresa?.Psc);
        spravca.Krajina = N(wcf.Adresa?.Krajina);
    }

    private async Task UpsertSudAsync(string? id, string? nazov)
    {
        if (string.IsNullOrEmpty(id)) return;

        var sud = db.Sudy.Local.FirstOrDefault(s => s.Id == id)
            ?? await db.Sudy.FindAsync(id);

        if (sud != null) return;

        db.Sudy.Add(new SudDb { Id = id, Nazov = nazov });
        await db.SaveChangesAsync();
    }
}
