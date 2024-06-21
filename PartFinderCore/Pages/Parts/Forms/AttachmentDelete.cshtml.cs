using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PartFinderCore.Models;

namespace PartFinderCore.Pages.Parts.Forms;

public class AttachmentDeleteModel(IWebHostEnvironment environment) : PageModel
{
    [BindProperty] public string SelectedId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public bool ShowSave;


    public void OnGet(int id)
    {
        SelectedId = id.ToString();
        using var dbContext = new SiteContext();
        var attachmentData = dbContext.PartAttachment.FirstOrDefault(p => p.PApkey == id);
        if (attachmentData != null)
        {
            Message = attachmentData.DisplayName;
        }
           
    }

    public void OnPostSaveFile()
    {
        using var dbContext = new SiteContext();
        var attachmentData = dbContext.PartAttachment.FirstOrDefault(p => p.PApkey.ToString() == SelectedId);
        if (attachmentData != null)
        {
            var fileName = attachmentData.FileName;
            var webRootPath = environment.WebRootPath;
            var file = Path.GetFullPath(fileName);

            if (System.IO.File.Exists(webRootPath + file))
            {
                System.IO.File.Delete(webRootPath + file);
            }
            dbContext.PartAttachment.Remove(attachmentData);
            dbContext.SaveChanges();
            ShowSave = true;
        }
        if (ShowSave)
        {
            Response.Redirect("done");
        }
    }
}