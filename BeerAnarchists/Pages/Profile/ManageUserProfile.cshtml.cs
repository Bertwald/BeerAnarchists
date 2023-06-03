using Forum.Data;
using Forum.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace BeerAnarchists.Pages.Profile;
[Authorize]
public class ManageUserProfileModel : PageModel {
    private readonly UserManager<ForumUser> _userManager;
    private readonly ForumDbContext _forumDbContext;

    public ManageUserProfileModel(UserManager<ForumUser> userManager, ForumDbContext forumDbContext) {
        _userManager = userManager;
        _forumDbContext = forumDbContext;
        //CurrentUserData = new UserDataHolder() {Ignored = new List<ForumUser>()};
    }

    [BindProperty]
    public UserDataHolder CurrentUserData { get; set; }
    [BindProperty]
    public IFormFile? UserImage { get; set; }
    public async Task<ActionResult> OnGet(string userId) {
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
        };
        return Page();
    }

    public async Task<ActionResult> OnPost() {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        string fileName = string.Empty;

        if (UserImage != null) {
            if (CurrentUserData.ImageUrl != null) {
                if (System.IO.File.Exists("./wwwroot/img/" + CurrentUserData.ImageUrl)) {
                    System.IO.File.Delete("./wwwroot/img/" + CurrentUserData.ImageUrl);
                }
            }

            fileName = Guid.NewGuid() + UserImage.FileName;
            var file = "./wwwroot/img/" + fileName;
            using var fileStream = new FileStream(file, FileMode.Create);
            await UserImage.CopyToAsync(fileStream);
            CurrentUserData.ImageUrl = fileName;
        }

        var savedUser = await _forumDbContext.Users.FindAsync(CurrentUserData.UserId);
        if (savedUser != null) {
            savedUser.Alias = CurrentUserData.Alias;
            savedUser.ImageUrl = CurrentUserData.ImageUrl;
            savedUser.Description = CurrentUserData.Description;
        }
        _forumDbContext.Entry(savedUser).State = EntityState.Modified;

        try {
            await _forumDbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            throw;
        }
        return Page();//RedirectToPage($"/Profile/Profile", new { userId = CurrentUserData.UserId });
    }

    public class UserDataHolder {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string? Alias { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public string MemberSince { get; set; }
        public int NumberOfPosts { get; set; }
        public int NumberOfFriends { get; set; }
        public string LatestPost { get; set; }
        public IEnumerable<ForumUser> Ignored { get; set; }
    }
}
