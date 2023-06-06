using Forum.Data.Models;

namespace Forum.Data.Interfaces;
public interface IForumThread
{
    public ForumThread GetThreadById(int id);
    public int GetThreadIdFromPostId(int postId);
    public Task AddThread(ForumThread newThread);
    public Task UpdateThread(ForumThread thread);
}
