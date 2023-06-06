using Microsoft.AspNetCore.Identity;

namespace Forum.Data.Models;

public class ForumUser : IdentityUser {
    public string? Alias { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public int Posts { get; set; }
    public DateTime MemberSince { get; set; }
    public DateTime? LastPost { get; set; }
    public virtual Avatar? Avatar { get; set; }
    public virtual IEnumerable<ForumUser> Friends { get; set; } = new List<ForumUser>();
    public virtual IEnumerable<ForumUser> Ignored { get; set; } = new List<ForumUser>();
    public virtual IEnumerable<Group> MemberGroups { get; set; } = new List<Group>();
    public virtual IEnumerable<Group> OwnedGroups { get; set; } = new List<Group>();
    public virtual IEnumerable<Group> Applications { get; set; } = new List<Group>();
    public virtual IEnumerable<Group> Invitations { get; set; } = new List<Group>();
}
