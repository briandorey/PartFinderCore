using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PartFinderCore.Models;

namespace PartFinderCore.Pages.Parts.Forms;

public class ParameterAddModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Name is required.")]
    [DisplayName("Param Name")]
    public string ParamName { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Value is required.")]
    [DisplayName("Value")]
    public string ParamValue { get; set; } = string.Empty;

    [BindProperty] public string SelectedId { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;

    public void OnGet(int id = 0)
    {
        SelectedId = id.ToString();
    }

    public void OnPostSaveData()
    {
        if (ModelState.IsValid)
        {
            using var dbContext = new SiteContext();
            PartParameter partParameter = new() { ParamName = ParamName, ParamValue = ParamValue, PartID= int.Parse(SelectedId) };
            dbContext.PartParameter.Add(partParameter);
            dbContext.SaveChanges();
            Response.Redirect("done");
        }
    }
}