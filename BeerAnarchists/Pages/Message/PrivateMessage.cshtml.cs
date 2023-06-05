using Forum.Data.Interfaces;
using Forum.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeerAnarchists.Pages.Message;
[Authorize]
public class PrivateMessageModel : PageModel {
    private readonly UserManager<ForumUser> _userManager;
    private readonly IUser _userService;
    [BindProperty]
    public string SenderId { get; set; }
    [BindProperty]
    public string RecieverId { get; set; }
    [BindProperty]
    public string MessageTitle { get; set; }
    [BindProperty]
    public string Message { get; set; }

    public PrivateMessageModel(IUser userService, UserManager<ForumUser> userManager) {
        _userService = userService;
        _userManager = userManager;
    }

    public async Task<ActionResult> OnGet(string senderId, string recieverId) {
        //Check if they are existing users
        bool hasSender = await _userService.IsValidUser(senderId);
        bool hasReciever = await _userService.IsValidUser(recieverId);

        if (!(hasSender && hasReciever)) {
            return BadRequest();
        }

        SenderId = senderId;
        RecieverId = recieverId;
        return Page();
    }

    public async Task<ActionResult> OnPost() {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        if(SenderId == RecieverId) {
            return RedirectToPage("/Index");
        }
        PrivateMessage message = new () {
            Content = Message,
            Title = this.MessageTitle,
            Sender = await _userManager.FindByIdAsync(SenderId),
            Reciever = await _userManager.FindByIdAsync(RecieverId),
            Created = DateTime.Now
        };
        await _userService.SendMessageAsync(message);
        return RedirectToPage("/Index");
    }
}
