using Microsoft.AspNetCore.Mvc.RazorPages;
using PartFinderCore.Classes;
using PartFinderCore.Models;
using System.Text.RegularExpressions;

namespace PartFinderCore.Pages.Admin.Files;

public partial class DeleteModel(IWebHostEnvironment environment) : PageModel
{
    public string ErrorMessage { get; set; } = string.Empty;

    public void OnGet(string? f, string? mode)
    {
        if (f == null || mode == null) return;
        var basePath = Path.Combine(environment.WebRootPath, "docs");

        var newFolder = f.TrimStart('|').TrimEnd('|');
        newFolder = newFolder.Replace("|", Path.DirectorySeparatorChar.ToString());

        string sanitizedPath = PathSanitizer.SanitizePath(basePath, newFolder);

        if (mode == "file")
        {
            if (System.IO.File.Exists(sanitizedPath))
            {
                if (sanitizedPath.StartsWith(environment.WebRootPath))
                {
                    // in web folder
                    FileInfo fi = new(sanitizedPath);
                    var strFilename = fi.Name;

                    System.IO.File.Delete(sanitizedPath);

                    // check if file is used as an attachment in database and delete if found
                    using var dbContext = new SiteContext();
                    var attachmentData = dbContext.PartAttachment.FirstOrDefault(p => sanitizedPath.Contains(p.FileName));
                    if (attachmentData != null)
                    {
                        dbContext.PartAttachment.Remove(attachmentData);
                        dbContext.SaveChanges();
                    }

                    Response.Redirect("/admin/files/index?d=" + sanitizedPath.Replace(strFilename, "").Replace(basePath, "")
                        .Replace(Path.DirectorySeparatorChar.ToString(), "|").TrimStart('|').TrimEnd('|'));
                }
            }
            else
            {
                ErrorMessage = "File Not Found Error";
            }

        }
        if (mode == "folder")
        {

            if (Directory.Exists(sanitizedPath))
            {
                if (sanitizedPath.StartsWith(environment.WebRootPath))
                {
                    Directory.Delete(sanitizedPath, true);

                    Response.Redirect("/admin/files/index");
                }
            }

            else
            {
                ErrorMessage = "Directory Not Found Error";
            }
        }
    }
}