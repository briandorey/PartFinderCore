using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PartFinderCore.Classes;
using PartFinderCore.Models;

namespace PartFinderCore.Pages;

public class SearchModel : PageModel
{
    public List<PartViewModel>? Data;
    public string LitMsg { get; set; } = string.Empty;

    public async Task OnGetAsync(string searchbox)
    {
        ViewData["Title"] = "Search";
        if (searchbox.Length > 0)
        {
            searchbox = searchbox.ToLower().CleanInput();

            await using var dbContext = new SiteContext();

            Data = await dbContext.Parts.Select(p => new PartViewModel
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
                StorageSortOrder = p.StorageLocation!.StorageSortOrder,
                ManufacturerName = p.Manufacturer!.ManufacturerName,
                FootprintName = p.Footprint!.FootprintName,
                PCName = p.PartCategory!.PCName
            }).Where(p => p.PartName!.Contains(searchbox) || p.PartDescription!.Contains(searchbox) || p.BarCode!.Contains(searchbox)).OrderBy(p => p.StockLevel).ToListAsync();
            if (Data.Count < 1)
            {
                LitMsg = "<div class=\"alert alert-warning text-center h4\" role=\"alert\">Your search did not match any parts</div>";
            }
        }
           

    }
}