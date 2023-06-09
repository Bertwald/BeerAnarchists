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
    public string MessageType { get; set; }
    [BindProperty]
    public string SenderId { get; set; }
    [BindProperty]
    public string? RecieverId { get; set; }
    [BindProperty]
    public int RecieverGroupId { get; set; }
    [BindProperty]
    public string MessageTitle { get; set; }
    [BindProperty]
    public string Message { get; set; }

    public PrivateMessageModel(IUser userService, UserManager<ForumUser> userManager) {
        _userService = userService;
        _userManager = userManager;
    }

    public async Task<ActionResult> OnGet(string senderId, string recieverId) {
        MessageType = "Private Message";
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

    public async Task<ActionResult> OnGetGroupMessageAsync(string userId, int groupId) {
        MessageType = "Group Message";
        bool hasSender = await _userService.IsValidUser(userId);
        if (!hasSender || groupId == 0) {
            return BadRequest();
        }
        SenderId = userId;
        RecieverGroupId = groupId;
        return Page();
    }

    public async Task<ActionResult> OnPost() {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        Forum.Data.Models.Message newMessage;

        if (MessageType == "Private Message") {
            if (SenderId == RecieverId) {
                return RedirectToPage("/Index");
            }

            newMessage = new PrivateMessage() {
                Content = Message,
                Title = this.MessageTitle,
                Sender = await _userManager.FindByIdAsync(SenderId),
                Reciever = await _userManager.FindByIdAsync(RecieverId),
                Created = DateTime.Now
            };
        } else {
            var group = await _userService.GetGroupAllInclusive(SenderId, RecieverGroupId);
            newMessage = new GroupMessage() {
                Content = Message,
                Title = this.MessageTitle,
                Sender = await _userManager.FindByIdAsync(SenderId),
                Recievers = group.Members,
                Created = DateTime.Now
            };
        }
        await _userService.SendMessageAsync(newMessage);
        return RedirectToPage("/Index");
    }
}
