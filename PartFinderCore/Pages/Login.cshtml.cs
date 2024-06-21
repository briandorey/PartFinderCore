using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PartFinderCore.Classes;
using PartFinderCore.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using static PartFinderCore.Classes.DataBaseInit;

namespace PartFinderCore.Pages;

[ValidateAntiForgeryToken]
public class LoginModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Please provide username", AllowEmptyStrings = false)]
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Please provide password", AllowEmptyStrings = false)]
    public string Password { get; set; } = string.Empty;

    [BindProperty] public bool RememberMe { get; set; }

    public string ReturnText { get; set; } = string.Empty;
    public string SavedPassword { get; set; } = string.Empty;

    public const string Key = "b14ca5898sadwwqfbbce2ea2315a1916";

    public async void OnGet()
    {
        // check for first run

        var dbContext = new SiteContext();
        if (!await dbContext.Database.GetService<IRelationalDatabaseCreator>().ExistsAsync())
        {
            await dbContext.Database.EnsureCreatedAsync();
            InitDataBaseValues();
        }

        var userList = await dbContext.Users.ToListAsync();
        if (userList.Count < 1)
        {
            ReturnText =
                "You must add an admin user to sign in. Please enter your email address and a password below.";
        }

        // check for login cookies
        var loginCookieUser = HttpContext.Request.Cookies["PartfinderUser"];
        if (loginCookieUser == null) return;
        var loginCookiePass = HttpContext.Request.Cookies["PartfinderUser"];
        if (string.IsNullOrEmpty(loginCookiePass)) return;
        var decryptedInput = Secure.DecryptString(Key, loginCookiePass);
        if (!decryptedInput.Contains(':')) return;
        var words = decryptedInput.Split(':');
        if (words.Length != 2) return;
        Username = words[0];
        SavedPassword = words[1];
        RememberMe = true;

        if (Username.Length > 1)
        {
            await DoLogin(Username, SavedPassword);
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await DoLogin(Username, Password);
        return Page();
    }

    public async Task<IActionResult> DoLogin(string username, string password)
    {
        var dbContext = new SiteContext();
        // check if user table has records. Add new user if empty.
        var userList = await dbContext.Users.ToListAsync();
        if (userList.Count < 1)
        {
            // add new user mode
                
            Users usr = new() { Username = username, UserPass = Secure.ComputeSha256Hash(password) };
            dbContext.Users.Add(usr);
            await dbContext.SaveChangesAsync();

            // login is ok
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, username),
                new(ClaimTypes.NameIdentifier, "idnumber")
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Login");
            if (RememberMe)
            {
                var passString = username + ":" + password;
                var encStr = Secure.EncryptString(Key, passString);

                var options = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddMonths(6)
                };

                HttpContext.Response.Cookies.Append("PartfinderUser", encStr, options);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true
                    });

                return RedirectToPage("/index");
            }
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
            return RedirectToPage("/index");
          
        }

        // check login
        var user = await dbContext.Users
            .FirstOrDefaultAsync(x => x.Username == Username);

        if (user == null)
        {
            ReturnText = "Username or password did not match.";
            return Page();
        }

           
        if (0 == string.CompareOrdinal(user.UserPass, Secure.ComputeSha256Hash(password)))
        {
            // login is ok
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, username),
                new(ClaimTypes.NameIdentifier, "idnumber")
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Login");
            if (RememberMe)
            {
                var passString = username + ":" + password;
                var encStr = Secure.EncryptString(Key, passString);

                var options = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddMonths(6)
                };

                HttpContext.Response.Cookies.Append("PartfinderUser", encStr, options);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true
                    });
                return RedirectToPage("/index");
            }
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
                return RedirectToPage("/index");
        }
        ReturnText = "Username or password did not match.";
        return Page();
    }
}