using BlogApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.DataAccess.Data
{
    public class BlogAppDbContext : DbContext
    {
        public BlogAppDbContext(DbContextOptions<BlogAppDbContext> options) : base(options)
        {

        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Author> Authors { get; set; }

        // seeding data in database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Human Resource" },
                new Category { Id = 2, Name = "Technology" },
                new Category { Id = 3, Name = "Travel" },
                new Category { Id = 4, Name = "Food" }
            );

            // Seed Authors
            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "Ankush Kumar" },
                new Author { Id = 2, Name = "Atul Sharma" },
                new Author { Id = 3, Name = "Sahil" },
                new Author { Id = 4, Name = "Aditi" },
                new Author { Id = 5, Name = "Manish" }
            );

            // Seed Posts
            modelBuilder.Entity<Post>().HasData(
                new Post { Id = 1, Title = "ASP.NET Core", Content = "Introduction to ASP.NET Core", CategoryId = 2, AuthorId = 3 },
                new Post { Id = 2, Title = "10 Tips for Traveling on a Budget", Content = "Travelling Information", CategoryId = 3, AuthorId = 4 },
                new Post { Id = 3, Title = "Food Blog", Content = "Food Information", CategoryId = 4, AuthorId = 1 }
            );
        }
    }
}

