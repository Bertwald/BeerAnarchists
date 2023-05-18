using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Data.Models;
public enum ReactionType {
    None,
    Like,
    Dislike,
    Heart
}
public class Reaction {
    public int Id { get; set; }
    public ReactionType Type { get; set; }
    public required virtual ForumPost Post { get; set; }
    public required virtual ForumUser User { get; set; }

}
