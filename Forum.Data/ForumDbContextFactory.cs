using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Forum.Data;

public class ForumDbContextFactory : IDesignTimeDbContextFactory<ForumDbContext> {
    public ForumDbContext CreateDbContext(string[] args) {
        var optionsBuilder = new DbContextOptionsBuilder<ForumDbContext>();
        optionsBuilder.UseSqlServer("Server =.\\SQLEXPRESS; Database = TestForum; Trusted_Connection = True; Encrypt = false");

        return new ForumDbContext(optionsBuilder.Options);
    }
}
