using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Forum.Data;

public class ForumDbContextFactory : IDesignTimeDbContextFactory<ForumDbContext> {
    public ForumDbContext CreateDbContext(string[] args) {
        var optionsBuilder = new DbContextOptionsBuilder<ForumDbContext>();
        optionsBuilder.UseSqlServer("Server =.\\SQLEXPRESS; Database = TestForum; Trusted_Connection = True; Encrypt = false");

        return new ForumDbContext(optionsBuilder.Options);
    }
}
