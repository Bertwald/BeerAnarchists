using Forum.Data.Models;

namespace Forum.Data.Interfaces;
public interface ISubforum
{
    public SubForum GetById(int id);
    public IEnumerable<SubForum> GetSubforums();
    public Task Create(SubForum subForum);
    public Task AddThread(SubForum forum, ForumThread thread);
    public Task Delete(int id);
    public Task UpdateTitle(int id, string title);
    public Task UpdateDescription(int id, string description);
    public Task UpdateImage(int id, string imageUrl);
}
