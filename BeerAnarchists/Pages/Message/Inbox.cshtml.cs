using Forum.Data.Interfaces;
using Forum.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeerAnarchists.Pages.Message;
[Authorize]
public class InboxModel : PageModel
{
    private readonly IUser _userService;
    private readonly UserManager<ForumUser> _userManager;

    public InboxModel(IUser userService, UserManager<ForumUser> userManager) {
        _userService = userService;
        _userManager = userManager;
    }
    [BindProperty]
    public string MailboxOwnerId { get; set; }
    [BindProperty]
    public IEnumerable<PrivateMessage> Inbox { get; set; }

    public async Task<ActionResult> OnGet(string userId)
    {
        if(userId == null) {
            return BadRequest(ModelState);
        }
        var User = await _userManager.FindByIdAsync(userId);
        if (User == null) {
            return BadRequest();
        }
        MailboxOwnerId = userId;

        Inbox = (await _userService.GetInboxAsync(userId)).ToList().OrderBy(message => message.Created);

        return Page();
    }
}
