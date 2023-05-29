﻿using Forum.Data;
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
    private IForumThread _threadService;
    public ForumPostService(ForumDbContext dbcontext, IForumThread threadService) {
        _dbcontext = dbcontext;
        _threadService = threadService;
    }

    public async Task Add(ForumPost post) {
        if (_dbcontext.ForumPosts == null) {
            return;
        }
        _dbcontext.ForumPosts.Add(post);
        await _dbcontext.SaveChangesAsync();
    }

    public async Task AddImage(int postId, string url) {
        if (_dbcontext.ForumPosts == null) {
            return;
        }
        var post = _dbcontext.ForumPosts.Find(postId);
        if (post == null) {
            return;
        }
        post.ImageUrl = url;
        _dbcontext.Entry(post).State = EntityState.Modified;

        try {
            await _dbcontext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            throw;
        }

    }

    public async Task AddReply(int parentId, ForumPost reply) {
        if (_dbcontext.ForumPosts == null) {
            return;
        }
        var post = _dbcontext.ForumPosts.Find(parentId);
        if (post == null) {
            return;
        }

        reply.Ancestor = post;
        post.Replies = post.Replies.Append(reply);
        _dbcontext.Entry(post).State = EntityState.Modified;
        _dbcontext.Entry(reply).State = EntityState.Modified;

        try {
            await _dbcontext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            throw;
        }
    }

    public async Task Delete(int id) {
        if (_dbcontext.ForumPosts == null) {
            return;
        }
        var forumPost = await _dbcontext.ForumPosts.FindAsync(id);
        if (forumPost == null) {
            return;
        }

        _dbcontext.ForumPosts.Remove(forumPost);
        await _dbcontext.SaveChangesAsync();
    }

    public IEnumerable<ForumPost> GetAll() {
        return _dbcontext.ForumPosts
            .Include(x => x.Author)
            .Include(x => x.Ancestor)
            .Include(x => x.Replies)
            .ToList();
    }

    public ForumPost? GetForumPostById(int id) {
        return _dbcontext.ForumPosts.Where(x => x.Id == id)
            .Include(x => x.Author)
            .Include(x => x.Ancestor)
            .Include(x => x.Replies)
            .FirstOrDefault();
    }

    public IEnumerable<ForumPost> GetPostsInForumThread(int threadId) {
        return _dbcontext.ForumThreads.Where(thread => thread.Id == threadId)
            .Include(thread => thread.Posts)
            .SelectMany(thread => thread.Posts)
            .Include(x => x.Author)
            .Include(x => x.Ancestor)
            .Include(x => x.Replies)
            .ToList();
    }

    public IEnumerable<ForumPost> GetPostsInSubForum(int subforumIndex) {
        return _dbcontext.Subfora
            .Where(forum => forum.Id == subforumIndex)
            .Include(forum => forum.ForumThreads).ThenInclude(thread => thread.Posts)
            .SelectMany(forum => forum.ForumThreads)
            .SelectMany(thread => thread.Posts)
            .Include(x => x.Author)
            .Include(x => x.Ancestor)
            .Include(x => x.Replies)
            .ToList();
    }

    public IEnumerable<ForumPost> Search(string query) {
        var postHits = _dbcontext.ForumPosts
            .Include(x => x.Author)
            .Include(x => x.Ancestor)
            .Include(x => x.Replies)
            .Where(x => x.Author.UserName == query ||
                        x.Content.Contains(query) ||
                        x.Author.Alias == query)
            .AsEnumerable();
        var threadHits = _dbcontext.ForumThreads
            .Include(thread => thread.Posts)
            .Where(x => x.Name == query || (string.Empty + x.Description).Contains(query))
            .SelectMany(x => x.Posts)
            .Include(x => x.Author)
            .Include(x => x.Ancestor)
            .Include(x => x.Replies)
            .AsEnumerable();

        return postHits.Concat(threadHits);
    }
}
