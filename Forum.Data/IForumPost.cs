using Forum.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Data;
/// <summary>
/// Gives the interface for actions performed on post objects and methods to retrieve and change post information
/// </summary>
public interface IForumPost {
    public ForumPost? GetForumPostById(int id);
    public IEnumerable<ForumPost> GetAll();
    public IEnumerable<ForumPost> GetPostsInSubForum(int subforumIndex);
    public IEnumerable<ForumPost> GetPostsInForumThread(int threadId);
    public IEnumerable<ForumPost> Search(string query);
    public Task Add(ForumPost post);
    public Task Delete(int id);
    public Task AddReply(ForumPost reply);
    // string filepath?
    public Task AddImage(string url);
}
