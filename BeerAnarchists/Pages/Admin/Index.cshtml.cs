using Forum.Data.Models;
using Forum.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BeerAnarchists.Pages.RoleAdmin;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    #region Services & Managers
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly AdminService _adminService;

    public readonly UserManager<ForumUser> _userManager;
    #endregion
    #region Properties
    public List<ForumUser> Users { get; set; }
    public List<IdentityRole> UserRoles { get; set; }

    #endregion
    #region Bindings
    [BindProperty]
    public string RoleName { get; set; }

    [BindProperty(SupportsGet = true)]
    public string AddUserId { get; set; }

    [BindProperty(SupportsGet = true)]
    public string RemoveUserId { get; set; }

    [BindProperty(SupportsGet = true)]
    public string Role { get; set; }
    #endregion
    #region Constructors
    public IndexModel(RoleManager<IdentityRole> roleManager, UserManager<ForumUser> userManager, AdminService adm)
    {
        _roleManager= roleManager;
        _userManager= userManager;
        _adminService= adm;
    }
    #endregion

    public async Task<IActionResult> OnGetAsync()
    {
        Users = await _userManager.Users.ToListAsync();
        UserRoles = await _roleManager.Roles.ToListAsync();

        if(AddUserId != null)
        {
            var alterUser = await _userManager.FindByIdAsync(AddUserId);
            _ = await _userManager.AddToRoleAsync(alterUser, Role);
        }

        if(RemoveUserId != null)
        {
            var alterUser = await _userManager.FindByIdAsync(RemoveUserId);
            _ = await _userManager.RemoveFromRoleAsync(alterUser, Role);
        }

        return Page();
    }


    public async Task<IActionResult> OnPostAsync()
    {
        if(RoleName != null)
        {
           _adminService.AddRoleAsync(RoleName);
        }
        return RedirectToPage("./Index");
    }

}
