using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace BeerAnarchists.Pages.Admin;

[Authorize(Roles = "Admin")]
public class ReportedPostsModel : PageModel
{
    public void OnGet()
    {
    }
}
