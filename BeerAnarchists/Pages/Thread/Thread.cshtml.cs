using Forum.Data;
using Forum.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeerAnarchists.Pages.Thread;

public class ThreadModel : PageModel
{

    public int Id { get; set; }
    public int PostCount { get; set; }
    public IEnumerable<ForumPost> Posts { get; set; }
    public string? Title { get; set; }
    public DateTime LastPost { get; set; }
    public string LastPoster { get; set; }
    public string? Description { get; set; }

    //public SubForumModel SubForum { get; set; }
    //internal IEnumerable<ThreadModel> ForumThreads { get; set; } = Enumerable.Empty<ThreadModel>();

    private ForumDbContext _dbContext;
    private readonly UserManager<ForumUser> _userManager;
    private readonly ISubforum _subforumService;
    private readonly IForumPost _postService;
    private readonly IForumThread _threadService;

    public ThreadModel(ForumDbContext context, ISubforum subforumService, IForumPost postService, IForumThread threadService, UserManager<ForumUser> userManager)
    {
        _dbContext = context;
        _subforumService = subforumService;
        _postService = postService;
        _threadService = threadService;
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGet(int id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var threadData = _threadService.GetThreadById(id);

        if (threadData != null)
        {
            Id = threadData.Id;
            PostCount = threadData.Posts.Count();
            Posts = threadData.Posts.ToList();
            Title = threadData.Name;
            Description = threadData.Description;

            var post = Posts
                .OrderByDescending(p => p.Created)
                .FirstOrDefault();
            LastPoster = post?.Author.Alias ?? post?.Author.UserName ?? "Unknown";
            LastPost = post?.Created ?? default;
        }

        //var forum =_subforumService.GetById(id);

        //if(forum == null) {
        //    return NotFound();
        //}

        //SubForum = new SubForumModel() {Id = forum?.Id ?? -1, Author = forum?.Author, Description=forum?.Description, ImageUrl = forum?.ImageUrl, Title = forum?.Title };

        //ForumThreads = _dbContext.Subfora
        //    .Include(x => x.ForumThreads).ThenInclude(t => t.Posts)
        //    .Where(x => x.Id == id)
        //    .SelectMany(x => x.ForumThreads)
        //    .AsEnumerable()
        //    .Select(x => new ThreadModel() {
        //     Description = x.Description,
        //     Id = x.Id,
        //     Title = x.Name,
        //     Posts = x.Posts,
        //     PostCount = x.Posts.Count(),
        // }).ToList();

        //foreach (var fthread in ForumThreads) {
        //    var post = _dbContext.ForumThreads
        //        .Where(thread => thread.Id == fthread.Id)
        //        .Include(thread => thread.Posts).ThenInclude(p => p.Author)
        //        .SelectMany(thread => thread.Posts)
        //        .OrderByDescending(p => p.Created)
        //        .FirstOrDefault();
        //    fthread.LastPoster = post?.Author.Alias ?? post?.Author.UserName ?? "Unknown";
        //    fthread.LastPost = post?.Created ?? default;
        //}

        return Page();
    }

    // TODO : Implement reactions
    public async Task<ActionResult> AddReaction(string userId, int postId, ReactionType reaction) {
        //Check if there is already an reaction of this type from this user, we dont want multiple likes
        var checkPost = _postService.GetForumPostById(postId);
        var reactions = checkPost?.Reactions.ToList();
        if(reactions?.Select(x => x.Type == reaction && x.User.Id == userId) is null) {
            var newReaction = new Reaction() {
                User = await _userManager.FindByIdAsync(userId),
                Type = reaction,
                Post = checkPost,
            };
            await _postService.AddReaction(postId, newReaction);
        }
        return Page();
    }

    public async Task<ActionResult> ReportPost(string userId, int postId) {

        return Page();
    }
}




/*
public class ThreadModel {
public int Id { get; set; }
public int PostCount { get; set; }
public IEnumerable<ForumPost> Posts { get; set; }
public string? Title { get; set; }
public DateTime LastPost { get; set; }
public string LastPoster { get; set; }
public string? Description { get; set; }
}
*/
