using Forum.Data;
using Forum.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Forum.Services;
public sealed class SubforumService : ISubforums {
    private readonly ForumDbContext _context;

    public SubforumService(ForumDbContext context) {
        _context = context;
    }

    public Task Create(SubForum subForum) {
        throw new NotImplementedException();
    }

    public Task Delete(int id) {
        throw new NotImplementedException();
    }

    public SubForum GetById(int id) {
        throw new NotImplementedException();
    }

    public IEnumerable<SubForum> GetSubforums() {
        return _context.Subfora.Include(subforum => subforum.ForumThreads);
    }

    public Task UpdateDescription(int id, string description) {
        throw new NotImplementedException();
    }

    public Task UpdateImage(int id, string imageUrl) {
        throw new NotImplementedException();
    }

    public Task UpdateTitle(int id, string title) {
        throw new NotImplementedException();
    }
}
