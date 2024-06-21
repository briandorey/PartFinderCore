using Ganss.Xss;
using System.Text;
using System.Text.RegularExpressions;
namespace PartFinderCore.Classes;

public static partial class Helpers
{
    public static string DoFmSizeFormat(long inValue)
    {
        var filesize = Convert.ToDouble(inValue);

        var displayValue = filesize switch
        {
            < 1024 => filesize.ToString("F") + " B",
            >= 1024 => (filesize / 1024).ToString("F") + " KB",
            _ => ""
        };
        if (filesize >= 1048576)
        {
            displayValue = (filesize / 1024 / 1024).ToString("F") + " MB";
        }

        displayValue = filesize switch
        {
            >= 1073741824 => (filesize / 1024 / 1024 / 1024).ToString("F") + " GB",
            < 1 => "0 B",
            _ => displayValue
        };

        return displayValue;

    }


    // check if image file name exists and return image if available
    public static string CheckImage(string inval)
    {
        if (inval != null && inval.Trim().Length > 1) { 
        
            return "<img src=\"" + inval + "\" class=\"img-fluid\" style=\"height: 50px;\" />";
        }
 
        return "";
    }

    // Get coondition of part
    public static string GetCondition(int key)
    {
        return key switch
        {
            0 => "Used",
            1 => "New",
            _ => "Unknown"
        };
    }

    // Sanitize inputs
    public static string CleanInput(this string strParam)
    {
        var sanitizer = new HtmlSanitizer();
        return sanitizer.Sanitize(strParam);
    }

    public static string DirectoryName(this string inval)
    {
        return MyRegexDirectoryName().Replace(inval, "").ToLower().Replace(" ", "-");
    }

     
    [GeneratedRegex("[^A-Za-z0-9]+")]
    private static partial Regex MyRegexDirectoryName();

    // Built Pagination for paging data
    public static string GetPagination(int totalPages, int currentPage, string pageUrl, string extraParam)
    {
        StringBuilder sb = new();
        sb.Append("<div class=\"row\">" + Environment.NewLine);
        sb.Append("    <div class=\"col-12\">" + Environment.NewLine);
        sb.Append("        <hr />" + Environment.NewLine);
        sb.Append("        <nav aria-label=\"Page navigation\">" + Environment.NewLine);
        sb.Append("            <ul class=\"pagination pagination-sm  justify-content-center\">" + Environment.NewLine);
        for (var i = 1; i <= totalPages; i++)
        {
            if (i == currentPage)
            {
                sb.Append("<li class=\"page-item active\"><a href=\"#\" class=\"page-link\">" + i + "</a></li>" + Environment.NewLine);
            } else
            {
                sb.Append("<li class=\"page-item\"><a href=\"/" + pageUrl + "?pagenumber=" + i + "&" + extraParam + "\" class=\"page-link\">" + i + "</a></li>" + Environment.NewLine);
            }
        }
        sb.Append("            </ul>" + Environment.NewLine);
        sb.Append("        </nav>" + Environment.NewLine);
        sb.Append("    </div>" + Environment.NewLine);
        sb.Append("</div>" + Environment.NewLine);
        return sb.ToString();
    }
}