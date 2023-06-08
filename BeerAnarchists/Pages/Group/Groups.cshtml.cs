using Forum.Data.Interfaces;
using Forum.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

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
    [BindProperty]
    public string? SelectedFriend { get; set; }

    public SelectList FriendsSL { get; set; }

    public async Task PopulateFriendsDropDownList(IUser userService, string userId) {
        var friends = (await userService.GetUserAllInclusiveAsync(userId))?.Friends;

        FriendsSL = new SelectList(friends,
            nameof(ForumUser.Id),
            nameof(ForumUser.UserName));
    }

    public GroupsModel(IUser userservice, UserManager<ForumUser> userManager) {
        _userService = userservice;
        _userManager = userManager;
    }

    public async Task<ActionResult> OnGet(string userId) {
        var user = await _userService.GetUserAllInclusiveAsync(userId);
        if (user == null) {
            return BadRequest();
        }
        await PopulateFriendsDropDownList(_userService, userId);

        OwnerId = user.Id;
        Friends = user.Friends.ToList();
        Ignored = user.Ignored.ToList();
        OwnedGroups = new List<Forum.Data.Models.Group>();
        foreach (var group in user .OwnedGroups) {
            OwnedGroups.Add(await _userService.GetGroupAllInclusive(OwnerId,group.Id));
        }

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
        return RedirectToPage("./Groups", new { userId = OwnerId });
    }

    public async Task<ActionResult> OnGetRemoveFriendAsync(string userId, string friendId) {
        if (friendId == null || userId == null) {
            return RedirectToPage("./Groups", new { userId = userId });
        }

        _ = await _userService.RemoveFriendAsync(userId, friendId);
        return RedirectToPage("./Groups", new { userId = userId });
    }
    public async Task<ActionResult> OnGetRemoveIgnoredAsync(string userId, string ignoredId) {
        if (userId == null || ignoredId == null) {
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

    public async Task<ActionResult> OnGetHandleApplicationAsync(string userId, int groupId, string applicantId, bool add) {
        if (userId == null || groupId == 0 || applicantId == null) {
            return RedirectToPage("./Groups", new { userId = userId });
        }

        if (add) {
            await _userService.AcceptApplicant(userId, applicantId, groupId);
        } else {
            await _userService.RejectApplicant(applicantId, groupId);
        }

        return RedirectToPage("./Groups", new { userId = userId });
    }

    public async Task<ActionResult> OnGetSeekMembershipAsync(string userId, int groupId, bool add) {
        if(userId == null || groupId == 0) {
            return RedirectToPage("./Groups", new { userId = userId });
        }
        if (add) {
            await _userService.AddGroupApplication(userId, groupId);
        } else {
            await _userService.RejectApplicant(userId, groupId);
        }

        return RedirectToPage("./Groups", new { userId = userId });
    }

    public async Task<ActionResult> OnPostSendInvitationAsync(int groupId) {
        //var selectedFriendId = Request.Form["SelectedFriend"];

        if(SelectedFriend == null || OwnerId == null) {
            return RedirectToPage("./Groups", new { userId = OwnerId });
        }

        await _userService.AddGroupInvitation(SelectedFriend, groupId);
        return RedirectToPage("./Groups", new { userId = OwnerId });
    }

}
