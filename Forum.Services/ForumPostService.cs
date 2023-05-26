using Forum.Data;
using Forum.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Services;
public class ForumPostService : IForumPost {

    private ForumDbContext _dbcontext;
    public ForumPostService(ForumDbContext dbcontext) {
        _dbcontext = dbcontext;
    }

    public Task Add(ForumPost post) {
        throw new NotImplementedException();
    }

    public Task AddImage(string url) {
        throw new NotImplementedException();
    }

    public Task AddReply(ForumPost reply) {
        throw new NotImplementedException();
    }

    public Task Delete(int id) {
        throw new NotImplementedException();
    }

    public IEnumerable<ForumPost> GetAll() {
        throw new NotImplementedException();
    }

    public ForumPost? GetForumPostById(int id) {
        return _dbcontext.ForumPosts.Where(x => x.Id == id).FirstOrDefault();
    }

    public IEnumerable<ForumPost> GetPostsInForumThread(int threadId) {
        return _dbcontext.ForumThreads.Where(thread => thread.Id == threadId)
            .Include(thread => thread.Posts)
            .SelectMany(thread => thread.Posts)
            .AsEnumerable();
    }

    public IEnumerable<ForumPost> GetPostsInSubForum(int subforumIndex) {
        return _dbcontext.Subfora
            .Where(forum => forum.Id == subforumIndex)
            .Include(forum => forum.ForumThreads).ThenInclude(thread => thread.Posts)
            .SelectMany(forum => forum.ForumThreads)
            .SelectMany(thread => thread.Posts)
            .AsEnumerable();
    }

    public IEnumerable<ForumPost> Search(string query) {
        throw new NotImplementedException();
    }
}
