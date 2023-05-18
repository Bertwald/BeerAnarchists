namespace Forum.Data.Models;

public class Group {
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime Created { get; set; }
    public required virtual ForumUser Creator { get; set; }
    public virtual IEnumerable<ForumUser> Members { get; set; } = Enumerable.Empty<ForumUser>();
}
