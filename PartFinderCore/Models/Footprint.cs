using System.ComponentModel.DataAnnotations;

namespace PartFinderCore.Models;

public class Footprint
{
    [Key]
    public int FootprintPkey { get; init; }
    [Required]
    [MaxLength(250)]
    public required string FootprintName { get; set; }
    [MaxLength(250)]
    public string? FootprintDescription { get; set; }
    [MaxLength(50)]
    public string? FootprintImage { get; set; }
    public int FootprintCategory { get; set; }

    public ICollection<Parts>? Parts { get; set; }

    public FootprintCategory? FootprintCategoryNavigation { get; set; }
}