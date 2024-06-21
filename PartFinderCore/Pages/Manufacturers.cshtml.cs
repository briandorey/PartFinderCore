using Microsoft.AspNetCore.Mvc.RazorPages;
using PartFinderCore.Classes;
using PartFinderCore.Models;

namespace PartFinderCore.Pages;

public class ManufacturersModel : PageModel
{
    public List<Manufacturer>? Data;

    // paging
    public int TotalPages = 1;
    private int _pageNumber;
    public int CurrentPage { get; set; }
    public string Pagination { get; set; } = string.Empty;

    // sorting 
    public string SortName { get; set; } = string.Empty;
    public string SortUrl { get; set; } = string.Empty;
    public string SortPhone { get; set; } = string.Empty;
    public string CurrentSort { get; set; } = string.Empty;

    public void OnGet(int pagenumber = 1, string sortOrder = "name")
    {
        ViewData["Title"] = "Manufacturers";
        // paging
        _pageNumber = pagenumber;
        const int pageSize = 25;
        CurrentPage = _pageNumber;
        var skip = (CurrentPage - 1) * pageSize;


        // sorting
        CurrentSort = sortOrder;

        SortName = sortOrder == "name" ? "name_desc" : "name";
        SortUrl = sortOrder == "url" ? "url_desc" : "url";
        SortPhone = sortOrder == "phone" ? "phone_desc" : "phone";


        using var dbContext = new SiteContext();
        Data = [.. dbContext.Manufacturer.OrderBy(p => p.ManufacturerName)];
        if (Data != null)
        {
            Data = CurrentSort switch
            {
                "name" => [.. Data.OrderBy(p => p.ManufacturerName)],
                "name_desc" => [.. Data.OrderByDescending(p => p.ManufacturerName)],
                "url" => [.. Data.OrderBy(p => p.ManufacturerURL)],
                "url_desc" => [.. Data.OrderByDescending(p => p.ManufacturerURL)],
                "phone" => [.. Data.OrderBy(p => p.ManufacturerPhone)],
                "phone_desc" => [.. Data.OrderByDescending(p => p.ManufacturerPhone)],
                _ => [.. dbContext.Manufacturer.OrderBy(p => p.ManufacturerName)]
            };

            Data = Data.Skip(skip).Take(pageSize).ToList();
        }

        var totalRecords = dbContext.Manufacturer.Count();
        TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        Pagination = Helpers.GetPagination(TotalPages, CurrentPage, ",manufacturers", "sortorder=" + CurrentSort);
    }
}