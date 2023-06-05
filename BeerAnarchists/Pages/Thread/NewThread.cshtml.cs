using Forum.Data.Interfaces;
using Forum.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeerAnarchists.Pages.Thread;

public class NewThreadModel : PageModel
{
    private readonly UserManager<Forum.Data.Models.ForumUser> _userManager;
    private readonly IForumThread _forumThreadsService;
    private readonly ISubforum _subforumService;

    public NewThreadModel(UserManager<ForumUser> userManager, IForumThread forumThreadsService, ISubforum subforumService) {
        _userManager = userManager;
        _forumThreadsService = forumThreadsService;
        _subforumService = subforumService;
    }


    #region InputFields
    [BindProperty]
    public string UserId { get; set; }
    [BindProperty]
    public int SubForumId { get; set; }
    [BindProperty]
    public string ThreadName { get; set; }
    [BindProperty]
    public string? ThreadDescription { get; set; }
    #endregion


    public async Task<ActionResult> OnGet(int subforumId, string userId)
    {
        if (userId == null || subforumId == null) {
            return BadRequest();
        }

        UserId = userId;
        SubForumId = subforumId;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync() {

        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        var newThread = new ForumThread() {
            Name = ThreadName,
            Description = ThreadDescription,
        };

        var user = await _userManager.FindByIdAsync(UserId);
        var subForum = _subforumService.GetById(SubForumId);

        await _forumThreadsService.AddThread(newThread);
        await _subforumService.AddThread(subForum, newThread);

        return RedirectToPage($"/SubForum/SubForum", new { id = SubForumId });
    }
}
