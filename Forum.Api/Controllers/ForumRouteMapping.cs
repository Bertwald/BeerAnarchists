using Forum.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Api.Controllers;

[Route("forum/[action]")]
[ApiController]
public abstract class ForumRouteMapping : ControllerBase, IForumAPI {
    [HttpGet]
    [ActionName("Posts")]
    public abstract Task<ActionResult<IEnumerable<ForumPost>>> GetForumPosts();
    [HttpGet]
    [ActionName("TotalPosts")]
    public abstract Task<ActionResult<int>> GetNumberOfPosts();
    [HttpGet("{id}")]
    [ActionName("TotalPostsBySubForumId")]
    public abstract Task<ActionResult<int>> GetNumberOfPostsBySubforum(int id);
    [HttpGet]
    [ActionName("RegisteredUsers")]
    public abstract Task<ActionResult<int>> GetNumberOfUsers();
    [HttpGet("{id}")]
    [ActionName("Post")]
    public abstract Task<ActionResult<ForumPost>> GetPostById(int id);
    [HttpGet]
    [ActionName("SubFora")]
    public abstract Task<ActionResult<IEnumerable<SubForum>>> GetSubFora();
    [HttpGet]
    [ActionName("TopPosters")]
    public abstract Task<ActionResult<IEnumerable<ForumUser>>> GetTopPosters();
    [HttpGet("{date}")]
    [ActionName("TopPosterSince")]
    public abstract Task<ActionResult<IEnumerable<ForumUser>>> GetTopPostersSince(DateTime date);  
    [HttpGet("{id}")]
    [ActionName("UserById")]
    public abstract Task<ActionResult<ForumUser>> GetUserById(string id);
    [HttpGet("{mail}")]
    [ActionName("UserByMail")]
    public abstract Task<ActionResult<ForumUser>> GetUserByMail(string mail);
    [HttpGet]
    [ActionName("Users")]
    public abstract Task<ActionResult<IEnumerable<ForumUser>>> GetUsers();
}
