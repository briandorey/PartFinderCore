namespace PartFinderCore.Models;

public class FootprintWithCategory
{
    public int FootprintPkey { get; set; }
    public required string FootprintName { get; set; }
    public string? FootprintDescription { get; set; }
    public string? FootprintImage { get; set; }
    public int FootprintCategory { get; set; }
    public string? FCName { get; set; }
    public int ParentCategory { get; set; }
    public int FCPkey { get; set; }
    public string? FCDescription { get; set; }
}