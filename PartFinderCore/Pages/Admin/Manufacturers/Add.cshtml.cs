using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PartFinderCore.Models;

namespace PartFinderCore.Pages.Admin.Manufacturers;

public class AddModel : PageModel
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


    public void OnGet()
    {
        ViewData["Title"] = "Add Manufacturer";
    }

    public void OnPostSave()
    {
        ViewData["Title"] = "Add Manufacturer";
           
        if (ModelState.IsValid)
        {
            using var dbContext = new SiteContext();
            Manufacturer part = new() { 
                ManufacturerName = ManufacturerName,
                ManufacturerAddress = ManufacturerAddress,
                ManufacturerURL = ManufacturerUrl,
                ManufacturerPhone = ManufacturerPhone, 
                ManufacturerEmail = ManufacturerEmail, 
                ManufacturerLogo = ManufacturerLogo, 
                ManufacturerComment = ManufacturerComment!
            };
            dbContext.Manufacturer.Add(part);
            dbContext.SaveChanges();

            Response.Redirect("index?mode=add");
        }
    }
}