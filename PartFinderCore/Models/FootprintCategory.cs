using System.ComponentModel.DataAnnotations;

namespace PartFinderCore.Models;

public class FootprintCategory
{
    [Key]
    public int FCPkey { get; init; }
    [Required]
    [MaxLength(50)]
    public required string FCName { get; set; }
    [MaxLength(250)]
    public string? FCDescription { get; set; }
    public int ParentCategory { get; set; }

    public ICollection<Footprint>? Footprints { get; set; }
}