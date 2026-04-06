using KonanieDb = IRSlovensko.Models.Konanie;
using OsobaDb = IRSlovensko.Models.Osoba;
using ServiceReference1;

namespace IRSlovensko.Services;

public class KonanieMapper
{
    // Mapovanie z plného WCF Konanie (z getKonanieDetail)
    public KonanieDb MapFromWcf(Konanie wcf, int? spravcaId, int? dlznikId)
    {
        var konanie = new KonanieDb { Id = wcf.Id };
        UpdateFromWcf(konanie, wcf, spravcaId, dlznikId);
        return konanie;
    }

    public void UpdateFromWcf(KonanieDb konanie, Konanie wcf, int? spravcaId, int? dlznikId)
    {
        konanie.SpisovaZnackaSudu = wcf.SpisovaZnackaSudu;
        konanie.SpisovaZnackaSpravcu = wcf.SpisovaZnackaSpravcu;
        konanie.Typ = wcf.TypSpecified ? wcf.Typ.ToString() : null;
        konanie.SudId = wcf.Sud?.id;
        konanie.SpravcaId = spravcaId;
        konanie.DlznikId = dlznikId;
        konanie.Sudca = wcf.Sudca;
        konanie.DatumZacatiaKonania = wcf.DatumZacatiaKonania == default ? null : wcf.DatumZacatiaKonania;
        konanie.DatumZacatiaProcesu = wcf.DatumZacatiaProcesu == default ? null : wcf.DatumZacatiaProcesu;
        konanie.TypSpravcu = wcf.TypSpravcuSpecified ? wcf.TypSpravcu.ToString() : null;
        konanie.TypPrideleniaSpravcu = wcf.TypPrideleniaSpravcuSpecified ? wcf.TypPrideleniaSpravcu.ToString() : null;
        konanie.TypKonaniaPodlaUzemnejPlatnosti = wcf.TypKonaniaPodlaUzemnejPlatnosti;

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

    // Mapovanie WCF Osoba (polymorfná) → DB OsobaDb (flat)
    public OsobaDb MapOsoba(Osoba wcf)
    {
        var osoba = new OsobaDb
        {
            Telefon = wcf.Telefon,
            Email = wcf.Email,
            Iban = wcf.BankovyUcet?.Iban,
            Swift = wcf.BankovyUcet?.Swift,
        };

        if (wcf.Adresa != null)
        {
            osoba.Ulica = wcf.Adresa.Ulica;
            osoba.SupisneCislo = wcf.Adresa.SupisneCislo;
            osoba.OrientacneCislo = wcf.Adresa.OrientacneCislo;
            osoba.Obec = wcf.Adresa.Obec;
            osoba.Psc = wcf.Adresa.Psc;
            osoba.Krajina = wcf.Adresa.Krajina;
        }

        switch (wcf)
        {
            case FyzickaOsoba fo:
                osoba.Typ = "FyzickaOsoba";
                osoba.Meno = fo.Meno;
                osoba.Priezvisko = fo.Priezvisko;
                osoba.TitulPredMenom = fo.TitulPredMenom;
                osoba.TitulZaMenom = fo.TitulZaMenom;
                osoba.RodneCislo = fo.RodneCislo;
                osoba.DatumNarodenia = fo.DatumNarodenia == default ? null : fo.DatumNarodenia;
                osoba.StatneObcianstvo = fo.StatneObcianstvo;
                break;

            case FyzickaOsobaPodnikatel fp:
                osoba.Typ = "FyzickaOsobaPodnikatel";
                osoba.Meno = fp.Meno;
                osoba.Priezvisko = fp.Priezvisko;
                osoba.TitulPredMenom = fp.TitulPredMenom;
                osoba.TitulZaMenom = fp.TitulZaMenom;
                osoba.ObchodneMeno = fp.ObchodneMeno;
                osoba.Ico = fp.Ico;
                osoba.Register = fp.Register;
                osoba.CisloRegistracie = fp.CisloRegistracie;
                break;

            case PravnickaOsoba po:
                osoba.Typ = "PravnickaOsoba";
                osoba.ObchodneMeno = po.ObchodneMeno;
                osoba.Ico = po.Ico;
                osoba.PravnaForma = po.PravnaForma;
                osoba.Register = po.Register;
                osoba.CisloRegistracie = po.CisloRegistracie;
                break;
        }

        return osoba;
    }
}
