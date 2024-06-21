using System.ComponentModel.DataAnnotations;

namespace PartFinderCore.Models;

public class Users
{
    [Key]
    public int UserPkey { get; init; }
    [Required]
    [MaxLength(50)]
    public required string Username { get; set; }
    [MaxLength(300)]
    public required string UserPass { get; set; }
}