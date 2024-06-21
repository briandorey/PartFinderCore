using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PartFinderCore.Classes;
using PartFinderCore.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PartFinderCore.Pages.Admin.Footprints;

public class EditModel : PageModel
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
    public string LitDeleteMsg { get; set; } = string.Empty;

    [BindProperty] public int SelectedId { get; set; }

    public void OnGet(int id = 0)
    {
        ViewData["Title"] = "Edit Footprint";

        SelectedId = id;
        using var dbContext = new SiteContext();
        var item = dbContext.Footprint.FirstOrDefault(p => p.FootprintPkey == id);
        if (item != null)
        {
            FootprintName = item.FootprintName;
            FootprintDescription = item.FootprintDescription;
            var processor = new FootprintCategoryHelpers();
            CategoryItems = FootprintCategoryHelpers.BuildNestedCategories(item.FootprintCategory, true);


               
            // check if category has items or sub categories and disable delete panel if true.

            var hasSub = dbContext.Parts.Where(p => p.PartFootprintID == id).ToList();
            if (hasSub.Count > 0)
            {
                LitDeleteMsg += "<p>This category has parts assigned and cannot be deleted.</p>";
            }
        }


    }

    public void OnPostSave()
    {
        ViewData["Title"] = "Edit Footprint";
        var processor = new FootprintCategoryHelpers();
        CategoryItems = FootprintCategoryHelpers.BuildNestedCategories(FootprintCategory, true);

        if (ModelState.IsValid)
        {
            using var dbContext = new SiteContext();
            var item = dbContext.Footprint.FirstOrDefault(p => p.FootprintPkey == SelectedId);
            if (item != null)
            {
                item.FootprintName = FootprintName;
                item.FootprintDescription = FootprintDescription;
                item.FootprintCategory = FootprintCategory;
                item.FootprintImage = FootprintImage;
            }
            dbContext.SaveChanges();


            Response.Redirect("index?mode=update");
        }
    }

    public void OnPostDeleteData()
    {

        using var dbContext = new SiteContext();
        var item = dbContext.Footprint.FirstOrDefault(p => p.FootprintPkey == SelectedId);
        if (item != null)
        {
            dbContext.Footprint.Remove(item);
            dbContext.SaveChanges();
            Response.Redirect("index?mode=delete");
        }
    }
}