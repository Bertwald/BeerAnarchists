using Forum.Data.Interfaces;
using Forum.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeerAnarchists.Pages.Group;
[Authorize]
public class GroupModel : PageModel
{
    private readonly IUser _userservice;
    private readonly UserManager<ForumUser> _userManager;

    public GroupModel(IUser userservice, UserManager<ForumUser> userManager) {
        _userservice = userservice;
        _userManager = userManager;
    }

    public void OnGet()
    {
    }
}
