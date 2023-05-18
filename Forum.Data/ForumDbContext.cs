using Forum.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Forum.Data;
public class ForumDbContext : IdentityDbContext<ForumUser> {
    public ForumDbContext(DbContextOptions options) : base(options) {
    }
    /*
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured) {
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS; Database=TestForum; Trusted_Connection=True; Encrypt=false");
        }
    }
    */
    protected override void OnModelCreating(ModelBuilder builder) {
        builder.Entity<PrivateMessage>();
        builder.Entity<GroupMessage>();
        base.OnModelCreating(builder);
        builder.Entity<ForumUser>()
            .HasMany(e => e.MemberGroups)
            .WithMany(e => e.Members);
        builder.Entity<ForumUser>()
            .HasMany(e => e.OwnedGroups)
            .WithOne(e => e.Creator);

        builder.Entity<PrivateMessage>()
            .HasOne(e => e.Reciever)
            .WithMany()
            .OnDelete(DeleteBehavior.ClientSetNull);
        builder.Entity<PrivateMessage>()
            .HasOne(e => e.Sender)
            .WithMany()
            .OnDelete(DeleteBehavior.ClientSetNull);
        builder.Entity<Message>().UseTpcMappingStrategy();
    }

    public DbSet<Avatar> Avatars { get; set; }
    public DbSet<ForumUser> ForumUsers { get; set; } 
    public DbSet<Group> Groups { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<ForumPost> ForumPosts { get; set; }
    public DbSet<PostReport> PostReports { get; set; }
    public DbSet<ForumThread> ForumThreads { get; set; }
    public DbSet<UserPreferences> UserPreferences { get; set; }
    public DbSet<SubForum> Subfora { get; set; }
    public DbSet<Reaction> Reactions { get; set; }

}
