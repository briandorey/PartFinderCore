using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using PartFinderCore.Classes;

namespace PartFinderCore.Pages.Admin.Files;

public partial class IndexModel(IWebHostEnvironment environment) : PageModel
{
    public string BasePath = "|";
    private string _fullUrl = "|";
    public string CurrentPath = "|";

    public string TextBreadcrumb { get; set; } = string.Empty;
    public string TextFolderTree { get; set; } = string.Empty;

    public IFormFile? Upload { get; set; }


    public string NewFolderName { get; set; } = string.Empty;

    [BindProperty] public string SelectedFolder { get; set; } = "";

    public string ErrorMessage { get; set; } = string.Empty;

    public List<FileListings> FileList = [];

    private string _userSettings = "list";

    public void OnGet(string? d = "")
    {
        ViewData["Title"] = "File Manager";
        BasePath = Path.Combine(environment.WebRootPath, "docs");
        LoadFiles(d);
    }

    public void LoadFiles(string? d = "")
    {
        ModelState.Clear();


        if (Request.Cookies["adminFileManagerView"] != null)
        {
            _userSettings = Request.Cookies["adminFileManagerView"]!;
        }
        if (d is { Length: > 1 })
        {
            SelectedFolder = "";
            // check for only lower case letters, numbers and | character
            string pattern = "^[a-z0-9|]+$";
            var isMatch = Regex.IsMatch(d, pattern);
            if (isMatch)
            {
                CurrentPath = ReturnSafePath(d);

                char[] myChar = ['|'];

                SelectedFolder = d.TrimStart(myChar);


                var currentPath = d.Replace("\\", "|").Replace("/", "|");
                _fullUrl = d;


                // litFolderTree.Text = GetDirectories(Server.MapPath("\\docs\\" + FullUrl), 1);


                StringBuilder sb = new();


                currentPath = currentPath.TrimStart(myChar);
                var words = currentPath.Split('|');
                var totalLength = words.Length;
                if (currentPath.Length == 0)
                {
                    totalLength = 0;
                }


                var tmpCounter = 0;

                if (totalLength == 0)
                {
                    sb.Append(
                        "<li  class=\"breadcrumb-item\"><a href=\"/admin/files/\">Files</a></li><li class=\"breadcrumb-item active\">docs</li>");
                }
                else
                {
                    sb.Append(
                        "<li  class=\"breadcrumb-item\"><a href=\"/admin/files/\">Files</a></li><li  class=\"breadcrumb-item\"><a href=\"/admin/files/\">docs</a></li>");


                    foreach (var word in words)
                    {
                        if (word.Trim().Length <= 1) continue;
                        if (tmpCounter == totalLength - 1)
                        {
                            sb.Append("<li class=\"breadcrumb-item active\">" + word + "</li>");
                        }
                        else
                        {
                            sb.Append("<li  class=\"breadcrumb-item\"><a href=\"/admin/files/index?d=" +
                                      TrimUrl(_fullUrl, word) + "\">" + word + "</a></li>");
                        }

                        tmpCounter++;
                    }
                }

                TextBreadcrumb = sb.ToString();
            }
        }
        else
        {
            TextBreadcrumb =
                "<li  class=\"breadcrumb-item\"><a href=\"/admin/files/\">Files</a></li><li class=\"breadcrumb-item active\">docs</li>";
        }

        var strDir = "";
        try
        {
            if (CurrentPath.Length != 0)
            {
                strDir = ReturnSafePath(CurrentPath);
            }
            /*
             * Debug for paths 
            ErrorMessage += "<br>CurrentPath: " + CurrentPath;
            ErrorMessage += "<br>BasePath: " + BasePath;
            ErrorMessage += "<br>File Listing strDir: " + strDir;


            ErrorMessage += "<br>File Listing Path: " + Path.Combine(BasePath, strDir);

            */
            //string strParent = strDir[..strDir.LastIndexOf("\\")];
            DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine(BasePath, strDir));
            //DirectoryInfo[] subDirs = DirInfo.GetDirectories();
            var files = dirInfo.GetFiles();


            // do files

            for (var i = 0; i <= files.Length - 1; i++)
            {
                FileListings fileListings = new()
                {


                    FileName = files[i].Name,
                    FolderName = files[i].FullName.Replace(BasePath, ""),
                    FileSize = Helpers.DoFmSizeFormat(files[i].Length),
                    FileDate = files[i].LastWriteTime
                };
                FileList.Add(fileListings);
            }
        }
        catch (Exception)
        {
            //SetPageTitle("Error retrieving directory info: "+e.Message);
        }
        var sbtree = new StringBuilder();

        ListDirectories(BasePath, sbtree, BasePath);
        TextFolderTree = sbtree.ToString();

        //TextFolderTree = DirectoryLister.GenerateJson(BasePath, BasePath, CurrentPath);

    }

    public string TrimUrl(string link, string lastLink)
    {
        var parts = link.Split('|');
        StringBuilder linker = new();
        foreach (var part in parts)
        {
            linker.Append("|" + part);
            if (part.Equals(lastLink))
            {
                return linker.ToString().TrimStart('|');
            }
        }

        return linker.ToString();
    }

    public string GetFolderPath()
    {
        return CurrentPath.Length != 0 ? CurrentPath.Replace(Path.DirectorySeparatorChar.ToString(), "|") : "";
    }


    public string MakeIcon(string fileName, bool popup)
    {
        fileName = Path.DirectorySeparatorChar.ToString() + "docs" + fileName.Replace("|", Path.DirectorySeparatorChar.ToString());
        if (fileName.ToLower().EndsWith("jpg") || fileName.EndsWith("gif") || fileName.EndsWith("png") || fileName.EndsWith("svg"))
        {
            if (popup)
            {
                return fileName.Replace("\\", "\\\\");
            }

            return fileName;
        }


        return Path.DirectorySeparatorChar.ToString() + "img/file.svg";
    }



    public void OnPostSave(string? newFolderName)
    {
        //ErrorMessage = NewFolderName + " error";

        if (!string.IsNullOrEmpty(newFolderName))
        {
            string pattern = "^[a-zA-Z0-9.]+$";

            bool isValid = Regex.IsMatch(newFolderName, pattern);
            if (isValid)
            {
                var newDirName = newFolderName.ToLower();
                BasePath = Path.Combine(environment.WebRootPath, "docs");
                if (SelectedFolder != null)
                {
                    var newpath = SelectedFolder.Replace('|', Path.DirectorySeparatorChar);

                    string newPath = Path.Combine(BasePath, newpath, newDirName);
                    Directory.CreateDirectory(newPath);
                    Response.Redirect("/admin/files/index?d=" + SelectedFolder + "|" + newDirName);
                }
                else
                {
                    string newPath = Path.Combine(BasePath, newDirName);
                    Directory.CreateDirectory(newPath);
                    Response.Redirect("/admin/files/index?d=" + newDirName);
                }
            }
            else
            {
                ErrorMessage = "Folder name is not valid";
            }

        }
    }

    public async Task OnPostSaveFileAsync()
    {
        if (Upload != null)
        {
            if (Upload.Length <= 0 || Upload.FileName.Length <= 0) return;

            var newFileName = Upload.FileName.ToLower();
            if (SelectedFolder != null && SelectedFolder.Length > 2)
            {
                var newFolder = ReturnSafePath(SelectedFolder);
                var file = Path.Combine(environment.WebRootPath, "docs", newFolder, newFileName);


                if (FileHelpers.HasValidExtension(file))
                {
                    await using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await Upload.CopyToAsync(fileStream);
                    }

                    Response.Redirect("/admin/files/index?d=" + SelectedFolder);
                }
                else
                {
                    ErrorMessage = "Invalid file type";
                }
            }
            else
            {
                var file = Path.Combine(environment.WebRootPath, "docs", newFileName);
                ErrorMessage = file;
                await using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await Upload.CopyToAsync(fileStream);
                }

                Response.Redirect("/admin/files/index?d=" + SelectedFolder);
            }
        }
        else
        {
            
            ErrorMessage = "Please return to the previous page and select a file to upload.";
        }
    }



    public string EncodeUrl(string url)
    {
        return url.Length > 0 ? WebUtility.UrlEncode(url) : "";
    }

    public FileResult? OnGetDownloadFile(string fileName)
    {
        
        string pattern = "^(?!.*\\..*\\.)([a-z0-9/\\\\.]+)$";
        fileName = WebUtility.UrlDecode(fileName);
        fileName = ReturnSafePath(fileName);
        ErrorMessage = fileName;
        bool isValid = Regex.IsMatch(fileName, pattern);
        if (isValid)
        {
            
            
            TextBreadcrumb = fileName;

            //Build the File Path.
            var path = Path.Combine(environment.WebRootPath, "docs", fileName);
            FileInfo fi = new(path);
            var strFilename = fi.Name;
            //Read the File data into Byte Array.
            var bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", strFilename);
        }

        return null;
    }

    void ListDirectories(string path, StringBuilder sb, string BasePath)
    {

        var directories = Directory.GetDirectories(path);

        if (directories.Any())
        {
            Array.Sort(directories);

            sb.AppendLine("<ul>");
            foreach (var directory in directories)
            {
                var di = new DirectoryInfo(directory);

                if (MakeSafePath(CurrentPath).Equals(MakeSafePath(di.FullName.Replace(BasePath, ""))))
                {
                    sb.AppendFormat("    <li  class=\"treeactive\"><a href=\"/admin/files/index?d=" + MakeSafePath(di.FullName.Replace(BasePath, "")) + "\" data=\"" + MakeSafePath(di.FullName.Replace(BasePath, "")) + "\"><i class=\"bi bi-folder2-open me-2 text-dark\" aria-hidden=\"true\"></i><strong class=\" text-dark\">{0}</strong></a>", di.Name);
                }
                else
                {
                    sb.AppendFormat("    <li><a href=\"/admin/files/index?d=" + MakeSafePath(di.FullName.Replace(BasePath, "")) + "\" data=\"" + MakeSafePath(di.FullName.Replace(BasePath, "")) + "\"><i class=\"bi bi-folder me-2\" aria-hidden=\"true\"></i>{0}</a>", di.Name);
                }



                sb.AppendLine();
                ListDirectories(directory, sb, BasePath);
                sb.AppendLine("</li>");
            }
            sb.AppendLine("</ul>");
        }
    }

    public string MakeSafePath(string path)
    {
        return path.Replace(Path.DirectorySeparatorChar.ToString(), "|").TrimStart('|');
    }
    private string ReturnSafePath(string path)
    {
        return path.Replace("|", Path.DirectorySeparatorChar.ToString()).TrimStart(Path.DirectorySeparatorChar);
    }

    public string CheckListMode(bool isIconMode)
    {
        switch (_userSettings)
        {
            case "icon" when isIconMode:
            case "list" when !isIconMode:
                return "";
            default:
                return "d-none";
        }
    }
}

public class FileListings
{
    public string FileName { get; init; } = string.Empty;
    public string FolderName { get; init; } = string.Empty;
    public string FileSize { get; init; } = string.Empty;
    public DateTime FileDate { get; init; } = DateTime.Now;
}