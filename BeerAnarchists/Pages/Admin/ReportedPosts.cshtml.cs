using Forum.Data;
using Forum.Data.Models;
using Forum.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace BeerAnarchists.Pages.Admin;

[Authorize(Roles = "Admin")]
public class ReportedPostsModel : PageModel {
    private readonly AdminService _adminService;
    private readonly IForumPost _postService;
    [BindProperty]
    public IEnumerable<PostReport> PostReports { get; set; }
    [BindProperty]
    public ReportStatus NewStatus { get; set; }
    [BindProperty]
    public int ReportId { get; set; }

    public ReportedPostsModel(AdminService adminService, IForumPost postService) {
        _adminService = adminService;
        _postService = postService;
        PostReports = new List<PostReport>();
    }

    public async Task<ActionResult> OnGetAsync() {
        PostReports = _adminService.GetReportedPosts();

        return Page();
    }

    public async Task<ActionResult> OnPostAsync() {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        var report = await _adminService.GetPostReportById(ReportId);
        if (report != null) {
            await _adminService.SetReportStatus(report, NewStatus);
        }


        return RedirectToPage();
    }
}
