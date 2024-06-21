using Microsoft.AspNetCore.Mvc.RazorPages;
using PartFinderCore.Classes;
using PartFinderCore.Models;

namespace PartFinderCore.Pages.Admin.Users;

public class IndexModel : PageModel
{
    public List<Models.Users>? Data;
    // paging
    public int TotalPages = 1;
    private int _pageNumber;
    public int CurrentPage { get; set; }
    public string Pagination { get; set; } = string.Empty;

    // sorting 
    public string SortName { get; set; } = string.Empty;
    public string CurrentSort { get; set; } = string.Empty;

    public void OnGet(int pagenumber = 1, string sortOrder = "name")
    {
        ViewData["Title"] = "Users";

        _pageNumber = pagenumber;
        const int pageSize = 25;
        CurrentPage = _pageNumber;
        var skip = (CurrentPage - 1) * pageSize;

        // sorting
        CurrentSort = sortOrder;

        SortName = sortOrder == "name" ? "name_desc" : "name";

        using var dbContext = new SiteContext();
        Data = [.. dbContext.Users.OrderBy(p => p.Username)];
        if (Data != null)
        {
            Data = CurrentSort switch
            {
                "name" => [.. Data.OrderBy(p => p.Username)],
                "name_desc" => [.. Data.OrderByDescending(p => p.Username)],
                _ => [.. dbContext.Users.OrderBy(p => p.Username)]
            };
            Data = Data.Skip(skip).Take(pageSize).ToList();
        }

        var totalRecords = dbContext.Users.Count();
        TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        Pagination = Helpers.GetPagination(TotalPages, CurrentPage, "admin/users/index", "sortorder=" + CurrentSort);
    }
}