using KonanieDb = IRSlovensko.Models.Konanie;
using KonanieOsobaDb = IRSlovensko.Models.KonanieOsoba;
using ServiceReference1;

namespace IRSlovensko.Services;

public class KonanieMapper
{
    private static string? N(string? s) => string.IsNullOrEmpty(s) ? null : s;

    private static readonly Dictionary<KonanieTyp, int> _konanieTyp = new()
    {
        [KonanieTyp.LIKVIDACIA]                    = 1,
        [KonanieTyp.INE]                           = 2,
        [KonanieTyp.RESTRUKTURALIZACIA]            = 3,
        [KonanieTyp.ODDLZENIE_SPLATKOVY_KALENDAR]  = 4,
        [KonanieTyp.ODDLZENIE_KONKURZ]             = 5,
        [KonanieTyp.MALYKONKURZ]                   = 6,
        [KonanieTyp.KONKURZ]                       = 7,
        [KonanieTyp.VPR]                           = 8,
    };

    private static readonly Dictionary<TypSpravcuNaKonani, int> _typSpravcu = new()
    {
        [TypSpravcuNaKonani.RIADNY]    = 1,
        [TypSpravcuNaKonani.DOZORNY]   = 2,
        [TypSpravcuNaKonani.PREDBEZNY] = 3,
    };

    private static readonly Dictionary<TypPrideleniaSpravcu, int> _typPridelenia = new()
    {
        [TypPrideleniaSpravcu.VYMENOVANY] = 1,
        [TypPrideleniaSpravcu.GENEROVANY]  = 2,
    };

    // WCF vracia UPPER_SNAKE_CASE bez diakritiky (napr. "ZACATY_PROCES_KONANIA")
    private static readonly Dictionary<string, int> _stavKonania = new(StringComparer.OrdinalIgnoreCase)
    {
        ["PREBIEHAJUCE_KONANIE"]  = 1,
        ["PRERUSENE_KONANIE"]     = 2,
        ["SKONCENY_PROCES"]       = 3,
        ["ZACATE_KONANIE"]        = 4,
        ["ZACATY_PROCES_KONANIA"] = 5,
        ["ZASTAVENE_KONANIE"]     = 6,
    };

    private static readonly Dictionary<string, int> _dovodUkoncenia = new(StringComparer.OrdinalIgnoreCase)
    {
        ["INAK"]                                                          = 1,
        ["INY_DOVOD"]                                                     = 2,
        ["NEDA_SA_ZOSTAVIT_SPLATKOVY_KALENDAR_SPRAVCOM"]                  = 3,
        ["NEDOSTATOK_MAJETKU"]                                            = 4,
        ["NESPLNENIE_PODMIENOK"]                                          = 5,
        ["ODMIETNUTIE"]                                                   = 6,
        ["ODSUHLAS_ENY_PLAN"]                                             = 7,
        ["OSVEDCENA_PLATOBNA_SCHOPNOST"]                                  = 8,
        ["POTVRDENIE_PLANU_SUDOM"]                                        = 9,
        ["POVOLENIE_RESTRUKTURALIZACIE"]                                  = 10,
        ["SKONCENE_ODDLZENIE"]                                            = 11,
        ["SPATVZATIE"]                                                    = 12,
        ["SPLNENIE_KONECNEHO_ROZVRHU"]                                    = 13,
        ["SPLNENIE_ROZVRHU_VYTAZKU"]                                      = 14,
        ["URCENY_SPLATKOVY_KALENDAR"]                                     = 15,
        ["VYHLASENIE_KONKURZU"]                                           = 16,
        ["ZAPLATENIE_SPLATNYCH_POHLADAVOK"]                               = 17,
        ["ZIADNY_VERITEL"]                                                = 18,
    };

    // Mapovanie z plného WCF Konanie (z getKonanieDetailPodlaZnackyASudu)
    public KonanieDb MapFromWcf(Konanie wcf, string? znackaSpravcu)
    {
        var konanie = new KonanieDb { Id = wcf.Id };
        UpdateFromWcf(konanie, wcf, znackaSpravcu);
        return konanie;
    }

    public void UpdateFromWcf(KonanieDb konanie, Konanie wcf, string? znackaSpravcu)
    {
        konanie.SpisovaZnackaSudu = N(wcf.SpisovaZnackaSudu);
        konanie.SpisovaZnackaSpravcu = N(wcf.SpisovaZnackaSpravcu);
        konanie.TypId = wcf.TypSpecified && _konanieTyp.TryGetValue(wcf.Typ, out var typId) ? typId : null;
        konanie.SudId = N(wcf.Sud?.id);
        konanie.ZnackaSpravcu = znackaSpravcu;
        konanie.Sudca = N(wcf.Sudca);
        konanie.DatumZacatiaKonania = wcf.DatumZacatiaKonania == default ? null : wcf.DatumZacatiaKonania;
        konanie.DatumZacatiaProcesu = wcf.DatumZacatiaProcesu == default ? null : wcf.DatumZacatiaProcesu;
        konanie.TypSpravcuId = wcf.TypSpravcuSpecified && _typSpravcu.TryGetValue(wcf.TypSpravcu, out var typSId) ? typSId : null;
        konanie.TypPrideleniaSpravcuId = wcf.TypPrideleniaSpravcuSpecified && _typPridelenia.TryGetValue(wcf.TypPrideleniaSpravcu, out var typPId) ? typPId : null;
        konanie.TypKonaniaPodlaUzemnejPlatnosti = N(wcf.TypKonaniaPodlaUzemnejPlatnosti);

        if (wcf is Konkurz konkurz)
        {
            konanie.MalyKonkurz = konkurz.MalyKonkurz;
            konanie.DatumPovoleniaOddlzenia = konkurz.DatumPovoleniaOddlzenia == default ? null : konkurz.DatumPovoleniaOddlzenia;
        }
        if (wcf is Restrukturalizacia restr)
        {
            konanie.DatumZavedeniaDozornejSpravy = restr.DatumZavedeniaDozornejSpravy == default ? null : restr.DatumZavedeniaDozornejSpravy;
        }
    }

    // Aktualizácia konania zo súhrnných dát KonanieInfo (z getKonaniePreObodie)
    public void UpdateFromKonanieInfo(KonanieDb konanie, KonanieInfo info)
    {
        konanie.StavKonaniaId = !string.IsNullOrEmpty(info.StavKonania) && _stavKonania.TryGetValue(info.StavKonania, out var stavId) ? stavId : null;
        konanie.PoslednaUdalost = N(info.PoslednaUdalost);
        konanie.DatumPoslednejUdalosti = info.DatumPoslednejUdalostiSpecified ? info.DatumPoslednejUdalosti : null;
        konanie.DatumUkonceniaProcesu = info.DatumUkonceniaProcesuSpecified ? info.DatumUkonceniaProcesu : null;
        konanie.DovodUkonceniaProcesuId = !string.IsNullOrEmpty(info.DovodUkonceniaProcesu) && _dovodUkoncenia.TryGetValue(info.DovodUkonceniaProcesu, out var dovodId) ? dovodId : null;
        konanie.DatumPodania = info.DatumPodaniaSpecified ? info.DatumPodania : null;
    }

    public KonanieOsobaDb MapKonanieOsoba(Osoba wcf)
    {
        var osoba = new KonanieOsobaDb
        {
            Telefon = N(wcf.Telefon),
            Email = N(wcf.Email),
            Iban = N(wcf.BankovyUcet?.Iban),
            Swift = N(wcf.BankovyUcet?.Swift),
        };

        if (wcf.Adresa != null)
        {
            osoba.Ulica = N(wcf.Adresa.Ulica);
            osoba.SupisneCislo = N(wcf.Adresa.SupisneCislo);
            osoba.OrientacneCislo = N(wcf.Adresa.OrientacneCislo);
            osoba.Obec = N(wcf.Adresa.Obec);
            osoba.Psc = N(wcf.Adresa.Psc);
            osoba.Krajina = N(wcf.Adresa.Krajina);
        }

        switch (wcf)
        {
            case FyzickaOsoba fo:
                osoba.TypId = 1;
                osoba.Meno = N(fo.Meno);
                osoba.Priezvisko = N(fo.Priezvisko);
                osoba.TitulPredMenom = N(fo.TitulPredMenom);
                osoba.TitulZaMenom = N(fo.TitulZaMenom);
                osoba.RodneCislo = N(fo.RodneCislo);
                osoba.DatumNarodenia = fo.DatumNarodenia == default ? null : fo.DatumNarodenia;
                osoba.StatneObcianstvo = N(fo.StatneObcianstvo);
                break;

            case FyzickaOsobaPodnikatel fp:
                osoba.TypId = 2;
                osoba.Meno = N(fp.Meno);
                osoba.Priezvisko = N(fp.Priezvisko);
                osoba.TitulPredMenom = N(fp.TitulPredMenom);
                osoba.TitulZaMenom = N(fp.TitulZaMenom);
                osoba.ObchodneMeno = N(fp.ObchodneMeno);
                osoba.Ico = N(fp.Ico);
                osoba.Register = N(fp.Register);
                osoba.CisloRegistracie = N(fp.CisloRegistracie);
                break;

            case PravnickaOsoba po:
                osoba.TypId = 3;
                osoba.ObchodneMeno = N(po.ObchodneMeno);
                osoba.Ico = N(po.Ico);
                osoba.PravnaForma = N(po.PravnaForma);
                osoba.Register = N(po.Register);
                osoba.CisloRegistracie = N(po.CisloRegistracie);
                break;
        }

        return osoba;
    }
}
