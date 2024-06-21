using Microsoft.AspNetCore.Mvc.RazorPages;
using PartFinderCore.Models;
using System.Text;

namespace PartFinderCore.Pages.Admin.FootprintCat;

public class TreeviewModel : PageModel
{
    private const string LinkUrl = "/admin/footprintcat/edit?id=";
    private const string AddUrl = "/admin/footprintcat/add?parent=";

    private List<FootprintCategory>? _partCategories;
    public required string PageData { get; set; }

    public void OnGet()
    {
        ViewData["Title"] = "Footprint Category Tree View";
        using var dbContext = new SiteContext();
        _partCategories = [.. dbContext.FootprintCategory.OrderBy(p => p.ParentCategory).ThenBy(p => p.FCName)];

        StringBuilder sb = new();
        MakeNav(0, sb, _partCategories, LinkUrl, AddUrl);
        PageData = sb.ToString();
    }

    public void MakeNav(int pkey, StringBuilder sb, List<FootprintCategory> ds, string linkUrl, string addUrl)
    {
        var dv = ds.Where(p => p.ParentCategory == pkey).OrderBy(p => p.FCName).ToList();

        if (dv.Count > 0)
        {
            sb.Append("<ul>" + Environment.NewLine);
            foreach (var drv in dv)
            {
                if (addUrl != null)
                {
                    sb.Append("<li><a href=\"" + linkUrl + drv.FCPkey + "\">" + drv.FCName + "</a> <a href=\"" + addUrl + drv.FCPkey + "\"><i class=\"small bi bi-plus\"></i></a>");
                }
                else
                {
                    sb.Append("<li><a href=\"" + linkUrl + drv.FCPkey + "\">" + drv.FCName + "</a>");
                }
                MakeNav(drv.FCPkey, sb, ds, linkUrl, addUrl!);
                sb.Append("</li>" + Environment.NewLine);
            }
            sb.Append("</ul>" + Environment.NewLine);
        }
    }
}