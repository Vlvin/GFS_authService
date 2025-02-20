using Backend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Database;
public class Context(DbContextOptions<Context> options) : IdentityDbContext<AuthUser>(options)
{
    required public DbSet<Post> Posts { get; set; }
    required public DbSet<Question> Questions { get; set; }
    required public DbSet<Answer> Answers { get; set; }
    //    required public DbSet<AuthUser> DbUsers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<AuthUser>()
                .HasIndex(i => i.Email)
                .IsUnique();
        //        modelBuilder.Entity<AuthUser>()
        //                .HasKey(u => new { u.Id, u.Email });

        modelBuilder.Entity<AuthUser>()
                .HasMany(i => i.Posts)
                .WithOne(i => i.Author);
        //modelBuilder.Entity<Post>()
        //        .HasOne(i => i.Author)
        //        .WithMany(i => i.Posts);
        modelBuilder.Entity<Post>()
                .HasMany(i => i.Questions);
        modelBuilder.Entity<Post>()
                .HasMany(i => i.Likes);

        modelBuilder.Entity<Question>()
                .HasMany(i => i.Answers)
                .WithOne(i => i.Question);
    }
}

