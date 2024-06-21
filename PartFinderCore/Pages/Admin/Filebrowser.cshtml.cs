using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;

namespace PartFinderCore.Pages.Admin;

public partial class FilebrowserModel(IWebHostEnvironment environment) : PageModel
{
    public string ErrorMessage { get; set; } = string.Empty;
    public bool ShowUp { get; set; }
    public string CurrentPath { get; set; } = string.Empty;
    public string ParentPath { get; set; } = string.Empty;
    public List<BrowserFile> FileList = [];
    public List<BrowserDirectory> DirectoryList = [];

    public string? FormFieldname { get; set; }
    public void OnGet(string d = "", string fieldname = "")
    {
        FormFieldname = fieldname.ToLower();
        var basePath = Path.Combine(environment.WebRootPath, "docs");
        if (!string.IsNullOrEmpty(d))
        {
            var isMatch = MyRegex().IsMatch(d);
            if (isMatch)
            {
                ShowUp = true;
                CurrentPath = d;
            }
        }
        else
        {
            CurrentPath = "\\";
        }

        ParentPath = CurrentPath[..CurrentPath.LastIndexOf('\\')];

        var directories = Directory.GetDirectories(basePath + CurrentPath);
        if (directories.Length != 0)
        {
            foreach (var directory in directories)
            {
                var di = new DirectoryInfo(directory);
                BrowserDirectory browserDirectory = new()
                {
                    FileDirectory = di.Name,
                    FileDirectoryPath = di.FullName.Replace(basePath, "")
                };
                DirectoryList.Add(browserDirectory);
            }
        }

        DirectoryInfo dirInfo = new(basePath + CurrentPath);
        var files = dirInfo.GetFiles();

        for (var i = 0; i <= files.Length - 1; i++)
        {
            BrowserFile fileListings = new()
            {
                FileName = files[i].Name,
                FilePath = GetFolderPath().TrimStart('\\') + "/" + files[i].Name
            };
            FileList.Add(fileListings);
        }

    }

    public string GetFolderPath()
    {
        return CurrentPath.Replace(" ", "").Replace("\\", "/");

    }

    public string MakeIcon(string fileName)
    {
        if (fileName.ToLower().EndsWith("jpg") || fileName.EndsWith("gif") || fileName.EndsWith("svg") || fileName.EndsWith("png") || fileName.EndsWith("webp"))
        {
            return "/docs/" + fileName;
        }

        return "/img/file.svg";
    }
    [GeneratedRegex(@"^[a-zA-Z0-9\\-]+$")]
    private static partial Regex MyRegex();
}
public class BrowserFile
{
    public string FileName { get; init; } = string.Empty;
    public string FilePath { get; init; } = string.Empty;
}
public class BrowserDirectory
{
    public string FileDirectory { get; init; } = string.Empty;
    public string FileDirectoryPath { get; init; } = string.Empty;
}