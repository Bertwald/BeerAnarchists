namespace Forum.Data.Models;

public class ForumThread {
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public virtual IEnumerable<ForumPost> Posts { get; set; } = new List<ForumPost>();
}
