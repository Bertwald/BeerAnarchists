using Forum.Data.Models;

namespace Forum.Data;
public interface IUser {
    public Task<bool> IsValidUser(string userId);
    public Task SendMessageAsync(PrivateMessage message);
    public Task<ForumUser?> GetUserAllInclusiceAsync(string userId);
    public Task<bool> AddIgnoredAsync(string userId, string ignoredId);
    public Task<bool> AddFriendAsync(string userId, string friendId);
    public Task<IEnumerable<PrivateMessage>> GetInboxAsync(string userId);
}
