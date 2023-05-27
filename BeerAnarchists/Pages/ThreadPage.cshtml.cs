using Forum.Data;
using Forum.Data.Models;
using Forum.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static BeerAnarchists.Pages.IndexModel;

namespace BeerAnarchists.Pages;

public class ThreadPageModel : PageModel {
    public SubForumModel SubForum { get; set; }
    internal IEnumerable<ThreadModel> ForumThreads { get; set; } = Enumerable.Empty<ThreadModel>();

    private ForumDbContext _dbContext;
    private readonly ISubforum _subforumService;

    public ThreadPageModel(ForumDbContext context, ISubforum subforumService) {
        _dbContext = context;
        _subforumService = subforumService;
    }

    public async Task<IActionResult> OnGet(int id)
    {
        if(id == null) {
            return NotFound();
        }

        var forum =_subforumService.GetById(id);

        if(forum == null) {
            return NotFound();
        }

        SubForum = new SubForumModel() {Id = forum?.Id ?? -1, Author = forum?.Author, Description=forum?.Description, ImageUrl = forum?.ImageUrl, Title = forum?.Title };

        ForumThreads = _dbContext.Subfora
            .Include(x => x.ForumThreads).ThenInclude(t => t.Posts)
            .Where(x => x.Id == id)
            .SelectMany(x => x.ForumThreads)
            .AsEnumerable()
            .Select(x => new ThreadModel() {
             Description = x.Description,
             Id = x.Id,
             Title = x.Name,
             Posts = x.Posts,
             PostCount = x.Posts.Count(),
         }).ToList();

        foreach (var fthread in ForumThreads) {
            var post = _dbContext.ForumThreads
                .Where(thread => thread.Id == fthread.Id)
                .Include(thread => thread.Posts).ThenInclude(p => p.Author)
                .SelectMany(thread => thread.Posts)
                .OrderByDescending(p => p.Created)
                .FirstOrDefault();
            fthread.LastPoster = post?.Author.Alias ?? post?.Author.UserName ?? "Unknown";
            fthread.LastPost = post?.Created ?? default;
        }

        return Page();
    }
}

public class ThreadModel {
public int Id { get; set; }
public int PostCount { get; set; }
public IEnumerable<ForumPost> Posts { get; set; }
public string? Title { get; set; }
public DateTime LastPost { get; set; }
public string LastPoster { get; set; }
public string? Description { get; set; }
}
