using Forum.Data;
using Forum.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum.Api.Controllers;
[Route("forum/[action]")]
[ApiController]
public class ForumController : ControllerBase, IForumAPI {
    #region Member variables
    private readonly ForumDbContext _context;
    #endregion
    #region Constructors
    public ForumController(ForumDbContext context) {
        _context = context;
    }
    #endregion
    #region GET
    [HttpGet("{id}")]
    [ActionName("User")]
    public async Task<ActionResult<ForumUser>> GetUserById(string id) {
        if (_context.Users is null) {
            return NotFound();
        }
        var user = await _context.ForumUsers.FindAsync(id);

        if (user is null) {
            return NotFound();
        }

        return user;
    }

    [HttpGet]
    [ActionName("Users")]
    public async Task<ActionResult<IEnumerable<ForumUser>>> GetUsers() {
        if (_context.ForumUsers is null) {
            return NotFound();
        }
        return await _context.ForumUsers.ToListAsync();
    }

    [HttpGet]
    [ActionName("RegisteredUsers")]
    public async Task<ActionResult<int>> GetNumberOfUsers() {
        if (_context.Users is null) {
            return 0;
        }
        return await _context.Users.CountAsync();
    }

    [HttpGet]
    [ActionName("SubFora")]
    public async Task<ActionResult<IEnumerable<SubForum>>> GetSubFora() {
        if (_context.Subfora is null) {
            return NotFound("No posts in forum Database");
        }
        return await _context.Subfora.Include(x => x.ForumThreads).ToListAsync();
    }

    [HttpGet("{id}")]
    [ActionName("Post")]
    public async Task<ActionResult<ForumPost>> GetPostById(int id) {
        if (_context.ForumPosts is null) {
            return NotFound("No posts in forum Database");
        }
        var forumPost = await _context.ForumPosts.FindAsync(id);

        if (forumPost is null) {
            return NotFound();
        }

        return forumPost;
    }

    [HttpGet]
    [ActionName("Posts")]
    public async Task<ActionResult<IEnumerable<ForumPost>>> GetForumPosts() {
        if (_context.ForumPosts is null) {
            return NotFound("No posts in forum Database");
        }
        return await _context.ForumPosts.ToListAsync();
    }

    [HttpGet]
    [ActionName("TopPosters")]
    public async Task<ActionResult<IEnumerable<ForumUser>>> GetTopPosters() {
        if (_context.ForumPosts is null || _context.ForumUsers is null) {
            return NotFound();
        }
        return await _context.ForumUsers
            .OrderByDescending(user => user.Posts)
            .Take(10)
            .ToListAsync();
    }

    [HttpGet("{date}")]
    [ActionName("TopPosterSince")]
    public async Task<ActionResult<IEnumerable<ForumUser>>> GetTopPostersSince(DateTime date) {
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

    [HttpGet]
    [ActionName("TotalPosts")]
    public async Task<ActionResult<int>> GetNumberOfPosts() {
        if (_context.ForumPosts is null) {
            return NotFound();
        }
        return await _context.ForumPosts.CountAsync();
    }

    [HttpGet("{id}")]
    [ActionName("TotalPostsBySubForumId")]
    public async Task<ActionResult<int>> GetNumberOfPostsBySubforum(int id) {
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
}
