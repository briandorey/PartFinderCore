using System.ComponentModel.DataAnnotations;

namespace PartFinderCore.Models;

public class PartParameter
{
    [Key]
    public int PPpkey { get; init; }
    public int PartID { get; init; }
    [Required]
    [MaxLength(250)]
    public required string ParamName { get; set; }
    [MaxLength(250)]
    public string? ParamDescription { get; set; }
    [MaxLength(50)]
    public string? ParamValue { get; set; }
    [MaxLength(50)]
    public string? NormalizedValue { get; set; }
    [MaxLength(50)]
    public string? MaximumValue { get; set; }
    [MaxLength(50)]
    public string? NormalizedMaxValue { get; set; }
    [MaxLength(50)]
    public string? MinimumValue { get; set; }
    [MaxLength(50)]
    public string? NormalizedMinValue { get; set; }
}