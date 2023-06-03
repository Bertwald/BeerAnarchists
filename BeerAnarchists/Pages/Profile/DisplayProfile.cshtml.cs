using Forum.Data.Models;
using Forum.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static BeerAnarchists.Pages.Profile.ManageUserProfileModel;
using Microsoft.AspNetCore.Authorization;

namespace BeerAnarchists.Pages.Profile;
[Authorize]
public class DisplayProfileModel : PageModel
{
    private readonly UserManager<ForumUser> _userManager;
    private readonly ForumDbContext _forumDbContext;

    public DisplayProfileModel(UserManager<ForumUser> userManager, ForumDbContext forumDbContext) {
        _userManager = userManager;
        _forumDbContext = forumDbContext;
    }

    [BindProperty]
    public UserDataHolder CurrentUserData { get; set; }
    [BindProperty]
    public string ViewerId { get; set; }

    public async Task<ActionResult> OnGet(string userId, string viewerId) {
        ViewerId = viewerId;
        var user = await _userManager.FindByIdAsync(userId);
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

}
