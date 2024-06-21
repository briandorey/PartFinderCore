using Microsoft.AspNetCore.Mvc;
using PartFinderCore.Models;

namespace PartFinderCore.Controllers;

[Route("jsondata")]
[ApiController]
public class PartsController : Controller
{
    [Route("parts")]
    public JsonResult GetParts(int filter = 0)
    {
        using SiteContext db = new();

        List<int> result = GetAllChildEntries(filter);

        result.Add(filter);

        return GetPartsList(db, result);
    }
    private static List<int> GetAllChildEntries(int id)
    {
        List<int> result = [];
        using SiteContext db = new();
        var entities = db.PartCategory.ToList();
        FindChildEntries(id, result, entities);
        return result;
    }

    private static void FindChildEntries(int id, List<int> result, List<PartCategory> entities)
    {
        var children = entities.Where(e => e.ParentID == id).ToList();
        foreach (var child in children)
        {
            result.Add(child.PCpkey);
            FindChildEntries(child.PCpkey, result, entities); // Recursive call to find grandchildren
        }
    }

    private JsonResult GetPartsList(SiteContext db, List<int> filter)
    {
        
        if (filter == null)
        {
            var parts = db.Parts.Select(p => new PartViewModel
            {
                PartPkey = p.PartPkey,
                PartCategoryID = p.PartCategoryID,
                PartFootprintID = p.PartFootprintID,
                PartManID = p.PartManID,
                PartName = p.PartName,
                PartDescription = p.PartDescription!,
                PartComment = p.PartComment,
                StockLevel = p.StockLevel,
                MinStockLevel = p.MinStockLevel,
                Price = p.Price,
                DateCreated = p.DateCreated,
                DateUpdated = p.DateUpdated,
                Condition = p.Condition,
                StorageLocationID = p.StorageLocationID,
                MPN = p.MPN!,
                BarCode = p.BarCode!,
                StorageName = p.StorageLocation!.StorageName,
                StorageSortOrder = p.StorageLocation.StorageSortOrder,
                ManufacturerName = p.Manufacturer!.ManufacturerName,
                FootprintName = p.Footprint!.FootprintName,
                PCName = p.PartCategory!.PCName
            }).OrderBy(c => c.PartName).ToList();
            return Json(parts);
        }

        var partsFiltered = db.Parts.Select(p => new PartViewModel
        {
            PartPkey = p.PartPkey,
            PartCategoryID = p.PartCategoryID,
            PartFootprintID = p.PartFootprintID,
            PartManID = p.PartManID,
            PartName = p.PartName,
            PartDescription = p.PartDescription!,
            PartComment = p.PartComment,
            StockLevel = p.StockLevel,
            MinStockLevel = p.MinStockLevel,
            Price = p.Price,
            DateCreated = p.DateCreated,
            DateUpdated = p.DateUpdated,
            Condition = p.Condition,
            StorageLocationID = p.StorageLocationID,
            MPN = p.MPN!,
            BarCode = p.BarCode!,
            StorageName = p.StorageLocation!.StorageName,
            StorageSortOrder = p.StorageLocation.StorageSortOrder,
            ManufacturerName = p.Manufacturer!.ManufacturerName,
            FootprintName = p.Footprint!.FootprintName,
            PCName = p.PartCategory!.PCName
        }).Where(p => filter.Contains(p.PartCategoryID)).OrderBy(c => c.PartName).ToList();
        return Json(partsFiltered);
    }
}