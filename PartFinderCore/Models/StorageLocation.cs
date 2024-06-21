using System.ComponentModel.DataAnnotations;

namespace PartFinderCore.Models;

public class StorageLocation
{
    [Key]
    public int StoragePkey { get; init; }
    [Required]
    [MaxLength(250)]
    public required string StorageName { get; set; }
    public int StorageSortOrder { get; set; }

    public ICollection<Parts>? Parts { get; set; }
}