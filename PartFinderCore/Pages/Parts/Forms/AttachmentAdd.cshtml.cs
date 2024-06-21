using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PartFinderCore.Classes;
using PartFinderCore.Models;

namespace PartFinderCore.Pages.Parts.Forms;

[ValidateAntiForgeryToken]
public class AttachmentAddModel(IWebHostEnvironment environment) : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Display Name is required.")]
    [DisplayName("Display Name")]
    public string DisplayName { get; set; } = string.Empty;

    public IFormFile? Upload { get; set; }
    [BindProperty] public string SelectedId { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;

    public bool ShowSave;

    public void OnGet(int id = 0)
    {
        SelectedId = id.ToString();
    }

    public void OnPostSaveFile()
    {
        if (Upload != null && ModelState.IsValid)
        {
            if (Upload.Length <= 0 || Upload.FileName.Length <= 0) return;

            var newFileName = Upload.FileName.ToLower();
            if (SelectedId.Length > 0)
            {
                using var dbContext = new SiteContext();

                if (dbContext.Parts.Select(p => new PartViewModel
                    {
                        PartPkey = p.PartPkey,
                        PartCategoryID = p.PartCategoryID,
                        PartFootprintID = p.PartFootprintID,
                        PartManID = p.PartManID,
                        PartName = p.PartName,
                        PartDescription = p.PartDescription!,
                        PartComment = p.PartComment,
                        StockLevel = p.StockLevel,
                        MinStockLevel = p.MinStockLevel,
                        Price = p.Price,
                        DateCreated = p.DateCreated,
                        DateUpdated = p.DateUpdated,
                        Condition = p.Condition,
                        StorageLocationID = p.StorageLocationID,
                        MPN = p.MPN!,
                        BarCode = p.BarCode!,
                        StorageName = p.StorageLocation!.StorageName,
                        StorageSortOrder = p.StorageLocation.StorageSortOrder,
                        ManufacturerName = p.Manufacturer!.ManufacturerName,
                        FootprintName = p.Footprint!.FootprintName,
                        FootprintImage = p.Footprint.FootprintImage!,
                        PCName = p.PartCategory!.PCName
                    }).FirstOrDefault(p => p.PartPkey.ToString() == SelectedId) is { } itemData)
                {
                    var fileExt = Path.GetExtension(newFileName).ToLower();
                      
                    var mimeType = fileExt.ToUpper().Replace(".", "");

                       

                    var folderManName = itemData.ManufacturerName!.DirectoryName();
                    var folderPartName = itemData.PartPkey.ToString().DirectoryName();
                    var file = Path.Combine(environment.WebRootPath, "docs", folderManName, folderPartName, newFileName);
                    var saveFile = Path.Combine("/docs", folderManName, folderPartName, newFileName);
                    var folder = Path.Combine(environment.WebRootPath, "docs", folderManName, folderPartName);
                    var exists = Directory.Exists(folder);
                    if (!exists)
                    {
                        Directory.CreateDirectory(folder);
                    }
                    if (FileHelpers.HasValidExtension(file))
                    {
                        using (var fileStream = new FileStream(file, FileMode.Create))
                        {
                            Upload.CopyTo(fileStream);
                        }
                        // insert row into database
                        PartAttachment partAttachment = new()
                        {
                            FileName = saveFile,
                            DisplayName = DisplayName,
                            MIMEType = mimeType,
                            PartID = int.Parse(SelectedId)
                        };

                        dbContext.PartAttachment.Add(partAttachment);
                        dbContext.SaveChanges();

                        ShowSave = true;

                        ErrorMessage = "Your file has been added.";
                    }
                    else
                    {
                        ErrorMessage = "The selected file type is now allowed to be added.";
                    }
                    // end itemdata null check
                }
            }
        }
        else
        {
            ErrorMessage = "Please select a file and enter the display name.";
        }
        if (ShowSave)
        {
            Response.Redirect("done");
        }
    }

    
}