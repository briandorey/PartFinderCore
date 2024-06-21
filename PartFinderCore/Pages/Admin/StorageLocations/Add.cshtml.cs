using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PartFinderCore.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PartFinderCore.Pages.Admin.StorageLocations;

public class AddModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Name is required.")]
    [DisplayName("Name")]
    public string StorageName { get; set; } = string.Empty;

    [BindProperty]
    [DisplayName("Sort Order")]
    public int StorageSortOrder { get; set; }

    public void OnGet()
    {
        ViewData["Title"] = "Add Storage Location";
    }

    public void OnPostSave()
    {
        ViewData["Title"] = "Add Storage Location";

        if (ModelState.IsValid)
        {
            using var dbContext = new SiteContext();
            StorageLocation part = new()
            {
                StorageName = StorageName,
                StorageSortOrder = StorageSortOrder
            };
            dbContext.StorageLocation.Add(part);
            dbContext.SaveChanges();


            Response.Redirect("index?mode=add");
        }
    }
}