namespace PartFinderCore.Models;

public class PartViewModel
{
    public int PartPkey { get; set; }
    public int PartCategoryID { get; set; }
    public int PartFootprintID { get; set; }
    public int PartManID { get; set; }
    public string? PartName { get; set; }
    public string? PartDescription { get; set; }
    public string? PartComment { get; set; }
    public int StockLevel { get; set; }
    public int MinStockLevel { get; set; }
    public decimal Price { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
    public int Condition { get; set; }
    public int StorageLocationID { get; set; }
    public string? MPN { get; set; }
    public string? BarCode { get; set; }
    public string? StorageName { get; set; }
    public int StorageSortOrder { get; set; }
    public string? ManufacturerName { get; set; }
    public string? FootprintImage { get; set; }
    public string? FootprintName { get; set; }
    public string? PCName { get; set; }
        


}