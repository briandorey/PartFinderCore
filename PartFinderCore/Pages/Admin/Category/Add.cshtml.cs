using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using PartFinderCore.Models;
using PartFinderCore.Classes;

namespace PartFinderCore.Pages.Admin.Category;

public class AddModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Part Name is required.")]
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
        var processor = new CategoryHelpers();
        CategoryItems = CategoryHelpers.BuildNestedCategories(parent, true);
    }

    public void OnPostSave()
    {
        ViewData["Title"] = "Add Category";
        var processor = new CategoryHelpers();
        CategoryItems = CategoryHelpers.BuildNestedCategories(ParentId, true);

        if (!ModelState.IsValid) return;
        using var dbContext = new SiteContext();
        PartCategory partCategory = new() { PCName = PcName, PCDescription = PcDescription, ParentID = ParentId };
        dbContext.PartCategory.Add(partCategory);
        dbContext.SaveChanges();

        Response.Redirect("treeview?mode=add");
    }
}