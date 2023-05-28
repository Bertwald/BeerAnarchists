using Forum.Data;
using Forum.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Services;
public sealed class ForumThreadsService : IForumThread {
    private ForumDbContext _context;

    public ForumThreadsService(ForumDbContext context) {
        _context = context;
    }

    public ForumThread GetThreadById(int id) {
        return _context.ForumThreads
            .Include(x => x.Posts)
            .ThenInclude(p => p.Author)
            .Include(x=> x.Posts)
            .ThenInclude(p => p.Reactions)
            .Where(x => x.Id == id)
            .FirstOrDefault();
    }
}
