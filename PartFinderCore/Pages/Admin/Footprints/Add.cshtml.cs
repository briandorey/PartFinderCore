using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PartFinderCore.Classes;
using PartFinderCore.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PartFinderCore.Pages.Admin.Footprints;

public class AddModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Part Name is required.")]
    [DisplayName("Name")]
    public string FootprintName { get; set; } = string.Empty;

    [BindProperty]
    [DisplayName("Description")]
    public string? FootprintDescription { get; set; } = string.Empty;

    [BindProperty]
    [DisplayName("Image")]
    public string? FootprintImage { get; set; } = string.Empty;

    public List<SelectListItem> CategoryItems = [];
    [BindProperty]
    public int FootprintCategory { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;

    public void OnGet(int parent = 0)
    {
        ViewData["Title"] = "Add Footprint";
        var processor = new FootprintCategoryHelpers();
        CategoryItems = FootprintCategoryHelpers.BuildNestedCategories(parent, true);
    }

    public void OnPostSave()
    {
        ViewData["Title"] = "Add Footprint";
        var processor = new FootprintCategoryHelpers();
        CategoryItems = FootprintCategoryHelpers.BuildNestedCategories(FootprintCategory, true);

        if (ModelState.IsValid)
        {
            using var dbContext = new SiteContext();
            Footprint part = new() { FootprintName = FootprintName, FootprintDescription = FootprintDescription, FootprintCategory = FootprintCategory, FootprintImage= FootprintImage };
            dbContext.Footprint.Add(part);
            dbContext.SaveChanges();


            Response.Redirect("index?mode=add");
        }
    }
}