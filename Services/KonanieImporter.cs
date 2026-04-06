using IRSlovensko.Data;
using IRSlovensko.Models;
using Microsoft.EntityFrameworkCore;
using ServiceReference1;
using OsobaDb = IRSlovensko.Models.Osoba;
using SudDb = IRSlovensko.Models.Sud;
using SpravcaDb = IRSlovensko.Models.Spravca;
using WcfOsoba = ServiceReference1.Osoba;
using WcfSpravca = ServiceReference1.Spravca;

namespace IRSlovensko.Services;

public class KonanieImporter(IRDbContext db)
{
    private const int VelkostDavky = 500;
    private readonly KonanieServicePortClient _klient = new();
    private readonly KonanieMapper _mapper = new();
    private HashSet<long> _existujuceKonanieIds = [];

    public async Task ImportPoslednych30RokovAsync()
    {
        Console.WriteLine("Spúšťam import konania za posledných 30 rokov...");
        Console.WriteLine("Stratégia: vyhladajPoslednuZmenuOd + getKonanieDetail");

        await NacitajExistujuceKonanieIdsAsync();
        await ImportSudovAsync();

        var zmenyOd = DateTime.UtcNow.AddYears(-40);
        int celkemSpracovanych = 0;

        while (true)
        {
            var zmeny = await NacitajZmenyAsync(zmenyOd);
            if (zmeny.Length == 0) break;

            Console.WriteLine($"\nDávka {zmeny.Length} konání od {zmenyOd:yyyy-MM-dd HH:mm:ss}");

            foreach (var zmena in zmeny)
            {
                try
                {
                    await SpracujKonanieAsync(zmena.KonanieId);
                    celkemSpracovanych++;
                    if (celkemSpracovanych % 10 == 0)
                        Console.Write($"\r  Spracovaných: {celkemSpracovanych}...");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n  Chyba pri KonanieId={zmena.KonanieId}: {ex.Message}. Preskakujem.");
                }

                //await Task.Delay(100);
            }

            Console.WriteLine($"\r  Dávka hotová. Celkovo: {celkemSpracovanych}");

            // Posunúť ZmenyOd na najnovší čas + 1 sekunda pre ďalšiu dávku
            var maxZmena = zmeny.Max(z => z.PoslednaZmena);
            zmenyOd = maxZmena.AddSeconds(1);

            // Ak sme dostali menej ako plnú dávku, sme na konci
            if (zmeny.Length < VelkostDavky) break;

            //await Task.Delay(200);
        }

        Console.WriteLine($"\nImport dokončený. Celkovo spracovaných: {celkemSpracovanych}");
    }

    private async Task NacitajExistujuceKonanieIdsAsync()
    {
        Console.WriteLine("Načítavam existujúce ID konaní do pamäte...");
        _existujuceKonanieIds = new HashSet<long>(await db.Konania.Select(k => k.Id).ToListAsync());
        Console.WriteLine($"V pamäti {_existujuceKonanieIds.Count} existujúcich ID.");
    }

    private async Task ImportSudovAsync()
    {
        Console.WriteLine("Načítavam zoznam súdov...");

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

    private async Task<PoslednaZmenaNaKonani[]> NacitajZmenyAsync(DateTime od)
    {
        var request = new vyhladajPoslednuZmenuOdRequest
        {
            ZmenyOd = od,
            ZmenyOdSpecified = true,
            MaximalnyPocetVysledkov = VelkostDavky
        };

        var response = await _klient.vyhladajPoslednuZmenuOdAsync(request);
        return response.vyhladajPoslednuZmenuOdResponse.PoslednaZmenaNaKonaniList ?? [];
    }

    private async Task SpracujKonanieAsync(string konanieId)
    {
        // Rýchla kontrola v pamäti — preskočiť WCF volanie ak ID už existuje
        if (long.TryParse(konanieId, out var idCheck) && _existujuceKonanieIds.Contains(idCheck))
            return;

        var detailResponse = await _klient.getKonanieDetailAsync(new getKonanieDetailRequest
        {
            KonanieId = konanieId
        });

        var wcfKonanie = detailResponse.getKonanieDetailResponse.Konanie;
        if (wcfKonanie == null) return;

        // Fallback kontrola pre prípad, že ID nebolo číselné
        if (_existujuceKonanieIds.Contains(wcfKonanie.Id)) return;

        // Upsert Súd
        if (wcfKonanie.Sud != null)
            await UpsertSudAsync(wcfKonanie.Sud.id, wcfKonanie.Sud.nazov);

        // Upsert Správca
        int? spravcaId = null;
        if (wcfKonanie.Spravca != null)
            spravcaId = await UpsertSpravcaAsync(wcfKonanie.Spravca);

        // Upsert Dlžník
        int? dlznikId = null;
        if (wcfKonanie.Dlznik != null)
            dlznikId = await UpsertOsobuAsync(wcfKonanie.Dlznik, wcfKonanie.Id, isNew: false);

        db.Konania.Add(_mapper.MapFromWcf(wcfKonanie, spravcaId, dlznikId));
        await db.SaveChangesAsync();
        _existujuceKonanieIds.Add(wcfKonanie.Id);

        // Navrhovatelia — vlož nové
        await AktualizujNavrhovateliAsync(wcfKonanie.Id, wcfKonanie.Navrhovatel);
    }

    private async Task AktualizujNavrhovateliAsync(long konanieId, WcfOsoba[]? navrhovatelia)
    {
        // Odmaž staré záznamy
        var stare = await db.Navrhovatelia
            .Where(n => n.KonanieId == konanieId)
            .ToListAsync();
        db.Navrhovatelia.RemoveRange(stare);
        await db.SaveChangesAsync();

        if (navrhovatelia == null || navrhovatelia.Length == 0) return;

        // Pridaj nové
        foreach (WcfOsoba wcfOsoba in navrhovatelia)
        {
            var osobaId = await UpsertOsobuAsync(wcfOsoba, konanieId, isNew: true);
            if (osobaId.HasValue)
            {
                db.Navrhovatelia.Add(new Navrhovatel
                {
                    KonanieId = konanieId,
                    OsobaId = osobaId.Value
                });
            }
        }

        await db.SaveChangesAsync();
    }

    private async Task<int?> UpsertOsobuAsync(WcfOsoba wcf, long konanieId, bool isNew)
    {
        var osoba = _mapper.MapOsoba(wcf);

        OsobaDb? existing = null;
        if (!string.IsNullOrEmpty(osoba.Ico))
        {
            existing = db.Osoby.Local.FirstOrDefault(o => o.Ico == osoba.Ico)
                       ?? await db.Osoby.FirstOrDefaultAsync(o => o.Ico == osoba.Ico);
        }
        else if (!string.IsNullOrEmpty(osoba.Meno) && !string.IsNullOrEmpty(osoba.Priezvisko) && osoba.DatumNarodenia.HasValue)
        {
            existing = db.Osoby.Local.FirstOrDefault(o =>
                o.Meno == osoba.Meno &&
                o.Priezvisko == osoba.Priezvisko &&
                o.DatumNarodenia == osoba.DatumNarodenia)
                ?? await db.Osoby.FirstOrDefaultAsync(o =>
                o.Meno == osoba.Meno &&
                o.Priezvisko == osoba.Priezvisko &&
                o.DatumNarodenia == osoba.DatumNarodenia);
        }

        if (existing != null)
            return existing.Id;

        db.Osoby.Add(osoba);
        await db.SaveChangesAsync();
        return osoba.Id;
    }

    private async Task<int?> UpsertSpravcaAsync(WcfSpravca wcf)
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
        return spravca.Id;
    }

    private SpravcaDb MapSpravca(WcfSpravca wcf) => new()
    {
        Znacka = wcf.Znacka,
        DatumZapisu = wcf.DatumZapisuSpecified ? wcf.DatumZapisu : null,
        Meno = wcf.Meno,
        Priezvisko = wcf.Priezvisko,
        TitulPredMenom = wcf.TitulPredMenom,
        TitulZaMenom = wcf.TitulZaMenom,
        DatumNarodenia = wcf.DatumNarodeniaSpecified ? wcf.DatumNarodenia : null,
        ObchodneMeno = wcf.ObchodneMeno,
        Ico = wcf.Ico,
        Telefon = wcf.Telefon,
        Email = wcf.Email,
        Ulica = wcf.Adresa?.Ulica,
        SupisneCislo = wcf.Adresa?.SupisneCislo,
        OrientacneCislo = wcf.Adresa?.OrientacneCislo,
        Obec = wcf.Adresa?.Obec,
        Psc = wcf.Adresa?.Psc,
        Krajina = wcf.Adresa?.Krajina,
    };

    private static void AktualizujSpravcu(SpravcaDb spravca, WcfSpravca wcf)
    {
        spravca.DatumZapisu = wcf.DatumZapisuSpecified ? wcf.DatumZapisu : null;
        spravca.Meno = wcf.Meno;
        spravca.Priezvisko = wcf.Priezvisko;
        spravca.TitulPredMenom = wcf.TitulPredMenom;
        spravca.TitulZaMenom = wcf.TitulZaMenom;
        spravca.DatumNarodenia = wcf.DatumNarodeniaSpecified ? wcf.DatumNarodenia : null;
        spravca.ObchodneMeno = wcf.ObchodneMeno;
        spravca.Ico = wcf.Ico;
        spravca.Telefon = wcf.Telefon;
        spravca.Email = wcf.Email;
        spravca.Ulica = wcf.Adresa?.Ulica;
        spravca.SupisneCislo = wcf.Adresa?.SupisneCislo;
        spravca.OrientacneCislo = wcf.Adresa?.OrientacneCislo;
        spravca.Obec = wcf.Adresa?.Obec;
        spravca.Psc = wcf.Adresa?.Psc;
        spravca.Krajina = wcf.Adresa?.Krajina;
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
