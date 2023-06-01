using Forum.Data;
using Forum.Data.Models;
using Forum.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeerAnarchists.Pages.Post;

public class NewPostModel : PageModel {
    #region Services
    private readonly IForumThread _threadService;
    private readonly IForumPost _postService;
    private readonly UserManager<ForumUser> _userManager;
    #endregion
    #region InputFields
    [BindProperty]
    public string UserId { get; set; }
    [BindProperty]
    public string PostAuthorId { get; set; }
    [BindProperty]
    public int ThreadId { get; set; }
    [BindProperty]
    public IFormFile? UploadedImage { get; set; }
    [BindProperty]
    public string PostContent { get; set; }
    #endregion
    public NewPostModel(IForumThread forumThreadsService, IForumPost forumPostService, UserManager<ForumUser> userManager) {
        _threadService = forumThreadsService;
        _postService = forumPostService;
        _userManager = userManager;
    }

    public void OnGet(int threadID, string userId) {
        ThreadId = threadID;
        PostAuthorId = userId;
    }

    public async Task<IActionResult> OnPostAsync(int threadId, string userId) {

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

        var newPost = new ForumPost() {
            Ancestor = null,
            Author = await _userManager.FindByIdAsync(PostAuthorId),
            Content = PostContent,
            ImageUrl = fileName,
            Created = DateTime.Now,
        };

        await _postService.Add(newPost);
        var thread = _threadService.GetThreadById(threadId);

        thread.Posts = thread.Posts.Append(newPost).ToList();
        await _threadService.UpdateThread(thread);

        return RedirectToPage($"/Thread/Thread", new { id = threadId });
    }
}
