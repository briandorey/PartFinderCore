using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PartFinderCore.Classes;
using PartFinderCore.Models;

namespace PartFinderCore.Pages.Parts;

public class ListbyStorageLocationModel : PageModel
{
    public List<PartViewModel>? Data;
    public string LitMsg { get; set; } = string.Empty;
    public string CurrentLocation { get; set; } = string.Empty;

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

            var currentFp = dbContext.StorageLocation.FirstOrDefault(p => p.StoragePkey == CurrentId);
            CurrentLocation = currentFp!.StorageName;
            ViewData["Title"] = "Parts by Storage Location " + CurrentLocation;

            // populate dropdown
            var fpList = dbContext.StorageLocation.OrderBy(p => p.StorageName).ToList();

            foreach (var record in fpList)
            {
                Options.Add(new SelectListItem { Text = record.StorageName, Value = record.StoragePkey.ToString(), Selected = currentFp.StoragePkey == record.StoragePkey });
            }

            Data = await PartService.GetPartsAsync(CurrentSort, skip, pageSize, dbContext, 0, 0, CurrentId);

            var totalRecords = dbContext.Parts.Count(p => p.PartFootprintID == CurrentId);
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (Data.Count < 1)
            {
                LitMsg = "<div class=\"alert alert-warning text-center h4\" role=\"alert\">Your search did not match any parts</div>";
            }


            Pagination = Helpers.GetPagination(TotalPages, CurrentPage, "parts/listbystoragelocation", "sortorder=" + CurrentSort + "&id=" + CurrentId);
        }
    }
}