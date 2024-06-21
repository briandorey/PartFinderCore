using System.ComponentModel.DataAnnotations;

namespace PartFinderCore.Models;

public class PartSuppliers
{
    [Key]
    public int SupPkey { get; init; }
    [Required]
    [MaxLength(250)]
    public required string SupplierName { get; set; }
    [Required]
    [MaxLength(250)]
    public required string URL { get; set; }
    public int PartID { get; init; }
}