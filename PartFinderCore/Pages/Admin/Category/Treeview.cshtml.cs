using Microsoft.AspNetCore.Mvc.RazorPages;
using PartFinderCore.Models;
using System.Text;

namespace PartFinderCore.Pages.Admin.Category;

public class TreeviewModel : PageModel
{
    private const string LinkUrl = "/admin/category/edit?id=";
    private const string AddUrl = "/admin/category/add?parent=";

    private List<PartCategory>? _partCategories;
    public required string PageData { get; set; }

    public void OnGet()
    {
        ViewData["Title"] = "Category Tree View";
        using var dbContext = new SiteContext();
        _partCategories = [.. dbContext.PartCategory.OrderBy(p =>p.ParentID).ThenBy(p => p.PCName)];

        StringBuilder sb = new();
        MakeNav(0, sb, _partCategories, LinkUrl, AddUrl);
        PageData = sb.ToString();
    }

    public void MakeNav(int pkey, StringBuilder sb, List<PartCategory> ds, string linkUrl, string addUrl)
    {
        var dv = ds.Where(p => p.ParentID == pkey).OrderBy(p => p.PCName).ToList();
            
        if (dv.Count > 0)
        {
            sb.Append("<ul>" + Environment.NewLine);
            foreach (var drv in dv)
            {
                if (addUrl != null)
                {
                    sb.Append("<li><a href=\"" + linkUrl + drv.PCpkey + "\">" + drv.PCName + "</a> <a href=\"" + addUrl + drv.PCpkey + "\"><i class=\"small bi bi-plus\"></i></a>");
                }
                else
                {
                    sb.Append("<li><a href=\"" + linkUrl + drv.PCpkey + "\">" + drv.PCName + "</a>");
                }
                MakeNav(drv.PCpkey, sb, ds, linkUrl, addUrl!);
                sb.Append("</li>" + Environment.NewLine);
            }
            sb.Append("</ul>" + Environment.NewLine);
        }
    }
}