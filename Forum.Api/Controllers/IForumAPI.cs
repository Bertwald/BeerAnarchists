using Forum.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum.Api.Controllers;

public interface IForumAPI {
    #region GET
    public Task<ActionResult<ForumUser>> GetUserById(string id);
    public Task<ActionResult<ForumUser>> GetUserByMail(string mail);
    public Task<ActionResult<IEnumerable<ForumUser>>> GetUsers();
    public Task<ActionResult<int>> GetNumberOfUsers();
    public Task<ActionResult<IEnumerable<ForumUser>>> GetTopPosters();
    public Task<ActionResult<IEnumerable<ForumUser>>> GetTopPostersSince(DateTime date);
    public Task<ActionResult<IEnumerable<SubForum>>> GetSubFora();
    public Task<ActionResult<ForumPost>> GetPostById(int id);
    public Task<ActionResult<int>> GetNumberOfPosts();
    public Task<ActionResult<int>> GetNumberOfPostsBySubforum(int id);
    public Task<ActionResult<IEnumerable<ForumPost>>> GetForumPosts();
    public Task<IEnumerable<object>> GetPostsBrief();
    #endregion
    #region POST
    public Task<ActionResult<ForumThread>> PostThread(ForumThread thread);
    #endregion
    #region DELETE
    public Task<ActionResult> DeleteThread(int threadId);
    #endregion
}
