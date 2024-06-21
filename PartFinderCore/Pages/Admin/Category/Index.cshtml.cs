using Microsoft.AspNetCore.Mvc.RazorPages;
using PartFinderCore.Classes;
using PartFinderCore.Models;

namespace PartFinderCore.Pages.Admin.Category;

public class IndexModel : PageModel
{
    public List<PartCategory>? Data;
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
        ViewData["Title"] = "Category List";
        _pageNumber = pagenumber;
        const int pageSize = 25;
        CurrentPage = _pageNumber;
        var skip = (CurrentPage - 1) * pageSize;
        // sorting
        CurrentSort = sortOrder;

        SortName = sortOrder == "name" ? "name_desc" : "name";
        SortDescription = sortOrder == "description" ? "description_desc" : "description";

        using var dbContext = new SiteContext();
        Data = [.. dbContext.PartCategory.OrderBy(p => p.PCName)];

        switch (CurrentSort)
        {
            case "name":
                Data = [.. Data.OrderBy(p => p.PCName)];
                break;
            case "name_desc":
                Data = [.. Data.OrderByDescending(p => p.PCName)];
                break;
            case "description":
                Data = [.. Data.OrderBy(p => p.PCDescription)];
                break;
            case "description_desc":
                Data = [.. Data.OrderByDescending(p => p.PCDescription)];
                break;
        }

        Data = Data.Skip(skip).Take(pageSize).ToList();

        var totalRecords = dbContext.PartCategory.Count();
        TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

        Pagination = Helpers.GetPagination(TotalPages, CurrentPage, "admin/category/index", "sortorder=" + CurrentSort);
    }
}