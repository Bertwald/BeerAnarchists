using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Data.Models;
public class SubForum {
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required string Author { get; set; }
    public string? ImageUrl { get; set; }
    public virtual IEnumerable<ForumThread> ForumThreads { get; set; }
}
