using IRSlovensko.Models.Ciselniky;

namespace IRSlovensko.Models;

public class VerejnyOznam
{
    // Základní info (z VerejnyOznamInfo)
    public long OznamId { get; set; }
    public int? OznamTypId { get; set; }
    public string? SudKod { get; set; }
    public string? SudNazov { get; set; }
    public string? SpisovaZnackaSudnehoSpisu { get; set; }
    public long KonanieId { get; set; }
    public int? KonanieTypId { get; set; }
    public DateTime DatumVydania { get; set; }

    // Detail (z VerejnyOznam / VerejnyOznamSud / VerejnyOznamSpravca)
    public bool? ObsahujePrilohy { get; set; }

    // Súdne oznamy (VerejnyOznamSud)
    public string? TextDruh { get; set; }
    public string? TextPoucenie { get; set; }
    public string? TextHlavicka { get; set; }
    public string? TextOdovodnenie { get; set; }
    public string? TextOznam { get; set; }
    public string? TextRozhodnutie { get; set; }

    // Správcovské oznamy (VerejnyOznamSpravca)
    public int? DruhPodaniaId { get; set; }
    public string? Text { get; set; }
    public string? SpisovaZnackaSpravcovskehoSpisu { get; set; }

    public Konanie? Konanie { get; set; }
    public CSIRVerejneOznamyOznamTyp? OznamTyp { get; set; }
    public CSIRVerejneOznamyKonanieTyp? KonanieTyp { get; set; }
    public CSIRVerejneOznamyDruhPodania? DruhPodania { get; set; }
}
