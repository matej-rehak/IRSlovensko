using IRSlovensko.Models.Ciselniky;

namespace IRSlovensko.Models;

public class KonanieOsoba
{
    public int Id { get; set; }
    public long IdKonania { get; set; }
    public int? RoleId { get; set; }
    public int? TypId { get; set; }
    public string? Meno { get; set; }
    public string? Priezvisko { get; set; }
    public string? TitulPredMenom { get; set; }
    public string? TitulZaMenom { get; set; }
    public string? RodneCislo { get; set; }
    public DateTime? DatumNarodenia { get; set; }
    public string? StatneObcianstvo { get; set; }
    public string? ObchodneMeno { get; set; }
    public string? Ico { get; set; }
    public string? Register { get; set; }
    public string? CisloRegistracie { get; set; }
    public string? PravnaForma { get; set; }
    public string? Telefon { get; set; }
    public string? Email { get; set; }
    public string? Ulica { get; set; }
    public string? SupisneCislo { get; set; }
    public string? OrientacneCislo { get; set; }
    public string? Obec { get; set; }
    public string? Psc { get; set; }
    public string? Krajina { get; set; }
    public string? Iban { get; set; }
    public string? Swift { get; set; }

    public Konanie? Konanie { get; set; }
    public CSIRKonaniaOsobyRole? Role { get; set; }
    public CSIRKonaniaOsobyTyp? Typ { get; set; }
}
