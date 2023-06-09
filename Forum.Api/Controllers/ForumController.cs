using Forum.Data;
using Forum.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum.Api.Controllers;

public class ForumController : ForumRouteMapping {
    #region Member variables
    private readonly ForumDbContext _context;
    #endregion
    #region Constructors
    public ForumController(ForumDbContext context) {
        _context = context;
    }
    #endregion
    #region GET
    public async override Task<ActionResult<ForumUser>> GetUserById(string id) {
        if (_context.Users is null) {
            return NotFound();
        }
        var user = await _context.ForumUsers.FindAsync(id);

        if (user is null) {
            return NotFound();
        }

        return user;
    }

    public override Task<IEnumerable<object>> GetPostsBrief() {
        var tcs = new TaskCompletionSource<IEnumerable<object>>();
        tcs.SetResult(_context.ForumPosts
            .Select(x => new {
                author = (x.Author.Alias ?? x.Author.UserName),
                content = x.Content,
                date = x.Created.ToLongDateString()
            })
            .ToList());
        return tcs.Task;
    }

    public async override Task<ActionResult<ForumUser>> GetUserByMail(string mail) {
        if (_context.Users is null) {
            return NotFound();
        }
        var user = await _context.ForumUsers.Where(x => x.Email == mail).FirstAsync();

        if (user is null) {
            return NotFound();
        }

        return user;
    }

    public async override Task<ActionResult<IEnumerable<ForumUser>>> GetUsers() {
        if (_context.ForumUsers is null) {
            return NotFound();
        }
        return await _context.ForumUsers.ToListAsync();
    }

    public async override Task<ActionResult<int>> GetNumberOfUsers() {
        if (_context.Users is null) {
            return 0;
        }
        return await _context.Users.CountAsync();
    }

    public async override Task<ActionResult<IEnumerable<SubForum>>> GetSubFora() {
        if (_context.Subfora is null) {
            return NotFound();
        }
        return await _context.Subfora.ToListAsync();
    }

    public async override Task<ActionResult<ForumPost>> GetPostById(int id) {
        if (_context.ForumPosts is null) {
            return NotFound();
        }
        var forumPost = await _context.ForumPosts.FindAsync(id);

        if (forumPost is null) {
            return NotFound();
        }

        return forumPost;
    }

    public async override Task<ActionResult<IEnumerable<ForumPost>>> GetForumPosts() {
        if (_context.ForumPosts is null) {
            return NotFound();
        }
        return await _context.ForumPosts.Include(x => x.Author).ToListAsync();
    }

    public async override Task<ActionResult<IEnumerable<ForumUser>>> GetTopPosters() {
        if (_context.ForumPosts is null || _context.ForumUsers is null) {
            return NotFound();
        }
        return await _context.ForumUsers
            .Select(x => x)
            .OrderByDescending(user => user.Posts)
            .Take(10)
            .ToListAsync();
    }

    public async override Task<ActionResult<IEnumerable<ForumUser>>> GetTopPostersSince(DateTime date) {
        if (_context.ForumPosts is null || _context.ForumUsers is null) {
            return NotFound();
        }
        return await _context.ForumPosts
            .Where(post => post.Created >= date)
            .GroupBy(post => post.Author)
            .OrderByDescending(group => group.Count())
            .Select(group => group.Key)
            .Take(10)
            .ToListAsync();
    }

    public async override Task<ActionResult<int>> GetNumberOfPosts() {
        if (_context.ForumPosts is null) {
            return NotFound();
        }
        return await _context.ForumPosts.CountAsync();
    }

    public async override Task<ActionResult<int>> GetNumberOfPostsBySubforum(int id) {
        if (_context.Subfora is null) {
            return NotFound();
        }
        return await _context.Subfora
            .Where(subfora => subfora.Id == id)
            .SelectMany(subfora => subfora.ForumThreads
                .Select(thread => thread.Posts))
            .CountAsync();
    }


    #endregion
    #region POST
    public async override Task<ActionResult<ForumThread>> PostThread([FromBody] ForumThread thread) {
        if (_context.ForumThreads == null) {
            return Problem("Entity set 'ForumDbContext.ForumThreads'  is null.");
        }
        _context.ForumThreads.Add(thread);
        await _context.SaveChangesAsync();

        return CreatedAtAction("Post", thread.Id);
    }
    #endregion
    #region DELETE
    public async override Task<ActionResult> DeleteThread(int threadId) {
        if (_context.ForumThreads == null) {
            return NotFound();
        }
        var forumThread = await _context.ForumThreads.FindAsync(threadId);
        if (forumThread == null) {
            return NotFound();
        }
        _context.ForumThreads.Remove(forumThread);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    #endregion
}
