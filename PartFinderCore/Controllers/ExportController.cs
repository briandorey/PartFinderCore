using Microsoft.AspNetCore.Mvc;
using PartFinderCore.Classes;
using PartFinderCore.Models;

namespace PartFinderCore.Controllers;

[ApiController]
[Route("api")]
public class ExportController(ICsvService csvService) : Controller
{
    [HttpGet("export")]
    public IActionResult Export()
    {
        // Generate sample data
        using SiteContext dbContext = new();

        List<PartViewModel> data = [.. dbContext.Parts.Select(p => new PartViewModel
        {
               
            PartName = p.PartName,
            PartDescription = p.PartDescription!,
            PartComment = p.PartComment,
            StockLevel = p.StockLevel,
            MinStockLevel = p.MinStockLevel,
            Price = p.Price,
            DateUpdated = p.DateUpdated,
            Condition = p.Condition,
            MPN = p.MPN!,
            BarCode = p.BarCode!,
            StorageName = p.StorageLocation!.StorageName,
            StorageSortOrder = p.StorageLocation.StorageSortOrder,
            ManufacturerName = p.Manufacturer!.ManufacturerName,
            FootprintName = p.Footprint!.FootprintName,
            PCName = p.PartCategory!.PCName
        })];


        var csvContent = csvService.GenerateCsv(data);

        var bytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
        return File(bytes, "text/csv", "data.csv");
    }
}