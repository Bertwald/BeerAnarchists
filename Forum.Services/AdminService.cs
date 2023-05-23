using Forum.Data;
using Forum.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

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
            .Where(x => status == null || x.Status==status)
            .OrderBy(x => x.Reported.Id)
            .ThenBy(x => x.Status)
            .AsEnumerable();

        return posts;
    }

    public async Task<bool> SetReportStatus(PostReport report, ReportStatus status) {
        if (report == null || report.Status == status) {
            return false;
        } else {
            report.Status = status;
            _context.Entry(report).State = EntityState.Modified;
            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                return false;
            }
        }
        return default;
    }

    


}
