using IRSlovensko.Models.Ciselniky;

namespace IRSlovensko.Models;

public class Konanie
{
    public long Id { get; set; }
    public int? TypId { get; set; }
    public string? SpisovaZnackaSudu { get; set; }
    public string? SpisovaZnackaSpravcu { get; set; }
    public string? SudId { get; set; }
    public string? ZnackaSpravcu { get; set; }
    public string? Sudca { get; set; }
    public DateTime? DatumZacatiaKonania { get; set; }
    public DateTime? DatumZacatiaProcesu { get; set; }
    public DateTime? DatumUkonceniaProcesu { get; set; }
    public int? DovodUkonceniaProcesuId { get; set; }
    public DateTime? DatumPodania { get; set; }
    public string? PoslednaUdalost { get; set; }
    public DateTime? DatumPoslednejUdalosti { get; set; }
    public int? StavKonaniaId { get; set; }
    public int? TypSpravcuId { get; set; }
    public int? TypPrideleniaSpravcuId { get; set; }
    public string? TypKonaniaPodlaUzemnejPlatnosti { get; set; }
    public bool? MalyKonkurz { get; set; }
    public DateTime? DatumPovoleniaOddlzenia { get; set; }
    public DateTime? DatumZavedeniaDozornejSpravy { get; set; }

    public Sud? Sud { get; set; }
    public Spravca? Spravca { get; set; }
    public CSIRKonaniaTyp? Typ { get; set; }
    public CSIRKonaniaStavKonania? StavKonania { get; set; }
    public CSIRKonaniaDovodUkonceniaProcesu? DovodUkonceniaProcesu { get; set; }
    public CSIRKonaniaTypSpravcu? TypSpravcu { get; set; }
    public CSIRKonaniaTypPrideleniaSpravcu? TypPrideleniaSpravcu { get; set; }
    public ICollection<KonanieOsoba> KonaniaOsoby { get; set; } = [];
}
