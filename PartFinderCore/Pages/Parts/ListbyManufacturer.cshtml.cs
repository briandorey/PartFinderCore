using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PartFinderCore.Classes;
using PartFinderCore.Models;

namespace PartFinderCore.Pages.Parts;

public class ListbyManufacturerModel : PageModel
{
    public List<PartViewModel>? Data;
    public string LitMsg { get; set; } = string.Empty;
    public string CurrentManufacturer { get; set; } = string.Empty;
    public string ManufacturerAddress { get; set; } = string.Empty;
    public string ManufacturerUrl { get; set; } = string.Empty;
    public string ManufacturerPhone { get; set; } = string.Empty;
    public string ManufacturerEmail { get; set; } = string.Empty;
    public string ManufacturerComment { get; set; } = string.Empty;

    public int CurrentId { get; set; }

    // paging
    public int TotalPages = 1;
    private int _pageNumber;
    public int CurrentPage { get; set; }
    public string Pagination { get; set; } = string.Empty;

    // sorting 
    public string SortName { get; set; } = string.Empty;
    public string SortDescription { get; set; } = string.Empty;
    public string SortStock { get; set; } = string.Empty;
    public string SortLocation { get; set; } = string.Empty;
    public string SortManufacturer { get; set; } = string.Empty;
    public string SortFootprint { get; set; } = string.Empty;
    public string SortCategory { get; set; } = string.Empty;

    public string CurrentSort { get; set; } = string.Empty;

    public List<SelectListItem> Options = [];

    public async Task OnGetAsync(int id = 0, int pagenumber = 1, string sortOrder = "name")
    {


        if (id > 0)
        {
            CurrentId = id;

            // paging
            _pageNumber = pagenumber;
            const int pageSize = 25;
            CurrentPage = _pageNumber;
            var skip = (CurrentPage - 1) * pageSize;

            // sorting
            CurrentSort = sortOrder;

            SortName = sortOrder == "name" ? "name_desc" : "name";
            SortDescription = sortOrder == "description" ? "description_desc" : "description";
            SortStock = sortOrder == "stock" ? "stock_desc" : "stock";
            SortLocation = sortOrder == "location" ? "location_desc" : "location";
            SortManufacturer = sortOrder == "manufacturer" ? "manufacturer_desc" : "manufacturer";
            SortFootprint = sortOrder == "footprint" ? "footprint_desc" : "footprint";
            SortCategory = sortOrder == "category" ? "category_desc" : "category";

            // load data
            await using var dbContext = new SiteContext();

            var currentFp = dbContext.Manufacturer.FirstOrDefault(p => p.mpkey == CurrentId);
            if (currentFp != null)
            {
                CurrentManufacturer = currentFp.ManufacturerName;
                ViewData["Title"] = "Parts by " + CurrentManufacturer;

                ManufacturerAddress = currentFp.ManufacturerAddress!;
                ManufacturerUrl = "<a href=\"" + currentFp.ManufacturerURL + "\" target=\"_blank\">" + currentFp.ManufacturerURL + "</a>";
                ManufacturerPhone = currentFp.ManufacturerPhone!;
                ManufacturerEmail = "<a href=\"mailto:" + currentFp.ManufacturerEmail + "\">" + currentFp.ManufacturerEmail! + "</a>";
                ManufacturerComment = currentFp.ManufacturerComment;

                // populate dropdown
                var manList = dbContext.Manufacturer.OrderBy(p => p.ManufacturerName).ToList();

                foreach (var record in manList)
                {
                    Options.Add(new SelectListItem { Text = record.ManufacturerName, Value = record.mpkey.ToString(), Selected = currentFp.mpkey == record.mpkey });
                }
            }

            Data = await PartService.GetPartsAsync(CurrentSort, skip, pageSize, dbContext, 0, CurrentId);

            var totalRecords = dbContext.Parts.Count(p => p.PartManID == CurrentId);
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (Data.Count < 1)
            {
                LitMsg = "<div class=\"alert alert-warning text-center h4\" role=\"alert\">Your search did not match any parts</div>";
            }

            Pagination = Helpers.GetPagination(TotalPages, CurrentPage, "parts/listbymanufacturer", "sortorder=" + CurrentSort + "&id=" + CurrentId);
        }
    }
}