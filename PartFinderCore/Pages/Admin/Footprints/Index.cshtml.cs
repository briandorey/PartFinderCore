using Microsoft.AspNetCore.Mvc.RazorPages;
using PartFinderCore.Classes;
using PartFinderCore.Models;

namespace PartFinderCore.Pages.Admin.Footprints;

public class IndexModel : PageModel
{
    public List<Footprint>? Data;
    // paging
    public int TotalPages = 1;
    private int _pageNumber;
    public int CurrentPage { get; set; }
    public string Pagination { get; set; } = string.Empty;

    // sorting 
    public string SortName { get; set; } = string.Empty;
    public string SortDescription { get; set; } = string.Empty;
    public string SortImage { get; set; } = string.Empty;
    public string CurrentSort { get; set; } = string.Empty;

    public void OnGet(int pagenumber = 1, string sortOrder = "name")
    {
        ViewData["Title"] = "Footprints List";

        _pageNumber = pagenumber;
        const int pageSize = 25;
        CurrentPage = _pageNumber;
        var skip = (CurrentPage - 1) * pageSize;

        // sorting
        CurrentSort = sortOrder;

        SortName = sortOrder == "name" ? "name_desc" : "name";
        SortDescription = sortOrder == "description" ? "description_desc" : "description";
        SortImage = sortOrder == "image" ? "image_desc" : "image";


        using var dbContext = new SiteContext();
        Data = [.. dbContext.Footprint.OrderBy(p => p.FootprintName)];
        if (Data != null)
        {
            Data = CurrentSort switch
            {
                "name" => [.. Data.OrderBy(p => p.FootprintName)],
                "name_desc" => [.. Data.OrderByDescending(p => p.FootprintName)],
                "description" => [.. Data.OrderBy(p => p.FootprintDescription)],
                "description_desc" => [.. Data.OrderByDescending(p => p.FootprintDescription)],
                "image" => [.. Data.OrderBy(p => p.FootprintImage)],
                "image_desc" => [.. Data.OrderByDescending(p => p.FootprintImage)],
                _ => [.. dbContext.Footprint.OrderBy(p => p.FootprintName)]
            };

            Data = Data.Skip(skip).Take(pageSize).ToList();
        }

        var totalRecords = dbContext.Footprint.Count();
        TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

        Pagination = Helpers.GetPagination(TotalPages, CurrentPage, "admin/footprints/index", "sortorder=" + CurrentSort);
    }
}