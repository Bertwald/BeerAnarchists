using Microsoft.AspNetCore.Identity;

namespace Forum.Data.Models;

public class ForumUser : IdentityUser {
    public string? Alias { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public int Posts { get; set; }
    public virtual Avatar? Avatar { get; set; }
    public virtual IEnumerable<ForumUser> Friends { get; set; } = Enumerable.Empty<ForumUser>();
    public virtual IEnumerable<ForumUser> Ignored { get; set; } = Enumerable.Empty<ForumUser>();
    public virtual IEnumerable<Group> MemberGroups { get; set; } = Enumerable.Empty<Group>();
    public virtual IEnumerable<Group> OwnedGroups { get; set; } = Enumerable.Empty<Group>();
}
