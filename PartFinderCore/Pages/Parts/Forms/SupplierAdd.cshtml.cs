using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PartFinderCore.Models;

namespace PartFinderCore.Pages.Parts.Forms;

public class SupplierAddModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Name is required.")]
    [DisplayName("Supplier Name")]
    public string SupplierName { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Url is required.")]
    [DisplayName("Url")]
    public string SupplierUrl { get; set; } = string.Empty;

    [BindProperty] public string SelectedId { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;

    public void OnGet(int id = 0)
    {
        SelectedId = id.ToString();
    }

    public void OnPostSaveData()
    {
        if (ModelState.IsValid)
        {
            using var dbContext = new SiteContext();
            PartSuppliers partParameter = new() { SupplierName = SupplierName, URL = SupplierUrl, PartID = int.Parse(SelectedId) };
            dbContext.PartSuppliers.Add(partParameter);
            dbContext.SaveChanges();
            Response.Redirect("done");
        }
    }
}