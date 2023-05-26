namespace Forum.Data.Models;

public class ForumPost {
    public int Id { get; set; }
    public string? ImageUrl { get; set; }
    public required string Content { get; set; }
    public DateTime Created { get; set; }
    public virtual ForumUser Author { get; set; } = null!;
    public virtual IEnumerable<ForumPost> Replies { get; set; }
    public virtual IEnumerable<Reaction> Reactions { get; set; }
    public virtual ForumPost? Ancestor { get; set; }
}
