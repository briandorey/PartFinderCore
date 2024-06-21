using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PartFinderCore.Classes;
using PartFinderCore.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PartFinderCore.Pages.Admin.FootprintCat;

public class AddModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Name is required.")]
    [DisplayName("Name")]
    public string PcName { get; set; } = string.Empty;

    [BindProperty]
    [DisplayName("Description")]
    public string? PcDescription { get; set; } = string.Empty;

    public List<SelectListItem> CategoryItems = [];
    [BindProperty]
    public int ParentId { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;

    public void OnGet(int parent = 0)
    {
        ViewData["Title"] = "Add Category";
        var processor = new FootprintCategoryHelpers();
        CategoryItems = FootprintCategoryHelpers.BuildNestedCategories(parent, true);
    }

    public void OnPostSave()
    {
        ViewData["Title"] = "Add Footprint Category";
        var processor = new FootprintCategoryHelpers();
        CategoryItems = FootprintCategoryHelpers.BuildNestedCategories(ParentId, true);

        if (ModelState.IsValid)
        {
            using var dbContext = new SiteContext();
            FootprintCategory partCategory = new() { FCName = PcName, FCDescription = PcDescription, ParentCategory = ParentId };
            dbContext.FootprintCategory.Add(partCategory);
            dbContext.SaveChanges();


            Response.Redirect("treeview?mode=add");
        }
    }
}