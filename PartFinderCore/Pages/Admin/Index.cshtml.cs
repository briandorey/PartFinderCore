using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PartFinderCore.Pages.Admin;

public class IndexModel : PageModel
{
    public void OnGet()
    {
        ViewData["Title"] = "Admin";
    }
}