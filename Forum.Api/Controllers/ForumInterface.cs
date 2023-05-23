using Forum.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum.Api.Controllers;

public interface IForumAPI {
    #region GET
    public Task<ActionResult<ForumUser>> GetUserById(string id);
    public Task<ActionResult<IEnumerable<ForumUser>>> GetUsers();
    public Task<ActionResult<int>> GetNumberOfUsers();
    public Task<ActionResult<IEnumerable<ForumUser>>> GetTopPosters();
    public Task<ActionResult<IEnumerable<ForumUser>>> GetTopPostersSince(DateTime date);
    public Task<ActionResult<IEnumerable<SubForum>>> GetSubFora();
    public Task<ActionResult<ForumPost>> GetPostById(int id);
    public Task<ActionResult<int>> GetNumberOfPosts();
    public Task<ActionResult<int>> GetNumberOfPostsBySubforum(int id);
    public Task<ActionResult<IEnumerable<ForumPost>>> GetForumPosts();
    #endregion
}
