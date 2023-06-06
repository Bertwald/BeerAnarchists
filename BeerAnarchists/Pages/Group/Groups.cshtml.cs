using Forum.Data.Interfaces;
using Forum.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeerAnarchists.Pages.Groups;
[Authorize]
public class GroupsModel : PageModel {
    private readonly IUser _userservice;
    private readonly UserManager<ForumUser> _userManager;

    public List<ForumUser> Friends { get; set; }
    public List<Forum.Data.Models.Group> OwnedGroups { get; set; }
    public List<Forum.Data.Models.Group> Applications { get; set; }
    public List<Forum.Data.Models.Group> Invitations { get; set; }

    public GroupsModel(IUser userservice, UserManager<ForumUser> userManager) {
        _userservice = userservice;
        _userManager = userManager;
    }

    public async Task<ActionResult> OnGet(string userId) {
        var user = await _userservice.GetUserAllInclusiceAsync(userId);
        if (user == null) {
            return BadRequest();
        }
        Friends = user.Friends.ToList();
        OwnedGroups = user.OwnedGroups.ToList();
        Applications = user.Applications.ToList();
        Invitations = user.Invitations.ToList();

        return Page();
    }
}
