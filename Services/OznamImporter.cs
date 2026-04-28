using IRSlovensko.Data;
using Microsoft.EntityFrameworkCore;
using StatistikaDb = IRSlovensko.Models.Statistika;
using ServiceReference2;
using VerejnyOznamDb = IRSlovensko.Models.VerejnyOznam;

namespace IRSlovensko.Services;

public class OznamImporter(IRDbContext db)
{
    private const int VelkostDavky = 100;
    private readonly OznamServicePortClient _klient = new();

    private static readonly Dictionary<OznamTyp, int> _oznamTyp = new()
    {
        [OznamTyp.OZNAM_SUD]     = 1,
        [OznamTyp.OZNAM_SPRAVCA] = 2,
    };

    private static readonly Dictionary<KonanieTyp, int> _konanieTyp = new()
    {
        [KonanieTyp.KONKURZ]                   = 1,
        [KonanieTyp.MALYKONKURZ]               = 2,
        [KonanieTyp.RESTRUKTURALIZACIA]         = 3,
        [KonanieTyp.INEKONANIA]                 = 4,
        [KonanieTyp.ODDLZENIEKONKURZ]           = 5,
        [KonanieTyp.ODDLZENIESPLATKOVYKALENDAR] = 6,
        [KonanieTyp.LIKVIDACIA]                 = 7,
        [KonanieTyp.VPR]                        = 8,
    };

    private static readonly Dictionary<DruhPodaniaSpravca, int> _druhPodania = new()
    {
        [DruhPodaniaSpravca.OZNAM_O_TOM_KDE_A_KEDY_MOZNO_NAHLIADAT_DO_SPISU]                                                           = 1,
        [DruhPodaniaSpravca.OZNAM_O_ZVOLANI_SCHODZE_VERITELOV]                                                                          = 2,
        [DruhPodaniaSpravca.ZAPISNICA_ZO_ZASADNUTIA_VERITELSKEHO_VYBORU]                                                                = 3,
        [DruhPodaniaSpravca.OZNAMENIA_SUVISIACE_S_DRAZBOU_MAJETKU_PODLIEHAJUCEHO_KONKURZU]                                              = 4,
        [DruhPodaniaSpravca.SUPIS_VSEOBECNEJ_PODSTATY]                                                                                  = 5,
        [DruhPodaniaSpravca.SUPIS_ODDELENEJ_PODSTATY]                                                                                   = 6,
        [DruhPodaniaSpravca.ZVEREJNENIE_NAVRHU_CIASTKOVEHO_ROZVRHU_VYTAZKU_ZO_VSEOBECNEJ_PODSTATY]                                     = 7,
        [DruhPodaniaSpravca.ZVEREJNENIE_NAVRHU_KONECNEHO_VYTAZKU_ZO_VSEOEBCNEJ_PODSTATY]                                               = 8,
        [DruhPodaniaSpravca.DOPLNENIE_SUPISU_VSEOBECNEJ_PODSTATY_O_NOVU_SUPISOVU_ZLOZKU_MAJETKU]                                       = 9,
        [DruhPodaniaSpravca.DOPLNENIE_SUPISU_ODDELENEJ_PODSTATY_O_NOVU_SUPISOVU_ZLOZKU_MAJETKU]                                        = 10,
        [DruhPodaniaSpravca.PRERADENIE_SUPISOVEJ_ZLOZKY]                                                                                = 11,
        [DruhPodaniaSpravca.PRIPISANIE_POZNAMKY_O_SPORNOM_ZAPISE]                                                                       = 12,
        [DruhPodaniaSpravca.DOPLNENIE_ALEBO_ZMENA_POZNAMKY_O_SPORNOM_ZAPISE]                                                            = 13,
        [DruhPodaniaSpravca.VYMAZ_POZNAMKY_O_SPORNOM_ZAPISE]                                                                            = 14,
        [DruhPodaniaSpravca.VYLUCENIE_SUPISOVEJ_ZLOZKY_MAJETKU_ZO_SUPISU]                                                               = 15,
        [DruhPodaniaSpravca.INE_ZVEREJNENIE]                                                                                             = 16,
        [DruhPodaniaSpravca.VYZVA_ZAHRANICNYM_VERITELOM_NA_PRIHLASENIE_POHLADAVOK]                                                       = 17,
        [DruhPodaniaSpravca.OZNAMENIE_CISLA_BANKOVEHO_UCTU_PRE_POTREBY_POPIERANIA_POHLADAVOK]                                           = 18,
        [DruhPodaniaSpravca.OZNAMENIE_O_DORUCENI_PRIHLASKY_PO_UPLYNUTI_ZAKLADNEJ_PRIHLASOVACEJ_LEHOTY]                                  = 19,
        [DruhPodaniaSpravca.OZNAMENIA_SUVISIACE_SO_SPENAZOVANIM_MAJETKU_PODLIEHAJUCEHO_KONKURZU]                                        = 20,
        [DruhPodaniaSpravca.OZNAMENIE_O_ZOSTAVENI_ZOZNAMU_POHLADAVOK_PROTI_PODSTATE_A_ZAMERE_ZOSTAVIT_ROZVRH_ZO_VSEOBECNEJ_PODSTATY]   = 21,
        [DruhPodaniaSpravca.OZNAMENIE_O_ZOSTAVENI_ZOZNAMU_POHLADAVOK_PROTI_PODSTATE_A_ZAMERE_ZOSTAVIT_ROZVRH_Z_ODDELENEJ_PODSTATY]     = 22,
        [DruhPodaniaSpravca.ZVEREJNENIE_NAVRHU_CIASTKOVEHO_ROZVRHU_VYTAZKU_Z_ODDELENEJ_PODSTATY]                                        = 23,
        [DruhPodaniaSpravca.ZVEREJNENIE_NAVRHU_KONECNEHO_ROZVRHU_VYTAZKU_Z_ODDELENEJ_PODSTATY]                                          = 24,
        [DruhPodaniaSpravca.ZAPISNICA_ZO_ZASADNUTIA_SO_ZASTUPCOM_VERITELOV]                                                              = 25,
        [DruhPodaniaSpravca.OZNAM_O_ZOSTAVENI_NAVRHU_SPLATKOVEHO_KALENDARA]                                                              = 26,
        [DruhPodaniaSpravca.OZNAM_O_NEMOZNOSTI_ZOSTAVIT_NAVRH_SPLATKOVEHO_KALENDARA]                                                    = 27,
        [DruhPodaniaSpravca.OZNAM_O_ZRUSENI_KONKURZU_Z_DOVODU_ZE_DOSLO_K_SPLNENIU_ROZVRHU_VYTAZKU]                                      = 28,
        [DruhPodaniaSpravca.OZNAM_O_ZRUSENI_KONKURZU_Z_DOVODU_ZE_KONKURZNA_PODSTATA_NEPOKRYJE_NAKLADY_KONKURZU]                         = 29,
        [DruhPodaniaSpravca.OZNAM_O_ZRUSENI_KONKURZU_Z_DOVODU_ZE_SA_DO_90_DNI_OD_VYHLASENIA_KONKURZU_NEPRIHLASIL_ZIADNY_VERITEL]       = 30,
        [DruhPodaniaSpravca.OZNAM_O_ZRUSENI_KONKURZU_Z_DOVODU_ZE_POSTAVENIE_VSETKYCH_VERITELOV_AKO_UCASTNIKOV_KONANIA_ZANIKLO]          = 31,
        [DruhPodaniaSpravca.OZNAM_O_SKONCENI_KONANIA_O_NAVRHU_NA_URCENIE_SK]                                                            = 32,
        [DruhPodaniaSpravca.OZNAM_O_UPLATNENI_NAMIETKY_VOCI_ZAPISU_MAJETKU_DO_SUPISU]                                                   = 33,
        [DruhPodaniaSpravca.PREBIEHAJUCA_LIKVIDACIA]                                                                                     = 34,
        [DruhPodaniaSpravca.LIKVIDACIA_PRIHLASOVANIE_POHLADAVOK]                                                                         = 35,
        [DruhPodaniaSpravca.LIKVIDACIA_VYHOTOVENIE_ZOZNAMU_POHLADAVOK]                                                                   = 36,
        [DruhPodaniaSpravca.LIKVIDACIA_VYHOTOVENIE_ZOZNAMU_MAJETKU]                                                                      = 37,
        [DruhPodaniaSpravca.PODANIE_NAVRHU_NA_VYHLASENIE_KONKURZU_LIKVIDATOR]                                                            = 38,
        [DruhPodaniaSpravca.PRERUSENA_LIKVIDACIA_VYHLASENY_KONKURZ]                                                                      = 39,
        [DruhPodaniaSpravca.PRERUSENA_LIKVIDACIA]                                                                                        = 40,
        [DruhPodaniaSpravca.SKONCENE_OZNAMENIM_LIKVIDATORA]                                                                              = 41,
        [DruhPodaniaSpravca.SCHVALENIE_UCTOVNEJ_ZAVIERKY_KONECNEJ_SPRAVY_A_ROZDELENIA_ZOSTATKU]                                          = 42,
        [DruhPodaniaSpravca.PREBIEHAJUCA_DODATOCNA_LIKVIDACIA]                                                                           = 43,
        [DruhPodaniaSpravca.DODATOCNA_LIKVIDACIA_PRIHLASOVANIE_POHLADAVOK]                                                               = 44,
        [DruhPodaniaSpravca.DODATOCNA_LIKVIDACIA_VYHOTOVENIE_ZOZNAMU_POHLADAVOK]                                                         = 45,
        [DruhPodaniaSpravca.DODATOCNA_LIKVIDACIA_VYHOTOVENIE_ZOZNAMU_MAJETKU]                                                            = 46,
        [DruhPodaniaSpravca.DODATOCNA_LIKVIDACIA_PODANIE_NAVRHU_NA_VYHLASENIE_KONKURZU]                                                  = 47,
        [DruhPodaniaSpravca.DODATOCNA_LIKVIDACIA_PRERUSENE_VYHLASENY_KONKURZ]                                                            = 48,
        [DruhPodaniaSpravca.DODATOCNA_LIKVIDACIA_PRERUSENE]                                                                              = 49,
        [DruhPodaniaSpravca.DODATOCNA_LIKVIDACIA_SKONCENE_OZNAMENIM_LIKVIDATORA]                                                         = 50,
        [DruhPodaniaSpravca.DODATOCNA_LIKVIDACIA_SCHVALENIE_UCTOVNEJ_ZAVIERKY_KONECNEJ_SPRAVY_A_ROZDELENIA_ZOSTATKU]                     = 51,
        [DruhPodaniaSpravca.ZVOLANIE_SCHVALOVACEJ_SCHODZE_VERITELOV_PO_SCHVALENI_PLANU_VERITELSKYM_VYBOROM]                              = 52,
        [DruhPodaniaSpravca.SPRAVA_O_POSTUPE_SPENAZOVANIA]                                                                               = 53,
        [DruhPodaniaSpravca.ZAPISNICA_ZO_SCHODZE_VERITELOV]                                                                              = 54,
        [DruhPodaniaSpravca.ZAPISNICA_ZO_SCHVALOVACEJ_SCHODZE_VERITELOV]                                                                 = 55,
        [DruhPodaniaSpravca.ZVOLANIE_SCHODZE_VERITELSKEHO_VYBORU_NA_SCHVALENIE_PLANU]                                                    = 56,
        [DruhPodaniaSpravca.ZAZNAM_Z_ONLINE_SCHODZE_VERITELOV]                                                                           = 57,
        [DruhPodaniaSpravca.OZNAMENIE_O_ZVEREJNENI_SUPISU_MAJETKU]                                                                       = 58,
        [DruhPodaniaSpravca.UKONCENIE_PRIHLASOVANIA_POHLADAVOK]                                                                          = 59,
        [DruhPodaniaSpravca.UKONCENIE_PRIESKUMU_POHLADAVOK]                                                                              = 60,
        [DruhPodaniaSpravca.NEDEFINOVANE]                                                                                                = 61,
    };

    public async Task ImportPoslednych30RokovAsync()
    {
        int rokOd = DateTime.UtcNow.Year - 1;
        int rokDo = DateTime.UtcNow.Year;
        int celkoveZpracovanych = 0;

        for (int rok = rokOd; rok <= rokDo; rok++)
        {
            Console.WriteLine($"\n[Oznamy] Spracúvam rok {rok}...");
            int stranka = 0;

            while (true)
            {
                var response = await _klient.getVerejneOznamyPreObdobieAsync(
                    new getVerejneOznamyPreObdobieRequest
                    {
                        DatumOd = new DateTime(rok, 1, 1),
                        DatumOdSpecified = true,
                        DatumDo = new DateTime(rok, 12, 31),
                        DatumDoSpecified = true,
                        Stranka = stranka,
                        VysledkovNaStranku = VelkostDavky
                    });

                var seznam = response.getVerejneOznamyPreObdobieResponse?.VerejnyOznamInfoList ?? [];

                foreach (var info in seznam)
                {
                    try
                    {
                        await ZpracujOznamAsync(info);
                        celkoveZpracovanych++;
                        if (celkoveZpracovanych % 10 == 0)
                            Console.Write($"\r  [Oznamy] Spracovaných: {celkoveZpracovanych}...");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"\n  [Oznamy] Chyba pri OznamId={info.OznamId}: {ex.Message} | Detail: {ex.InnerException?.Message}. Preskakujem.");
                    }
                }

                Console.WriteLine($"\r  [Oznamy] Rok {rok}, stránka {stranka}: {seznam.Length} záznamov. Celkovo: {celkoveZpracovanych}");

                if (seznam.Length < VelkostDavky) break;
                stranka++;
            }
        }

        Console.WriteLine($"\n[Oznamy] Import dokončený. Celkovo spracovaných: {celkoveZpracovanych}");
        await UlozDatumStahovania();
    }

    private async Task UlozDatumStahovania()
    {
        var existing = await db.Statistika.SingleOrDefaultAsync();
        if (existing != null)
            db.Statistika.Remove(existing);
        db.Statistika.Add(new StatistikaDb { DatumStahovania = DateTime.UtcNow });
        await db.SaveChangesAsync();
    }

    private async Task ZpracujOznamAsync(VerejnyOznamInfo info)
    {
        var konanieExistuje = await db.Konania.AnyAsync(k => k.Id == info.KonanieId);
        if (!konanieExistuje) return;

        var detailResponse = await _klient.getVerejnyOznamDetailAsync(
            new getVerejnyOznamDetailRequest { OznamId = info.OznamId });
        var detail = detailResponse?.getVerejnyOznamDetailResponse?.VerejnyOznam;

        var existing = await db.VerejneOznamy.FindAsync(info.OznamId);
        if (existing != null)
        {
            NaplnDetail(existing, info, detail);
        }
        else
        {
            var oznam = new VerejnyOznamDb
            {
                OznamId = info.OznamId,
                OznamTypId = _oznamTyp.TryGetValue(info.OznamTyp, out var otId) ? otId : null,
                SudKod = info.SudKod,
                SudNazov = info.SudNazov,
                SpisovaZnackaSudnehoSpisu = info.SpisovaZnackaSudnehoSpisu,
                KonanieId = info.KonanieId,
                KonanieTypId = _konanieTyp.TryGetValue(info.KonanieTyp, out var ktId) ? ktId : null,
                DatumVydania = info.DatumVydania,
            };
            NaplnDetail(oznam, info, detail);
            db.VerejneOznamy.Add(oznam);
        }

        await db.SaveChangesAsync();
    }

    private static void NaplnDetail(VerejnyOznamDb oznam, VerejnyOznamInfo info, VerejnyOznam? detail)
    {
        oznam.DatumVydania = info.DatumVydania;

        if (detail == null) return;

        oznam.ObsahujePrilohy = detail.ObsahujePrilohy;

        if (detail is VerejnyOznamSud sud)
        {
            oznam.TextDruh = sud.TextDruh.ToString();
            oznam.TextPoucenie = sud.TextPoucenie;
            oznam.TextHlavicka = sud.TextHlavicka;
            oznam.TextOdovodnenie = sud.TextOdovodnenie;
            oznam.TextOznam = sud.TextOznam;
            oznam.TextRozhodnutie = sud.TextRozhodnutie;
        }
        else if (detail is VerejnyOznamSpravca spravca)
        {
            oznam.DruhPodaniaId = _druhPodania.TryGetValue(spravca.DruhPodania, out var dpId) ? dpId : null;
            oznam.Text = spravca.Text;
            oznam.SpisovaZnackaSpravcovskehoSpisu = spravca.SpisovaZnackaSpravcovskehoSpisu;
        }
    }
}
