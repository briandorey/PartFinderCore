using System.ComponentModel.DataAnnotations;

namespace PartFinderCore.Models;

public class PartCategory
{
    [Key]
    public int PCpkey { get; init; }
    public int ParentID { get; set; }
    [Required]
    [MaxLength(250)]
    public required string PCName { get; set; }
    [MaxLength(250)]
    public string? PCDescription { get; set; }

    public ICollection<Parts>? Parts { get; set; }
}