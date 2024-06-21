using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using PartFinderCore.Classes;
using PartFinderCore.Models;

namespace PartFinderCore.Pages;

public class IndexModel : PageModel
{        
    public List<PartViewModel>? Data;
    public int TotalPages = 1;
    private int _pageNumber;
    public int CurrentPage { get; set; }
    public string Pagination { get; set; } = string.Empty;

    public string LitMsg { get; set; } = string.Empty;
    public string LitStorageCount { get; set; } = string.Empty;
    public string LitAttachmentCount { get; set; } = string.Empty;
    public string LitManCount { get; set; } = string.Empty;
    public string LitPartCount { get; set; } = string.Empty;

    public async Task OnGetAsync(int pagenumber = 1)
    {
        ViewData["Title"] = "Dashboard";

        _pageNumber = pagenumber;
        const int pageSize = 25;
        CurrentPage = _pageNumber;
        var skip = (CurrentPage - 1) * pageSize;

        await using var dbContext = new SiteContext();
        if (await dbContext.Database.GetService<IRelationalDatabaseCreator>().ExistsAsync())
        {
            //await dbContext.Database.EnsureCreatedAsync();

            LitStorageCount = dbContext.StorageLocation.Count().ToString();
            LitAttachmentCount = dbContext.PartAttachment.Count().ToString();
            LitManCount = dbContext.Manufacturer.Count().ToString();
            LitPartCount = dbContext.Parts.Count().ToString();

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
                StorageSortOrder = p.StorageLocation.StorageSortOrder,
                ManufacturerName = p.Manufacturer!.ManufacturerName,
                FootprintName = p.Footprint!.FootprintName,
                PCName = p.PartCategory!.PCName
            }).Where(p => p.StockLevel < p.MinStockLevel).OrderBy(p => p.StockLevel).Skip(skip).Take(pageSize).ToListAsync();

            var totalRecords = dbContext.Parts.Where(p => p.StockLevel < p.MinStockLevel).OrderBy(p => p.StockLevel).Count();
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            if (totalRecords < 1)
            {
                LitMsg = "You do not have any low stock parts";
            }
            //

            Pagination = Helpers.GetPagination(TotalPages, CurrentPage, "index", "");
        } else
        {
            // database does not exist, create and add default values
            await dbContext.Database.EnsureCreatedAsync();
            DataBaseInit.InitDataBaseValues();
        }
    }
}