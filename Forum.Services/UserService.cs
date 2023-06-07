using Forum.Data;
using Forum.Data.Interfaces;
using Forum.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

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
        if (user == null || ignored == null || user == ignored) {
            return false;
        }
        if (user.Ignored.Contains(ignored)) {
            return false;
        }
        user.Ignored = user.Ignored.Append(ignored).ToList();
        _db.Entry(user).State = EntityState.Modified;

        try {
            await _db.SaveChangesAsync();
        }
        catch (Exception) {
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
        catch (Exception) {
            throw;
        }
        return true;
    }

    public async Task<IEnumerable<PrivateMessage>> GetInboxAsync(string userId) {
        if (userId == null) {
            return Enumerable.Empty<PrivateMessage>();
        }
        var userIgnored = _db.Users.SelectMany(user => user.Ignored);
        var messages = _db.PrivateMessages.Where(message => message.Reciever.Id == userId && !userIgnored.Contains(message.Sender));
        return messages.Include(x => x.Sender).Include(x => x.Reciever).AsEnumerable();
    }

    public async Task<bool> IsValidUser(string userId) {
        return (await _db.Users.FindAsync(userId)) != null;
    }

    public async Task SendMessageAsync(PrivateMessage message) {
        if (message == null) {
            return;
        }
        _db.Messages.Add(message);
        try {
            await _db.SaveChangesAsync();
        }
        catch (Exception) {
            throw;
        }
    }

    public async Task<ForumUser?> GetUserAllInclusiceAsync(string userId) {
        if (userId == null) {
            return null;
        }
        return await _db.Users
            .Where(user => user.Id == userId)
            .Include(user => user.Friends)
            .Include(user => user.Ignored)
            .Include(user => user.OwnedGroups)
            .Include(user => user.MemberGroups)
            .Include(user => user.Invitations)
            .Include(user => user.Applications)
            .FirstAsync();
    }

    public int GetNumberInInbox(string userId) {
        if (userId == null) {
            return default;
        }
        var userIgnored = _db.Users.SelectMany(user => user.Ignored);
        var messages = _db.PrivateMessages.Where(x => x.Reciever.Id == userId && x.Read==false && !userIgnored.Contains(x.Sender));

        return messages.Count();
    }

    public int GetNumberInGroupMessages(string userId) {
        if (userId == null) {
            return default;
        }
        return _db.GroupMessages.Where(x => (x.Recievers.Where(y => y.Id == userId)).Any()).Count();
    }

    public async Task<bool> AddGroupInvitation(string userId, int groupId) {
        if (userId == null) {
            return false;
        }
        var user = await _db.ForumUsers
                    .Where(x => x.Id == userId)
                    .Include(x => x.Invitations)
                    .FirstOrDefaultAsync();
        var group = await _db.Groups
            .Where(x => x.Id == groupId)
            .Include(x => x.Invitees)
            .FirstOrDefaultAsync();

        if (user != null && group != null) {
            user.Invitations = user.Invitations.Append(group).ToList();
            group.Invitees = group.Invitees.Append(user).ToList();

            _db.Entry(user).State = EntityState.Modified;
            _db.Entry(group).State = EntityState.Modified;

            try {
                await _db.SaveChangesAsync();
            }
            catch (Exception) {
                throw;
            }

            return true;
        }
        return false;
    }

    public async Task<bool> AddGroupApplication(string userId, int groupId) {
        if (userId == null) {
            return false;
        }
        var user = await _db.ForumUsers
            .Where(x => x.Id == userId)
            .Include(x => x.Applications)
            .FirstOrDefaultAsync();
        var group = await _db.Groups
            .Where(x => x.Id == groupId)
            .Include(x => x.Applicants)
            .FirstOrDefaultAsync();

        if (user != null && group != null) {
            user.Applications = user.Applications.Append(group).ToList();
            group.Applicants = group.Applicants.Append(user).ToList();

            _db.Entry(user).State = EntityState.Modified;
            _db.Entry(group).State = EntityState.Modified;

            try {
                await _db.SaveChangesAsync();
            }
            catch (Exception) {
                throw;
            }

            return true;
        }
        return false;
    }

    public async Task<Group?> SearchGroup(string searchstring) {
        return await _db.Groups.Where(group => group.Name == searchstring).FirstOrDefaultAsync();
    }

    public async Task AcceptInvitation(string userId, int groupId) {
        if (userId == null) {
            return;
        }
        var user = await _db.ForumUsers
            .Where(x => x.Id == userId)
            .Include(x => x.Invitations)
            .Include(x => x.MemberGroups)
            .FirstOrDefaultAsync();
        var group = await _db.Groups
            .Where(x => x.Id == groupId)
            .Include(x => x.Invitees)
            .Include(x => x.Members)
            .FirstOrDefaultAsync();

        if ((user?.Invitations.Contains(group) ?? false) && (group?.Invitees.Contains(user) ?? false)) {
            user.Invitations = user.Invitations.Where(x => x != group).ToList();
            group.Invitees = group.Invitees.Where(x => x != user).ToList();

            group.Members = group.Members.Append(user).ToList();
            user.MemberGroups = user.MemberGroups.Append(group).ToList();

            _db.Entry(user).State = EntityState.Modified;
            _db.Entry(group).State = EntityState.Modified;
            try {
                await _db.SaveChangesAsync();
            }
            catch (Exception) {
                throw;
            }
        }
    }

    public async Task AcceptApplicant(string ownerId, string applicantId, int groupId) {
        if (applicantId == null || ownerId == null) {
            return;
        }
        var owner = await _db.ForumUsers
            .Where(x => x.Id == ownerId)
            .Include(x => x.OwnedGroups)
            .FirstOrDefaultAsync();
        var applicant = await _db.ForumUsers
            .Where(x => x.Id == applicantId)
            .Include(x => x.Applications)
            .Include(x => x.MemberGroups)
            .FirstOrDefaultAsync();
        var group = await _db.Groups
            .Where(x => x.Id == groupId)
            .Include(x => x.Creator)
            .Include(x => x.Applicants)
            .Include(x => x.Members)
            .FirstOrDefaultAsync();

        if ((applicant?.Applications.Contains(group) ?? false) && (group?.Applicants.Contains(applicant) ?? false) && group.Creator == owner) {
            applicant.Invitations = applicant.Invitations.Where(x => x != group).ToList();
            group.Invitees = group.Invitees.Where(x => x != applicant).ToList();

            group.Members = group.Members.Append(applicant).ToList();
            applicant.MemberGroups = applicant.MemberGroups.Append(group).ToList();

            _db.Entry(applicant).State = EntityState.Modified;
            _db.Entry(group).State = EntityState.Modified;
            try {
                await _db.SaveChangesAsync();
            }
            catch (Exception) {
                throw;
            }
        }
    }

    public async Task<IEnumerable<GroupMessage>> GetGroupMessages(string userId) {
        var user = _db.ForumUsers.Where(x => x.Id == userId).FirstOrDefault();
        return await _db.GroupMessages.Where(x => x.Recievers.Contains(user)).ToListAsync();
    }

    public async Task<IEnumerable<Group>> GetUserGroups(string userId) {
        List<Group> groups = new();

        var ownedGroups = await _db.Users.Where(x => x.Id == userId).SelectMany(x => x.OwnedGroups).ToListAsync();
        var memberGroups = await _db.Users.Where(x => x.Id == userId).SelectMany(x => x.OwnedGroups).ToListAsync();

        groups.AddRange(ownedGroups);
        groups.AddRange(memberGroups);

        return groups;
    }

    public async Task CreateGroup(string ownerId, string groupName) {
        if(ownerId == null || groupName == null) {
            return;
        }
        var user = _db.ForumUsers.Where(x => x.Id == ownerId).FirstOrDefault();
        if (user != null) {
            var newGroup = new Group { Creator = user, Name = groupName };
            _db.Groups.Add(newGroup);

            try {
                await _db.SaveChangesAsync();
            } catch (Exception) {
                throw;
            }
        }
    }

    public async Task<bool> RemoveIgnoredAsync(string userId, string ignoredId) {
        var user = _db.ForumUsers.Where(x => x.Id == userId).Include(x => x.Ignored).FirstOrDefault();
        if (user != null) {
            user.Ignored = user.Ignored.Where(x => x.Id != ignoredId).ToList();
            _db.Entry(user).State = EntityState.Modified;
            try {
                await _db.SaveChangesAsync();
            } catch (Exception) {
                throw;
            }
            return true;
        }
        return false;
    }

    public async Task<bool> RemoveFriendAsync(string userId, string friendId) {
        var user = _db.ForumUsers.Where(x => x.Id == userId).Include(x => x.Friends).FirstOrDefault();
        if (user != null) {
            user.Friends = user.Friends.Where(x => x.Id != friendId).ToList();
            _db.Entry(user).State = EntityState.Modified;
            try {
                await _db.SaveChangesAsync();
            }
            catch (Exception) {
                throw;
            }
            return true;
        }
        return false;
    }

    public async Task DeleteGroup(string ownerId, int groupId) {
        if(groupId == 0 || ownerId == null) {
            return;
        }
        var group = _db.Groups.Where(x => x.Id == groupId && x.Creator.Id == ownerId).FirstOrDefault();
        if(group != null) {
            _db.Groups.Remove(group);

            try {
                await _db.SaveChangesAsync();
            } catch(Exception) {
                throw;
            }
        }
    }
}
