using System.Globalization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PartFinderCore.Classes;
using PartFinderCore.Models;

namespace PartFinderCore.Pages.Parts;

public class ViewModel : PageModel
{
    public List<PartStockLevelHistory>? ItemStockLevelHistory;
    public List<PartSuppliers>? ItemPartSuppliers;
    public List<PartParameter>? ItemPartParameter;
    public List<PartAttachment>? ItemPartAttachment;

    public string LitPartName { get; set; } = string.Empty;
    public string LitMpn { get; set; } = string.Empty;
    public string LitManufacturerName { get; set; } = string.Empty;
    public string LitBarcode { get; set; } = string.Empty;
    public string LitStockLevel { get; set; } = string.Empty;
    public string LitMinStockLevel { get; set; } = string.Empty;
    public string LitPrice { get; set; } = string.Empty;
    public string LitFootprintName { get; set; } = string.Empty;
    public string LitFootprintImage { get; set; } = string.Empty;
    public string LitPcName { get; set; } = string.Empty;
    public string HyperLinkLocation { get; set; } = string.Empty;
    public string LitPartDescription { get; set; } = string.Empty;
    public string LitPartComment { get; set; } = string.Empty;
    public string LitDateCreated { get; set; } = string.Empty;
    public string LitDateUpdated { get; set; } = string.Empty;
    public string LitCondition { get; set; } = string.Empty;
    public int LitItemId { get; set; }

    public void OnGet(int id = 0)
    {
        ViewData["Title"] = "View Part";
        if (id > 0)
        {
            LitItemId = id;
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
                }).FirstOrDefault(p => p.PartPkey == LitItemId) is { } itemData)
            {
                HyperLinkLocation = "<a href=\"/listbystoragelocation?id=" + itemData.StorageLocationID + "\">" + itemData.StorageName + "</a>";
                LitBarcode = itemData.BarCode!;

                LitCondition = Helpers.GetCondition(itemData.Condition);
                LitDateCreated = itemData.DateCreated.ToShortDateString();
                LitDateUpdated = itemData.DateUpdated.ToShortDateString();
                LitFootprintImage = CheckImage(itemData.FootprintImage!);
                LitFootprintName = itemData.FootprintName!;
                LitMpn = itemData.BarCode!;
                LitManufacturerName = itemData.ManufacturerName!;
                LitMinStockLevel = itemData.MinStockLevel.ToString();
                LitPcName = itemData.PCName!;
                LitPartComment = itemData.PartComment!;
                LitPartDescription = itemData.PartDescription!;
                LitPartName = itemData.PartName!;
                LitPrice = itemData.Price.ToString(CultureInfo.CurrentCulture);
                LitStockLevel = itemData.StockLevel.ToString();

                ItemStockLevelHistory = [.. dbContext.PartStockLevelHistory.Where(p => p.PartPkey == LitItemId)];
                ItemPartSuppliers = [.. dbContext.PartSuppliers.Where(p => p.PartID == LitItemId)];
                ItemPartParameter = [.. dbContext.PartParameter.Where(p => p.PartID == LitItemId)];
                ItemPartAttachment = [.. dbContext.PartAttachment.Where(p => p.PartID == LitItemId)];
            }
            else
            {
                // no record found
                Response.Redirect("list");
            }
        }
    }
    public string CheckImage(string inval)
    {
        if (inval.Trim().Length > 0)
        {
            return "<br><img src=\"" + inval + "\" class=\"img-fluid\" />";
        }
        return "";
    }
}