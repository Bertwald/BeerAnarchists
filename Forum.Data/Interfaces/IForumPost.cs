using Forum.Data.Models;

namespace Forum.Data.Interfaces;
/// <summary>
/// Gives the interface for actions performed on post objects and methods to retrieve and change post information
/// </summary>
public interface IForumPost
{
    public ForumPost? GetForumPostById(int id);
    public IEnumerable<ForumPost> GetAll();
    public IEnumerable<ForumPost> GetPostsInSubForum(int subforumIndex);
    public IEnumerable<ForumPost> GetPostsInForumThread(int threadId);
    public IEnumerable<ForumPost> Search(string query);
    public Task Add(ForumPost post);
    public Task Delete(int id);
    public Task AddReply(int parentId, ForumPost reply);
    public Task AddReport(PostReport report);
    public Task AddImage(int postId, string url);
    public Task AddReaction(int postId, Reaction reaction);
}
