namespace IRSlovensko.Models;

public class Konanie
{
    public long Id { get; set; }
    public string? Typ { get; set; }
    public string? SpisovaZnackaSudu { get; set; }
    public string? SpisovaZnackaSpravcu { get; set; }
    public string? SudId { get; set; }
    public int? SpravcaId { get; set; }
    public int? DlznikId { get; set; }
    public string? Sudca { get; set; }
    public DateTime? DatumZacatiaKonania { get; set; }
    public DateTime? DatumZacatiaProcesu { get; set; }
    public DateTime? DatumUkonceniaProcesu { get; set; }
    public string? DovodUkonceniaProcesu { get; set; }
    public DateTime? DatumPodania { get; set; }
    public string? DlznikMeno { get; set; }
    public string? DlznikIco { get; set; }
    public DateTime? DlznikDatumNarodenia { get; set; }
    public string? PoslednaUdalost { get; set; }
    public DateTime? DatumPoslednejUdalosti { get; set; }
    public string? StavKonania { get; set; }
    public string? TypSpravcu { get; set; }
    public string? TypPrideleniaSpravcu { get; set; }
    public string? TypKonaniaPodlaUzemnejPlatnosti { get; set; }
    public bool? MalyKonkurz { get; set; }
    public DateTime? DatumPovoleniaOddlzenia { get; set; }
    public DateTime? DatumZavedeniaDozornejSpravy { get; set; }

    public Sud? Sud { get; set; }
    public Spravca? Spravca { get; set; }
    public Osoba? Dlznik { get; set; }
    public ICollection<Navrhovatel> Navrhovatelia { get; set; } = [];
}
