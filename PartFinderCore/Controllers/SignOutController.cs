using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace PartFinderCore.Controllers;

public class SignOutController : Controller
{
    public async Task<IActionResult> IndexAsync()
    {
        if (HttpContext.Request.Cookies.Count > 0)
        {
            var siteCookies = HttpContext.Request.Cookies.Where(c => c.Key.Contains(".AspNetCore.") || c.Key.Contains("Microsoft.Authentication"));
            foreach (var cookie in siteCookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }
        }

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Session.Clear();
        return RedirectToPage("/Login");
    }
}