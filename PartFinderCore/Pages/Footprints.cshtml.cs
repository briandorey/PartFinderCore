using Microsoft.AspNetCore.Mvc.RazorPages;
using PartFinderCore.Classes;
using PartFinderCore.Models;

namespace PartFinderCore.Pages;

public class FootprintsModel : PageModel
{
    public List<FootprintWithCategory>? Data;

    // paging
    public int TotalPages = 1;
    private int _pageNumber;
    public int CurrentPage { get; set; }
    public string Pagination { get; set; } = string.Empty;

    // sorting 
    public string SortName { get; set; } = string.Empty;
    public string SortCategory { get; set; } = string.Empty;
    public string CurrentSort { get; set; } = string.Empty;

    public void OnGet(int pagenumber = 1, string sortOrder = "name")
    {
        ViewData["Title"] = "Footprints";
        // paging
        _pageNumber = pagenumber;
        const int pageSize = 25;
        CurrentPage = _pageNumber;
        var skip = (CurrentPage - 1) * pageSize;


        // sorting
        CurrentSort = sortOrder;

        SortName = sortOrder == "name" ? "name_desc" : "name";
        SortCategory = sortOrder == "category" ? "category_desc" : "category";


        using var dbContext = new SiteContext();

        var query = from footprint in dbContext.Footprint
            join category in dbContext.FootprintCategory
                on footprint.FootprintCategory equals category.FCPkey
            select new FootprintWithCategory
            {
                FootprintPkey = footprint.FootprintPkey,
                FootprintName = footprint.FootprintName,
                FootprintDescription = footprint.FootprintDescription!,
                FootprintImage = footprint.FootprintImage!,
                FootprintCategory = footprint.FootprintCategory,
                FCName = category.FCName,
                ParentCategory = category.ParentCategory,
                FCPkey = category.FCPkey,
                FCDescription = category.FCDescription!
            };
        Data = [.. query];
        Data = CurrentSort switch
        {
            "name" => [.. Data.OrderBy(p => p.FootprintName)],
            "name_desc" => [.. Data.OrderByDescending(p => p.FootprintName)],
            "category" => [.. Data.OrderBy(p => p.FCName)],
            "category_desc" => [.. Data.OrderByDescending(p => p.FCName)],
            _ => [.. query]
        };

        Data = Data.Skip(skip).Take(pageSize).ToList();
           

        var totalRecords = dbContext.Footprint.Count();
        TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        Pagination = Helpers.GetPagination(TotalPages, CurrentPage, "footprints", "sortorder=" + CurrentSort);
    }
}