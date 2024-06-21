using Microsoft.AspNetCore.Mvc.RazorPages;
using PartFinderCore.Classes;
using PartFinderCore.Models;

namespace PartFinderCore.Pages.Admin.FootprintCat;

public class IndexModel : PageModel
{
    public List<FootprintCategory>? Data;
    // paging
    public int TotalPages = 1;
    private int _pageNumber;
    public int CurrentPage { get; set; }
    public string Pagination { get; set; } = string.Empty;

    // sorting 
    public string SortName { get; set; } = string.Empty;
    public string SortDescription { get; set; } = string.Empty;
    public string CurrentSort { get; set; } = string.Empty;

    public void OnGet(int pagenumber = 1, string sortOrder = "name")
    {
        ViewData["Title"] = "Footprint Category List";

        _pageNumber = pagenumber;
        const int pageSize = 25;
        CurrentPage = _pageNumber;
        var skip = (CurrentPage - 1) * pageSize;

        // sorting
        CurrentSort = sortOrder;

        SortName = sortOrder == "name" ? "name_desc" : "name";
        SortDescription = sortOrder == "description" ? "description_desc" : "description";


        using var dbContext = new SiteContext();
        Data = [.. dbContext.FootprintCategory.OrderBy(p => p.FCName)];
        if (Data != null)
        {
            Data = CurrentSort switch
            {
                "name" => [.. Data.OrderBy(p => p.FCName)],
                "name_desc" => [.. Data.OrderByDescending(p => p.FCName)],
                "description" => [.. Data.OrderBy(p => p.FCDescription)],
                "description_desc" => [.. Data.OrderByDescending(p => p.FCDescription)],
                _ => [.. dbContext.FootprintCategory.OrderBy(p => p.FCName)]
            };

            Data = Data.Skip(skip).Take(pageSize).ToList();
        }


        var totalRecords = dbContext.FootprintCategory.Count();
        TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

        Pagination = Helpers.GetPagination(TotalPages, CurrentPage, "admin/footprintcat/index", "sortorder=" + CurrentSort);

    }
}