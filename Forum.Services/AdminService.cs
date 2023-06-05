using Forum.Data;
using Forum.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum.Services;
/// <summary>
/// Used by administrator to perform different admin associated tasks
/// </summary>
public sealed class AdminService {

    private readonly ForumDbContext _context;

    public AdminService(ForumDbContext context) {
        _context = context;
    }

    public IEnumerable<PostReport> GetReportedPosts(ReportStatus? status = null) {

        if (_context.PostReports == null) {
            return Enumerable.Empty<PostReport>();
        }

        var posts = _context.PostReports
            .Include(x => x.ReportedPost)
            .Include(x => x.Reporter)
            .Include(x => x.Reported)
            // TODO: Debug to ensure == is correct
            .Where(x => status == null || x.Status == status)
            .OrderBy(x => x.Reported.Id)
            .ThenBy(x => x.Status)
            .AsEnumerable();

        return posts;
    }

    public async Task<bool> SetReportStatus(PostReport report, ReportStatus status) {
        if (report == null || report.Status == status) {
            return false;
        }
            //Special troll treatment
        if (status == ReportStatus.Trollstatus) {
            var culprit = report.Reported;
            if (culprit != null) {
                culprit.ImageUrl = "avatars/chicken.jpg";
                culprit.Alias = "Captain McNugget";
                culprit.LockoutEnabled = true;
                culprit.LockoutEnd = DateTime.Now.AddDays(2);
                _context.Entry(culprit).State = EntityState.Modified;
            }
        }

        report.Status = status;
        _context.Entry(report).State = EntityState.Modified;
        try {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            return false;
        }
        return default;
    }

    public async Task<PostReport> GetPostReportById(int id) {
        return await _context.PostReports
            .Where(report => report.Id == id)
            .Include(x => x.Reported)
            .Include(x => x.Reporter)
            .Include(x => x.ReportedPost)
            .FirstOrDefaultAsync();
    }

}
