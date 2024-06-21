using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PartFinderCore.Classes;
using PartFinderCore.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PartFinderCore.Pages.Admin.FootprintCat;

public class EditModel : PageModel
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

    [BindProperty] public int SelectedId { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;
    public string LitDeleteMsg { get; set; } = string.Empty;



    public void OnGet(int id = 0)
    {
        ViewData["Title"] = "Edit Footprint Category";

        SelectedId = id;
        using var dbContext = new SiteContext();
        var item = dbContext.FootprintCategory.FirstOrDefault(p => p.FCPkey == id);
        if (item != null)
        {
            PcName = item.FCName;
            PcDescription = item.FCDescription;
            var processor = new FootprintCategoryHelpers();
            CategoryItems = FootprintCategoryHelpers.BuildNestedCategories(item.ParentCategory, true);

            // check if category has items or sub categories and disable delete panel if true.

            var hasSub = dbContext.FootprintCategory.Where(p => p.ParentCategory == id).ToList();
            if (hasSub.Count > 0)
            {
                LitDeleteMsg += "<p>This category has sub categories and cannot be deleted.</p>";
            }


            var hasItems = dbContext.Footprint.Where(p => p.FootprintCategory == id).ToList();
            if (hasItems.Count > 0)
            {

                LitDeleteMsg += "<p>This category has footprints assigned and cannot be deleted.</p>";
            }
        }


    }

    public void OnPostSave()
    {
        ViewData["Title"] = "Edit Footprint Category";
        var processor = new CategoryHelpers();
        CategoryItems = CategoryHelpers.BuildNestedCategories(ParentId, true);

        if (ModelState.IsValid)
        {
            using var dbContext = new SiteContext();
            var item = dbContext.FootprintCategory.FirstOrDefault(p => p.FCPkey == SelectedId);
            if (item != null)
            {
                item.FCName = PcName;
                item.FCDescription = PcDescription;
                item.ParentCategory = ParentId;
            }
            dbContext.SaveChanges();


            Response.Redirect("treeview?mode=update");
        }
    }

    public void OnPostDeleteData()
    {

        using var dbContext = new SiteContext();
        var item = dbContext.FootprintCategory.FirstOrDefault(p => p.FCPkey == SelectedId);
        if (item != null)
        {
            dbContext.FootprintCategory.Remove(item);
            dbContext.SaveChanges();
            Response.Redirect("treeview?mode=delete");
        }
    }
}