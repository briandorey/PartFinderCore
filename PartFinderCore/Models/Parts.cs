using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartFinderCore.Models;

public class Parts
{
    [Key]
    public int PartPkey { get; init; }
    public int PartCategoryID { get; set; }
    public int PartFootprintID { get; set; }
    public int PartManID { get; set; }
    [Required]
    [MaxLength(250)]
    public required string PartName { get; set; }
    [MaxLength(250)]
    public string? PartDescription { get; set; }
    [Column(TypeName = "ntext")]
    public string? PartComment { get; set; } = string.Empty;
    [MaxLength(4000)]
    public int StockLevel { get; set; }
    public int MinStockLevel { get; set; }
    public decimal Price { get; set; } = decimal.Zero;
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public DateTime DateUpdated { get; set; } = DateTime.Now;
    public int Condition { get; set; }
    public int StorageLocationID { get; set; }
    [MaxLength(250)]
    public string? MPN { get; set; }
    [MaxLength(50)]
    public string? BarCode { get; set; }



    public StorageLocation? StorageLocation { get; set; }
    public Manufacturer? Manufacturer { get; set; }
    public Footprint? Footprint { get; set; }
    public PartCategory? PartCategory { get; set; }
}