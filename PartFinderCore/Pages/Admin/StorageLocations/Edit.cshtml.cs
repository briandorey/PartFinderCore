using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PartFinderCore.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PartFinderCore.Pages.Admin.StorageLocations;

public class EditModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Name is required.")]
    [DisplayName("Name")]
    public string StorageName { get; set; } = string.Empty;

    [BindProperty]
    [DisplayName("Sort Order")]
    public int StorageSortOrder { get; set; }

    public string LitDeleteMsg { get; set; } = string.Empty;

    [BindProperty] public int SelectedId { get; set; }

    public void OnGet(int id = 0)
    {
        ViewData["Title"] = "Edit Storage Location";

        SelectedId = id;
        using var dbContext = new SiteContext();
        var item = dbContext.StorageLocation.FirstOrDefault(p => p.StoragePkey == id);
        if (item != null)
        {
            StorageName = item.StorageName;
            StorageSortOrder = item.StorageSortOrder;
             

            // check if manufacturer has items and disable delete panel if true.

            var hasSub = dbContext.Parts.Where(p => p.StorageLocationID == id).ToList();
            if (hasSub.Count > 0)
            {
                LitDeleteMsg += "<p>This location has parts assigned and cannot be deleted.</p>";
            }
        }


    }

    public void OnPostSave()
    {
        ViewData["Title"] = "Edit Storage Location";

        if (ModelState.IsValid)
        {
            using var dbContext = new SiteContext();
            var item = dbContext.StorageLocation.FirstOrDefault(p => p.StoragePkey == SelectedId);
            if (item != null)
            {
                item.StorageName = StorageName;
                item.StorageSortOrder = StorageSortOrder;
            }
            dbContext.SaveChanges();


            Response.Redirect("index?mode=update");
        }
    }

    public void OnPostDeleteData()
    {
        using var dbContext = new SiteContext();
        var item = dbContext.StorageLocation.FirstOrDefault(p => p.StoragePkey == SelectedId);
        if (item != null)
        {
            dbContext.StorageLocation.Remove(item);
            dbContext.SaveChanges();
            Response.Redirect("index?mode=delete");
        }
    }
}