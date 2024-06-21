using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using PartFinderCore.Classes;
using PartFinderCore.Models;

namespace PartFinderCore.Pages.Parts.Forms;

public class PartEditModel(IWebHostEnvironment environment) : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Part Name is required.")]
    [DisplayName("Part Name")]
    public string PartName { get; set; } = string.Empty;

    [BindProperty]
    [DisplayName("Description")]
    public string? PartDescription { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Stock Level is required.")]
    [DisplayName("Stock Level")]
    public int StockLevel { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Min Stock Level is required.")]
    [DisplayName("Min Stock Level")]
    public int MinStockLevel { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Price is required.")]
    [DisplayName("Price")]
    public decimal Price { get; set; }


    [BindProperty]
    [DisplayName("BarCode")]
    public string? BarCode { get; set; } = string.Empty;

    [BindProperty]
    [DisplayName("MPN")]
    public string? Mpn { get; set; } = string.Empty;

    [BindProperty]
    [DisplayName("Comment")]
    public string? PartComment { get; set; } = string.Empty;

    // dropdowns
    [BindProperty]
    public int Condition { get; set; }

    [BindProperty]
    public int PartCategoryId { get; set; }

    [BindProperty]
    public int PartManId { get; set; }

    [BindProperty]
    public int StorageLocationId { get; set; }

    [BindProperty]
    public int PartFootprintId { get; set; }

    public List<SelectListItem> ConditionItems = [];
    public List<SelectListItem> CategoryItems = [];
    public List<SelectListItem> ManItems = [];
    public List<SelectListItem> StorageItems = [];
    public List<SelectListItem> FootprintItems = [];

    public string ErrorMessage { get; set; } = string.Empty;

    [BindProperty] public int SelectedId { get; set; }
       
    public void OnGet(int? id = 0)
    {
        if (!(id > 0)) return;
        SelectedId = (int)id;
        using var dbContext = new SiteContext();
        var item = dbContext.Parts.FirstOrDefault(p => p.PartPkey == SelectedId);
        if (item == null) return;
        LoadMenus(item.PartCategoryID, item.Condition, item.StorageLocationID, item.PartFootprintID, item.PartManID);
        PartName = item.PartName;
        PartDescription = item.PartDescription;
        StockLevel = item.StockLevel;
        MinStockLevel = item.MinStockLevel;
        Price = item.Price;
        BarCode = item.BarCode;
        Mpn = item.MPN;
        PartComment = item.PartComment;

        HttpContext.Session.SetInt32("stocklevel", item.StockLevel);
    }

    public void OnPostSaveData()
    {
        if (ModelState.IsValid)
        {
            using var dbContext = new SiteContext();

            // Process stock changes.
            var oldStockLevel = HttpContext.Session.GetInt32("stocklevel");
            if (oldStockLevel != null) { 
                if (oldStockLevel !=  StockLevel)
                {
                    // stock level has changed, save into history table.
                    PartStockLevelHistory partStockLevelHistory = new()
                    {
                        StockLevel = StockLevel,
                        PartPkey = SelectedId,
                        DateChanged = DateTime.Now
                    };
                    dbContext.PartStockLevelHistory.Add(partStockLevelHistory);
                    dbContext.SaveChanges();
                }
            }

            // save changes
            var item = dbContext.Parts.FirstOrDefault(p => p.PartPkey == SelectedId);
            if (item != null)
            {
                item.PartName = PartName;
                item.PartDescription = PartDescription;
                item.StockLevel = StockLevel;
                item.MinStockLevel = MinStockLevel;
                item.Price = Price;
                item.BarCode = BarCode;
                item.MPN = Mpn;
                item.PartComment = PartComment!;
                item.PartCategoryID = PartCategoryId;
                item.Condition = Condition;
                item.StorageLocationID = StorageLocationId;
                item.PartFootprintID = PartFootprintId;
                item.PartManID = PartManId;

            }
            dbContext.SaveChanges();
            Response.Redirect("done");
               
        }
    }

    public void OnPostDeleteData()
    {
           
        using var dbContext = new SiteContext();

        if (dbContext.Parts.Select(p => new PartViewModel
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
                FootprintImage = p.Footprint.FootprintImage!,
                PCName = p.PartCategory!.PCName
            }).FirstOrDefault(p => p.PartPkey == SelectedId) is { } itemData)
        {
            // delete attachments folder
            var folderManName = itemData.ManufacturerName!.DirectoryName();
            var folderPartName = itemData.PartPkey.ToString().DirectoryName();
                
            var folder = Path.Combine(environment.WebRootPath, "docs", folderManName, folderPartName);
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }



            dbContext.PartAttachment.RemoveRange(dbContext.PartAttachment.Where(p => p.PartID == SelectedId));
            dbContext.PartParameter.RemoveRange(dbContext.PartParameter.Where(p => p.PartID == SelectedId));
            dbContext.PartSuppliers.RemoveRange(dbContext.PartSuppliers.Where(p => p.PartID == SelectedId));
            dbContext.PartStockLevelHistory.RemoveRange(dbContext.PartStockLevelHistory.Where(p => p.PartPkey == SelectedId));

            dbContext.Parts.Remove(dbContext.Parts.FirstOrDefault(p => p.PartPkey == SelectedId)!);

            dbContext.SaveChanges();
            Response.Redirect("done");

        }

    }

    private void LoadMenus(int category, int condition, int storage, int footprint, int manufacturer)
    {
        var processor = new CategoryHelpers();
        CategoryItems = CategoryHelpers.BuildNestedCategories(category);

        ConditionItems.Add(new SelectListItem { Text = "New", Value = "1", Selected = condition == 1 });
        ConditionItems.Add(new SelectListItem { Text = "Used", Value = "0", Selected = condition == 0 });

        using var dbContext = new SiteContext();
        var storageList = dbContext.StorageLocation.OrderBy(p => p.StorageName).ToList();
        foreach (var record in storageList)
        {
            StorageItems.Add(new SelectListItem { Text = record.StorageName, Value = record.StoragePkey.ToString(), Selected = record.StoragePkey == storage });
        }

        var footprintList = dbContext.Footprint.OrderBy(p => p.FootprintName).ToList();
        foreach (var record in footprintList)
        {
            FootprintItems.Add(new SelectListItem { Text = record.FootprintName, Value = record.FootprintPkey.ToString(), Selected = record.FootprintPkey == footprint });
        }

        var manList = dbContext.Manufacturer.OrderBy(p => p.ManufacturerName).ToList();
        foreach (var record in manList)
        {
            ManItems.Add(new SelectListItem { Text = record.ManufacturerName, Value = record.mpkey.ToString(), Selected = record.mpkey == manufacturer });
        }
    }
}