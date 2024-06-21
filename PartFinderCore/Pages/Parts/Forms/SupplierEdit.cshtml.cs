using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PartFinderCore.Models;

namespace PartFinderCore.Pages.Parts.Forms;

public class SupplierEditModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Name is required.")]
    [DisplayName("Supplier Name")]
    public string SupplierName { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "URL is required.")]
    [DisplayName("URL")]
    public string SupplierUrl { get; set; } = string.Empty;

    [BindProperty] public int SelectedId { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;

    public void OnGet(int id = 0)
    {
        SelectedId = id;
        using var dbContext = new SiteContext();
        var item = dbContext.PartSuppliers.FirstOrDefault(p => p.SupPkey == SelectedId);
        if (item == null) return;
        SupplierName = item.SupplierName;
        SupplierUrl = item.URL;
    }

    public void OnPostSaveData()
    {
        if (!ModelState.IsValid) return;
        using var dbContext = new SiteContext();
        var item = dbContext.PartSuppliers.FirstOrDefault(p => p.SupPkey == SelectedId);
        if (item != null)
        {
            item.URL = SupplierUrl;
            item.SupplierName = SupplierName;

        }
        dbContext.SaveChanges();
        Response.Redirect("done");
    }

    public void OnPostDeleteData()
    {
        using var dbContext = new SiteContext();
        var item = dbContext.PartSuppliers.FirstOrDefault(p => p.SupPkey == SelectedId);
        if (item != null)
        {
            dbContext.PartSuppliers.Remove(item);
            dbContext.SaveChanges();
            Response.Redirect("done");

        }

    }
}