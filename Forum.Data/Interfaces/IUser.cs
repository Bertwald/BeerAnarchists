using Forum.Data.Models;

namespace Forum.Data.Interfaces;
public interface IUser
{
    public Task<bool> IsValidUser(string userId);
    public Task SendMessageAsync(PrivateMessage message);
    public Task<ForumUser?> GetUserAllInclusiceAsync(string userId);
    public Task<bool> AddIgnoredAsync(string userId, string ignoredId);
    public Task<bool> RemoveIgnoredAsync(string userId, string ignoredId);
    public Task<bool> AddFriendAsync(string userId, string friendId);
    public Task<bool> RemoveFriendAsync(string userId, string friendId);
    public Task<IEnumerable<PrivateMessage>> GetInboxAsync(string userId);
    public int GetNumberInInbox(string userId);
    public int GetNumberInGroupMessages(string userId);
    public Task<IEnumerable<GroupMessage>> GetGroupMessages(string userId);
    public Task<bool> AddGroupInvitation(string userId, int groupId);
    public Task<bool> AddGroupApplication(string userId, int groupId);
    public Task<Group?> SearchGroup(string searchstring);
    public Task CreateGroup(string ownerId, string groupName);
    public Task DeleteGroup(string ownerId, int groupId);
    public Task AcceptInvitation(string userId, int groupId);
    public Task AcceptApplicant(string ownerId, string userId, int groupId);
    public Task <IEnumerable<Group>> GetUserGroups(string userId);

}
