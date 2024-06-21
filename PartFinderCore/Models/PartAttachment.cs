using System.ComponentModel.DataAnnotations;

namespace PartFinderCore.Models;

public class PartAttachment
{
    [Key]
    public int PApkey { get; init; }
    [Required]
    [MaxLength(250)]
    public required string FileName { get; set; }
    [Required]
    [MaxLength(250)]
    public required string DisplayName { get; set; }
    [Required]
    [MaxLength(250)]
    public required string MIMEType { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public int PartID { get; set; }
}