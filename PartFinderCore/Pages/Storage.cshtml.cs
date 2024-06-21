using Microsoft.AspNetCore.Mvc.RazorPages;
using PartFinderCore.Classes;
using PartFinderCore.Models;

namespace PartFinderCore.Pages;

public class StorageModel : PageModel
{
    // paging
    public int TotalPages = 1;
    private int _pageNumber;
    public int CurrentPage { get; set; }
    public string Pagination { get; set; } = string.Empty;

    // sorting 
    public string SortName { get; set; } = string.Empty;
    public string CurrentSort { get; set; } = string.Empty;

    public List<StorageLocation>? Data;
    public void OnGet(int pagenumber = 1, string sortOrder = "name")
    {
        ViewData["Title"] = "Storage";
        _pageNumber = pagenumber;
        const int pageSize = 25;
        CurrentPage = _pageNumber;
        var skip = (CurrentPage - 1) * pageSize;
        // sorting
        CurrentSort = sortOrder;
        SortName = sortOrder == "name" ? "name_desc" : "name";

        using var dbContext = new SiteContext();
        Data = [.. dbContext.StorageLocation.OrderBy(p => p.StorageName)];

        if (Data != null)
        {
            Data = CurrentSort switch
            {
                "name" => [.. Data.OrderBy(p => p.StorageName)],
                "name_desc" => [.. Data.OrderByDescending(p => p.StorageName)],
                _ => [.. dbContext.StorageLocation.OrderBy(p => p.StorageName)]
            };

            Data = Data.Skip(skip).Take(pageSize).ToList();
        }

        var totalRecords = dbContext.StorageLocation.Count();
        TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        Pagination = Helpers.GetPagination(TotalPages, CurrentPage, "storage", "sortorder=" + CurrentSort);
    }
}