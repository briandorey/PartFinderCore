using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartFinderCore.Models;

public class Manufacturer
{
    [Key]
    public int mpkey { get; init; }
    [Required]
    [MaxLength(250)]
    public required string ManufacturerName { get; set; }
    [MaxLength(250)]
    public  string? ManufacturerAddress { get; set; }
    [MaxLength(250)]
    public  string? ManufacturerURL { get; set; }
    [MaxLength(250)]
    public  string? ManufacturerPhone { get; set; }
    [MaxLength(250)]
    public  string? ManufacturerEmail { get; set; }
    [MaxLength(250)]
    public  string? ManufacturerLogo { get; set; }
    [Column(TypeName = "ntext")]
    public string ManufacturerComment { get; set; } = string.Empty;
    [MaxLength(4000)]

    public ICollection<Parts>? Parts { get; set; }
}