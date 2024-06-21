using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PartFinderCore.Models;

namespace PartFinderCore.Pages.Parts.Forms;

public class ParameterEditModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Name is required.")]
    [DisplayName("Param Name")]
    public string ParamName { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Value is required.")]
    [DisplayName("Value")]
    public string ParamValue { get; set; } = string.Empty;

    [BindProperty] public int SelectedId { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;

    public void OnGet(int id = 0)
    {
        SelectedId = id;
        using var dbContext = new SiteContext();
        var item = dbContext.PartParameter.FirstOrDefault(p => p.PPpkey == SelectedId);
        if (item != null)
        {
            ParamName = item.ParamName;
            ParamValue = item.ParamValue!;

        }
    }

    public void OnPostSaveData()
    {
        if (ModelState.IsValid)
        {
            using var dbContext = new SiteContext();
            var item = dbContext.PartParameter.FirstOrDefault(p => p.PPpkey == SelectedId);
            if (item != null)
            {
                item.ParamName = ParamName;
                item.ParamValue = ParamValue;

            }
            dbContext.SaveChanges();
            Response.Redirect("done");
        }
    }

    public void OnPostDeleteData()
    {
        using var dbContext = new SiteContext();
        var item = dbContext.PartParameter.FirstOrDefault(p => p.PPpkey == SelectedId);
        if (item != null)
        {
            dbContext.PartParameter.Remove(item);
            dbContext.SaveChanges();
            Response.Redirect("done");
        }
    }
}