using Forum.Data.Interfaces;
using Forum.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeerAnarchists.Pages.Groups;
[Authorize]
public class GroupsModel : PageModel {
    private readonly IUser _userService;
    private readonly UserManager<ForumUser> _userManager;

    public List<GroupMessage> GroupMessages { get; set; } = new();
    public List<ForumUser> Friends { get; set; } = new();
    public List<ForumUser> Ignored { get; set; } = new();
    public List<Forum.Data.Models.Group> OwnedGroups { get; set; } = new();
    public List<Forum.Data.Models.Group> FriendGroups { get; set; } = new();
    public List<Forum.Data.Models.Group> MyApplications { get; set; } = new();
    public List<Forum.Data.Models.Group> Invitations { get; set; } = new();
    [BindProperty]
    public string OwnerId { get; set; }
    [BindProperty]
    public string NewGroupName { get; set; }

    public GroupsModel(IUser userservice, UserManager<ForumUser> userManager) {
        _userService = userservice;
        _userManager = userManager;
    }

    public async Task<ActionResult> OnGet(string userId) {
        var user = await _userService.GetUserAllInclusiceAsync(userId);
        if (user == null) {
            return BadRequest();
        }
        OwnerId= user.Id;
        Friends = user.Friends.ToList();
        Ignored = user.Ignored.ToList();
        OwnedGroups = user.OwnedGroups.ToList();
        foreach (var friend in Friends) {
            FriendGroups.AddRange(await _userService.GetUserGroups(friend.Id));
        }
        MyApplications = user.Applications.ToList();
        Invitations = user.Invitations.ToList();
        GroupMessages = (await _userService.GetGroupMessages(userId)).ToList();
        return Page();
    }

    public async Task<ActionResult> OnPostAsync() {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        await _userService.CreateGroup(OwnerId, NewGroupName);
        return RedirectToPage("./Groups", new {userId = OwnerId });
    }

    public async Task<ActionResult> OnGetRemoveFriendAsync(string userId, string friendId) {
        if(friendId == null || userId == null) {
            return RedirectToPage("./Groups", new { userId = userId });
        }

        _ = await _userService.RemoveFriendAsync(userId, friendId);
        return RedirectToPage("./Groups", new { userId = userId });
    }
    public async Task<ActionResult> OnGetRemoveIgnoredAsync(string userId, string ignoredId) {
        if(userId == null || ignoredId == null) {
            return RedirectToPage("./Groups", new { userId = userId });
        }

        _ = await _userService.RemoveIgnoredAsync(userId, ignoredId);
        return RedirectToPage("./Groups", new { userId = userId });
    }

    public async Task<ActionResult> OnGetDeleteGroupAsync(string userId, int groupId) {
        if (userId == null || groupId == 0) {
            return RedirectToPage("./Groups", new { userId = userId });
        }
        await _userService.DeleteGroup(userId, groupId);
        return RedirectToPage("./Groups", new { userId = userId });
    }

}
