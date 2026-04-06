namespace IRSlovensko.Models;

public class Navrhovatel
{
    public long KonanieId { get; set; }
    public int OsobaId { get; set; }

    public Konanie? Konanie { get; set; }
    public Osoba? Osoba { get; set; }
}
