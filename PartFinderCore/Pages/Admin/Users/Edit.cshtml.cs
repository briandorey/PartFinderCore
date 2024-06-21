using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PartFinderCore.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PartFinderCore.Classes;

namespace PartFinderCore.Pages.Admin.Users;

public class EditModel : PageModel
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
        
    public string UserPass2 { get; set; } = string.Empty;

    public string LitDeleteMsg { get; set; } = string.Empty;

    [BindProperty] public int SelectedId { get; set; }

    public void OnGet(int id = 0)
    {
        ViewData["Title"] = "Edit User";

        SelectedId = id;
        using var dbContext = new SiteContext();
        var userCount = dbContext.Users.Count();
        if (userCount == 1)
        {
            LitDeleteMsg = "You cannot delete the primary admin user.";
        }
        var item = dbContext.Users.FirstOrDefault(p => p.UserPkey == id);
        if (item != null)
        {
            Username = item.Username;
            UserPass = item.UserPass;
        }
    }

    public void OnPostSave()
    {
        ViewData["Title"] = "Edit User";
        if (ModelState.IsValid)
        {
            using var dbContext = new SiteContext();
            var item = dbContext.Users.FirstOrDefault(p => p.UserPkey == SelectedId);
            if (item != null)
            {
                item.Username = Username;
                item.UserPass = Secure.ComputeSha256Hash(UserPass);
            }
            dbContext.SaveChanges();

            Response.Redirect("index?mode=update");
        }
    }

    public void OnPostDeleteData()
    {
        using var dbContext = new SiteContext();
        var item = dbContext.Users.FirstOrDefault(p => p.UserPkey == SelectedId);
        if (item != null)
        {
            dbContext.Users.Remove(item);
            dbContext.SaveChanges();
            Response.Redirect("index?mode=delete");
        }
    }
}