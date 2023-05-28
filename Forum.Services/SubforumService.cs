using Forum.Data;
using Forum.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Forum.Services;
public sealed class SubforumService : ISubforum {
    #region Fields
    private readonly ForumDbContext _context;
    #endregion
    #region Constructors
    public SubforumService(ForumDbContext context) {
        _context = context;
    }
    #endregion

    public Task Create(SubForum subForum) {
        throw new NotImplementedException();
    }

    public Task Delete(int id) {
        throw new NotImplementedException();
    }

    public SubForum GetById(int id) {
        return _context.Subfora
            .Where(x => x.Id == id)
            .Include(subforum => subforum.ForumThreads)
            .ThenInclude(thread => thread.Posts)
            .ThenInclude(post => post.Author)
            .FirstOrDefault();
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
