using Forum.Data;
using Forum.Data.Interfaces;
using Forum.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static BeerAnarchists.Pages.Profile.ManageUserProfileModel;

namespace BeerAnarchists.Pages.Profile;
[Authorize]
public class DisplayProfileModel : PageModel
{
    private readonly UserManager<ForumUser> _userManager;
    private readonly IUser _userService;
    private readonly ForumDbContext _forumDbContext;

    public DisplayProfileModel(UserManager<ForumUser> userManager, ForumDbContext forumDbContext, IUser userService) {
        _userManager = userManager;
        _forumDbContext = forumDbContext;
        _userService = userService;
    }

    [BindProperty]
    public UserDataHolder CurrentUserData { get; set; }
    [BindProperty]
    public string ViewerId { get; set; }

    public async Task<ActionResult> OnGet(string userId, string viewerId) {
        if(userId is null || viewerId is null) {
            return BadRequest();
        }
        ViewerId = viewerId;
        var user = await _userService.GetUserAllInclusiceAsync(userId);
        CurrentUserData = new UserDataHolder {
            MemberSince = user.MemberSince.ToShortDateString(),
            UserId = userId,
            UserName = user.UserName,
            Alias = user.Alias,
            ImageUrl = user.ImageUrl,
            Description = user.Description,
            NumberOfPosts = user.Posts,
            NumberOfFriends = user.Friends.Count(),
            Ignored = user.Ignored,
            LatestPost = user.LastPost?.ToShortDateString(),
        };
        return Page();
    }

    public async Task<ActionResult> OnPostIgnoreUserAsync(string userId, string ignoredId) {
        _ = await _userService.AddIgnoredAsync(userId, ignoredId);
        return RedirectToPage( "./DisplayProfile", new { userId = ignoredId, viewerId = userId});
    }

    public async Task<ActionResult> OnPostAddFriendAsync(string userId, string friendId) {
        _ = await _userService.AddFriendAsync(userId, friendId);
        return RedirectToPage("./DisplayProfile", new { userId = friendId, viewerId = userId });
    }

}
