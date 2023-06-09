using Forum.Data;
using Forum.Data.Interfaces;
using Forum.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum.Services;
public sealed class ForumThreadsService : IForumThread {
    private ForumDbContext _context;

    public ForumThreadsService(ForumDbContext context) {
        _context = context;
    }

    public async Task AddThread(ForumThread newThread) {

        if (_context.ForumThreads == null) {
            return;
        }
        _context.ForumThreads.Add(newThread);
        await _context.SaveChangesAsync();
    }

    public ForumThread GetThreadById(int id) {
        return _context.ForumThreads
            .Include(x => x.Posts)
                .ThenInclude(p => p.Author)
            .Include(x => x.Posts)
                .ThenInclude(p => p.Reactions)
            .Where(x => x.Id == id)
            .FirstOrDefault();
    }

    public int GetThreadIdFromPostId(int postId) {
        return _context.ForumThreads
            .Where(x => x.Posts
                         .Where(x => x.Id == postId).Any())
            .First()
            .Id;
    }

    public async Task UpdateThread(ForumThread thread) {
        if (_context.ForumThreads == null) {
            return;
        }
        _context.Entry(thread).State = EntityState.Modified;
        try {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            throw;
        }
    }
}
