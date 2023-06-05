using Forum.Data.Interfaces;
using Forum.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeerAnarchists.Pages.SubForum;
[Authorize(Roles = "Admin")]
public class NewSubForumModel : PageModel {
    private readonly UserManager<Forum.Data.Models.ForumUser> _userManager;
    private readonly ISubforum _subForumService;

    #region InputFields
    [BindProperty]
    public string UserId { get; set; }
    [BindProperty]
    public string SubForumName { get; set; }
    [BindProperty]
    public string? SubForumDescription { get; set; }
    #endregion

    public NewSubForumModel(UserManager<ForumUser> userManager, ISubforum subForumService) {
        _userManager = userManager;
        _subForumService = subForumService;
    }

    public async Task<ActionResult> OnGet(string userId) {
        if (userId == null || userId == string.Empty) {
            return BadRequest();
        }
        UserId = userId;
        return Page();
    }

    public async Task<ActionResult> OnPostAsync() {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        ForumUser? User = await _userManager.FindByIdAsync(UserId);
        if (User == null) {
            return BadRequest($"Unable to find {UserId}");
        }
        var newSubForum = new Forum.Data.Models.SubForum() {
            Title = SubForumName,
            Description = SubForumDescription,
            Author = User.Alias ?? User.UserName ?? "Unknown",
            CreationDate = DateTime.Now,
            ImageUrl = string.Empty,
        };

        await _subForumService.Create(newSubForum);

        return RedirectToPage($"/Index");
    }
}
