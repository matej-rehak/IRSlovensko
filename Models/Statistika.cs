using System.ComponentModel.DataAnnotations;

namespace IRSlovensko.Models;

public class Statistika
{
    [Key]
    public DateTime DatumStahovania { get; set; }
}
