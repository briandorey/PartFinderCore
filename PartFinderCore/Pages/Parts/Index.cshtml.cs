using Microsoft.AspNetCore.Mvc.RazorPages;
using PartFinderCore.Models;
using System.Text;

namespace PartFinderCore.Pages.parts;

public class IndexModel : PageModel
{
    public string TextFolderTree { get; set; } = string.Empty;
    public void OnGet()
    {
        ViewData["Title"] = "Parts";

        using SiteContext db = new();
        List<PartCategory> data = [.. db.PartCategory.OrderByDescending(c => c.PCName)];
        StringBuilder sb = new();
        MakeData(0, sb, data);
        TextFolderTree = sb.ToString();

    }
    private static void MakeData(int pkey, StringBuilder sb, IReadOnlyCollection<PartCategory> ds)
    {
        var dv = ds.Where(p => p.ParentID == pkey).OrderBy(p => p.PCName).ToList();
        if (dv.Count > 0)
        {
            sb.Append("<ul>");
            foreach (var item in dv)
            {

                sb.Append("<li><a href=\"javascript:buildList(" + item.PCpkey + ",'" + item.PCName + "');\"><i class=\"bi bi-folder me-2\" aria-hidden=\"true\"></i>" + item.PCName + "</a>");

                if (ds.Where(p => p.ParentID == item.PCpkey).ToList().Count > 0)
                {
                    MakeData(item.PCpkey, sb, ds);
                }
                sb.Append("</li>");
            }
          
            sb.Append("</ul>");
        }
    }
}