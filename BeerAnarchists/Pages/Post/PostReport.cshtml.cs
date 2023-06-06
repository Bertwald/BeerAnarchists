using Forum.Data.Interfaces;
using Forum.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeerAnarchists.Pages.Post;

public class PostReportModel : PageModel {
    private readonly IForumPost _postManager;
    private readonly UserManager<ForumUser> _userManager;

    [BindProperty]
    public string? ReporterId { get; set; }
    [BindProperty]
    public string ReportedId { get; set; }
    [BindProperty]
    public int PostId { get; set; }
    [BindProperty]
    public string? ReportMessage { get; set; }

    public PostReportModel(IForumPost postManager, UserManager<ForumUser> userManager) {
        _postManager = postManager;
        _userManager = userManager;
    }

    public async Task<ActionResult> OnGetAsync(string? reporterId, string reportedId, int postId) {
        ReporterId= reporterId;
        ReportedId= reportedId;
        PostId= postId;

        return Page();
    }

    public async Task<ActionResult> OnPostAsync() {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        PostReport report = new () {
            Created = DateTime.Now,
            Message = ReportMessage,
            Reporter = await _userManager.FindByIdAsync(ReporterId),
            Reported = await _userManager.FindByIdAsync(ReportedId),
            Status = ReportStatus.None,
            ReportedPost = _postManager.GetForumPostById(PostId),
        };

        await _postManager.AddReport(report);

        return RedirectToPage("/Index");
    }

}
