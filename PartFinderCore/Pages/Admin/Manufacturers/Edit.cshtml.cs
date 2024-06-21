using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PartFinderCore.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PartFinderCore.Pages.Admin.Manufacturers;

public class EditModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Name is required.")]
    [DisplayName("Name")]
    public string ManufacturerName { get; set; } = string.Empty;

    [BindProperty]
    [DisplayName("Address")]
    public string? ManufacturerAddress { get; set; } = string.Empty;
    [BindProperty]
    [DisplayName("URL")]
    public string? ManufacturerUrl { get; set; } = string.Empty;
    [BindProperty]
    [DisplayName("Phone")]
    public string? ManufacturerPhone { get; set; } = string.Empty;
    [BindProperty]
    [DisplayName("Email")]
    public string? ManufacturerEmail { get; set; } = string.Empty;
    [BindProperty]
    [DisplayName("Logo")]
    public string? ManufacturerLogo { get; set; } = string.Empty;
    [BindProperty]
    [DisplayName("Notes")]
    public string? ManufacturerComment { get; set; } = string.Empty;
        
    public string LitDeleteMsg { get; set; } = string.Empty;

    [BindProperty] public int SelectedId { get; set; }

    public void OnGet(int id = 0)
    {
        ViewData["Title"] = "Edit Manufacturer";

        SelectedId = id;
        using var dbContext = new SiteContext();
        var item = dbContext.Manufacturer.FirstOrDefault(p => p.mpkey == id);
        if (item != null)
        {
            ManufacturerName = item.ManufacturerName;
            ManufacturerAddress = item.ManufacturerAddress;
            ManufacturerUrl = item.ManufacturerURL;
            ManufacturerPhone = item.ManufacturerPhone;
            ManufacturerEmail = item.ManufacturerEmail;
            ManufacturerLogo = item.ManufacturerLogo;
            ManufacturerComment = item.ManufacturerComment;

            // check if manufacturer has items and disable delete panel if true.

            var hasSub = dbContext.Parts.Where(p => p.PartManID == id).ToList();
            if (hasSub.Count > 0)
            {
                LitDeleteMsg += "<p>This manufacturer has parts assigned and cannot be deleted.</p>";
            }
        }


    }

    public void OnPostSave()
    {
        ViewData["Title"] = "Edit Manufacturer";

        if (!ModelState.IsValid) return;
        using var dbContext = new SiteContext();
        var item = dbContext.Manufacturer.FirstOrDefault(p => p.mpkey == SelectedId);
        if (item != null)
        {
            item.ManufacturerName = ManufacturerName;
            item.ManufacturerAddress = ManufacturerAddress;
            item.ManufacturerURL = ManufacturerUrl;
            item.ManufacturerPhone = ManufacturerPhone;
            item.ManufacturerEmail = ManufacturerEmail;
            item.ManufacturerLogo = ManufacturerLogo;
            item.ManufacturerComment = ManufacturerComment!;
        }
        dbContext.SaveChanges();
        Response.Redirect("index?mode=update");
    }

    public void OnPostDeleteData()
    {

        using var dbContext = new SiteContext();
        var item = dbContext.Manufacturer.FirstOrDefault(p => p.mpkey == SelectedId);
        if (item == null) return;
        dbContext.Manufacturer.Remove(item);
        dbContext.SaveChanges();
        Response.Redirect("index?mode=delete");
    }
}