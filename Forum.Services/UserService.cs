using Forum.Data;
using Forum.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Forum.Services;
/// <summary>
/// Used to retrieve different data based on user and change data with authorization
/// </summary>
[Authorize]
public sealed class UserService : IUser {
    private readonly ForumDbContext _db;
    public UserService(ForumDbContext db) {
        _db = db;
    }

    public async Task<bool> AddIgnoredAsync(string userId, string ignoredId) {
        var user = _db.Users.Where(user => user.Id == userId).Include(user => user.Ignored).First();
        var ignored = await _db.Users.FindAsync(ignoredId);
        if(user == null || ignored == null || user == ignored){
            return false;
        }
        if (user.Ignored.Contains(ignored)) {
            return false;
        }
        user.Ignored = user.Ignored.Append(ignored).ToList();
        _db.Entry(user).State = EntityState.Modified;

        try {
            await _db.SaveChangesAsync();
        } catch (Exception ex) {
            throw;
        }
        return true;
    }

    public async Task<bool> AddFriendAsync(string userId, string friendId) {
        var user = _db.Users.Where(user => user.Id == userId).Include(user => user.Friends).First();
        var friend = await _db.Users.FindAsync(friendId);
        if (user == null || friend == null || user == friend) {
            return false;
        }
        if (user.Friends.Contains(friend)) {
            return false;
        }
        user.Friends = user.Friends.Append(friend).ToList();
        _db.Entry(user).State = EntityState.Modified;

        try {
            await _db.SaveChangesAsync();
        }
        catch (Exception ex) {
            throw;
        }
        return true;
    }

    public async Task<IEnumerable<PrivateMessage>> GetInboxAsync(string userId) {
        if(userId == null) {
            return Enumerable.Empty<PrivateMessage>();
        }
        var messages = _db.PrivateMessages.Where(message => message.Reciever.Id == userId);
        return messages.Include(x => x.Sender).Include(x => x.Reciever).AsEnumerable();
    }
}
