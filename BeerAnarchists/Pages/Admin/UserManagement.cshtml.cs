using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeerAnarchists.Pages.Admin;
[Authorize(Roles = "Admin")]
public class UserManagementModel : PageModel
{
    public void OnGet()
    {
    }
}
