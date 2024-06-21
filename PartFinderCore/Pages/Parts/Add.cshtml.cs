using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PartFinderCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using PartFinderCore.Classes;

namespace PartFinderCore.Pages.Parts;

public class AddModel : PageModel
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

    [BindProperty] [DisplayName("MPN")] public string? Mpn { get; set; } = string.Empty;

    [BindProperty]
    [DisplayName("Comment")]
    public string? PartComment { get; set; } = string.Empty;

    // dropdowns
    [BindProperty] public int Condition { get; set; }

    [BindProperty] public int PartCategoryId { get; set; }

    [BindProperty] public int PartManId { get; set; }

    [BindProperty] public int StorageLocationId { get; set; }

    [BindProperty] public int PartFootprintId { get; set; }

    public List<SelectListItem> ConditionItems = [];
    public List<SelectListItem> CategoryItems = [];
    public List<SelectListItem> ManItems = [];
    public List<SelectListItem> StorageItems = [];
    public List<SelectListItem> FootprintItems = [];

    public string ErrorMessage { get; set; } = string.Empty;

    public void OnGet(int? c = 0)
    {
        ViewData["Title"] = "Add Part";
        if (c > 0)
        {
            var currentIndex = (int)c;
            LoadMenus(currentIndex, 1, 0, 0, 0);
        }
        else
        {
            LoadMenus(0, 1, 0, 0, 0);
        }
    }


    public void OnPostSave()
    {
        LoadMenus(PartCategoryId, Condition, StorageLocationId, PartFootprintId, PartManId);


        if (ModelState.IsValid)
        {
            using var dbContext = new SiteContext();

            Models.Parts newPart = new()
            {
                PartCategoryID = PartCategoryId,
                Condition = Condition,
                StorageLocationID = StorageLocationId,
                PartFootprintID = PartFootprintId,
                PartManID = PartManId,
                PartName = PartName,
                PartDescription = PartDescription,
                PartComment = PartComment!,
                StockLevel = StockLevel,
                MinStockLevel = MinStockLevel,
                Price = Price,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                MPN = Mpn,
                BarCode = BarCode
            };
            dbContext.Parts.Add(newPart);
            dbContext.SaveChanges();

            var id = newPart.PartPkey;

            Response.Redirect("/parts/view?id=" + id);
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
            StorageItems.Add(new SelectListItem
            {
                Text = record.StorageName, Value = record.StoragePkey.ToString(),
                Selected = record.StoragePkey == storage
            });
        }

        var footprintList = dbContext.Footprint.OrderBy(p => p.FootprintName).ToList();
        foreach (var record in footprintList)
        {
            FootprintItems.Add(new SelectListItem
            {
                Text = record.FootprintName, Value = record.FootprintPkey.ToString(),
                Selected = record.FootprintPkey == footprint
            });
        }

        var manList = dbContext.Manufacturer.OrderBy(p => p.ManufacturerName).ToList();
        foreach (var record in manList)
        {
            ManItems.Add(new SelectListItem
            {
                Text = record.ManufacturerName, Value = record.mpkey.ToString(),
                Selected = record.mpkey == manufacturer
            });
        }
    }
}