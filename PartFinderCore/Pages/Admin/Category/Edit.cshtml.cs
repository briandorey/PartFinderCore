using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PartFinderCore.Classes;
using PartFinderCore.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PartFinderCore.Pages.Admin.Category;

public class EditModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Part Name is required.")]
    [DisplayName("Name")]
    public string PcName { get; set; } = string.Empty;

    [BindProperty]
    [DisplayName("Description")]
    public string? PcDescription { get; set; } = string.Empty;

    public List<SelectListItem> CategoryItems = [];
    [BindProperty] public int ParentId { get; set; }

    [BindProperty] public int SelectedId { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;
    public string LitDeleteMsg { get; set; } = string.Empty;


    public void OnGet(int id = 0)
    {
        ViewData["Title"] = "Edit Category";

        SelectedId = id;
        using var dbContext = new SiteContext();
        var item = dbContext.PartCategory.FirstOrDefault(p => p.PCpkey == id);
        if (item != null)
        {
            PcName = item.PCName;
            PcDescription = item.PCDescription;
            var processor = new CategoryHelpers();
            CategoryItems = CategoryHelpers.BuildNestedCategories(item.ParentID, true);

            // check if category has items or sub categories and disable delete panel if true.

            var hasSub = dbContext.PartCategory.Where(p => p.ParentID == id).ToList();
            if (hasSub.Count > 0)
            {
                LitDeleteMsg += "<p>This category has sub categories and cannot be deleted.</p>";
            }


            var hasItems = dbContext.Parts.Where(p => p.PartCategoryID == id).ToList();
            if (hasItems.Count > 0)
            {
                LitDeleteMsg += "<p>This category has parts assigned and cannot be deleted.</p>";
            }
        }
    }

    public void OnPostSave()
    {
        ViewData["Title"] = "Edit Category";
        var processor = new CategoryHelpers();
        CategoryItems = CategoryHelpers.BuildNestedCategories(ParentId, true);

        if (ModelState.IsValid)
        {
            using var dbContext = new SiteContext();
            var item = dbContext.PartCategory.FirstOrDefault(p => p.PCpkey == SelectedId);
            if (item != null)
            {
                item.PCName = PcName;
                item.PCDescription = PcDescription;
                item.ParentID = ParentId;
            }

            dbContext.SaveChanges();

            Response.Redirect("treeview?mode=update");
        }
    }

    public void OnPostDeleteData()
    {
        using var dbContext = new SiteContext();
        var item = dbContext.PartCategory.FirstOrDefault(p => p.PCpkey == SelectedId);
        if (item != null)
        {
            dbContext.PartCategory.Remove(item);
            dbContext.SaveChanges();
            Response.Redirect("treeview?mode=delete");
        }
    }
}