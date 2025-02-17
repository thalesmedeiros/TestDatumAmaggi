using Microsoft.EntityFrameworkCore;
using Datum.Blog.Domain.Entities;

namespace Datum.Blog.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
 
        modelBuilder.Entity<Post>()
            .HasOne(p => p.Autor)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.AutorId)
            .OnDelete(DeleteBehavior.Cascade);

       
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Post>()
            .HasIndex(p => p.DataCriacao);

        modelBuilder.Entity<User>().HasData(new User
        {
            Id = new Guid("b76aeea0-5ddf-4f21-8dbd-5a8a18c7f9d0"),
            Nome = "Admin",
            Email = "admin@email.com",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("admin123"), 
            DataCriacao = DateTime.UtcNow
        });
    }
}