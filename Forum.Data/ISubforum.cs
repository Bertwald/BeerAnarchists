using Forum.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Data;
public interface ISubforum {
    public SubForum GetById(int id);
    public IEnumerable<SubForum> GetSubforums();
    public Task Create(SubForum subForum);
    public Task Delete(int id);
    public Task UpdateTitle(int id, string title);
    public Task UpdateDescription(int id, string description);
    public Task UpdateImage(int id, string imageUrl);
}
