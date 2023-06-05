using Forum.Data.Interfaces;
using Forum.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeerAnarchists.Pages.Post;
[Authorize]
public class PostReplyModel : PageModel {
    #region Services
    private readonly IForumPost _postService;
    private readonly IForumThread _threadService;
    private readonly UserManager<ForumUser> _userManager;
    #endregion
    #region DisplayFields
    public string? PostImageUrl { get; set; }
    public string? PostContent { get; set; }
    public ForumUser PostAuthor { get; set; }
    public string AgeString { get; set; }
    #endregion
    #region InputFields
    [BindProperty]
    public string ReplyAuthorId { get; set; }
    [BindProperty]
    public int PostId { get; set; }
    [BindProperty]
    public IFormFile? UploadedImage { get; set; }
    [BindProperty]
    public string ReplyContent { get; set; }
    #endregion

    #region Constructors
    public PostReplyModel(IForumPost service, UserManager<ForumUser> userManager, IForumThread threadService) {
        _postService = service;
        _userManager = userManager;
        _threadService = threadService;
    }
    #endregion
    public async Task<IActionResult> OnGet(int postId) {
        if (User == null) {
            return NotFound();
        }

        var ReplyAuthor = await _userManager.GetUserAsync(User);

        if (ReplyAuthor == null) {
            return NotFound();
        }
        ReplyAuthorId = ReplyAuthor.Id;

        var originalPost = _postService.GetForumPostById(postId);
        if (originalPost != null) {
            PostId = originalPost.Id;
            PostContent = originalPost.Content;
            PostAuthor = originalPost.Author;
            PostImageUrl = originalPost.ImageUrl;
            AgeString = (DateTime.Now - originalPost.Created).Days > 1 ? (DateTime.Now - originalPost.Created).Days.ToString() + " days "
                        : (DateTime.Now - originalPost.Created).Hours > 1 ? (DateTime.Now - originalPost.Created).Hours.ToString() + " hours "
                        : (DateTime.Now - originalPost.Created).Minutes.ToString() + " minutes ";
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync() {

        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        string fileName = string.Empty;

        if (UploadedImage != null) {
            Random rnd = new();
            fileName = rnd.Next(0, 100000).ToString() + UploadedImage.FileName;
            var file = "./wwwroot/img/" + fileName;
            using var fileStream = new FileStream(file, FileMode.Create);
            await UploadedImage.CopyToAsync(fileStream);
        }

        var oldPost = _postService.GetForumPostById(PostId);
        if (oldPost == null || ReplyAuthorId == null) {
            return NotFound();
        }

        var newPost = new ForumPost() {
            Ancestor = oldPost,
            Author = await _userManager.FindByIdAsync(ReplyAuthorId),
            Content = ReplyContent,
            ImageUrl = fileName,
            Created = DateTime.Now,
        };

        await _postService.Add(newPost);

        int threadIndex = _threadService.GetThreadIdFromPostId(PostId);
        var thread = _threadService.GetThreadById(threadIndex);

        thread.Posts = thread.Posts.Append(newPost).ToList();
        await _threadService.UpdateThread(thread);

        return RedirectToPage($"/Thread/Thread", new { id = thread.Id });
    }
}
