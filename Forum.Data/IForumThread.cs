using Forum.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Data;
public interface IForumThread {
    public ForumThread GetThreadById(int id);
    public int GetThreadIdFromPostId(int postId);
    public Task AddThread(ForumThread newThread);
    public Task UpdateThread(ForumThread thread);
}
