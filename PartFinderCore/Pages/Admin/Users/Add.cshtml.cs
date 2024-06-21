using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PartFinderCore.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PartFinderCore.Classes;

namespace PartFinderCore.Pages.Admin.Users;

public class AddModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Username is required.")]
    [DisplayName("Username")]
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Password is required.")]
    [DisplayName("Password")]
    [DataType(DataType.Password)]
    public string UserPass { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Password confirmation is required.")]
    [DisplayName("Password Confirm")]
    [DataType(DataType.Password)]
    [Compare(nameof(UserPass))]
    public string UserPass2 { get; set; } = string.Empty;

    public void OnGet()
    {
        ViewData["Title"] = "Add User";
    }

    public void OnPostSave()
    {
        ViewData["Title"] = "Add User";

        if (ModelState.IsValid)
        {
            using var dbContext = new SiteContext();
            Models.Users part = new()
            {
                Username = Username,
                UserPass = Secure.ComputeSha256Hash(UserPass)
            };
            dbContext.Users.Add(part);
            dbContext.SaveChanges();


            Response.Redirect("index?mode=add");
        }
    }
}