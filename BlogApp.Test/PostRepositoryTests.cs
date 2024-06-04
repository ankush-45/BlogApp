using BlogApp.DataAccess.Data;
using BlogApp.DataAccess.Repository;
using BlogApp.DataAccess.Repository.IRepository;
using BlogApp.Models;
using BlogApp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Test
{
    public class PostRepositoryTests
    {
        private async Task<BlogAppDbContext> GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<BlogAppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new BlogAppDbContext(options);
            context.Database.EnsureCreated();

            // Seeding the database with test data 
            context.Authors.Add(new Author { Id = 1, Name = "Author 1" });
            context.Categories.Add(new Category { Id = 1, Name = "Category 1" });
            context.Categories.Add(new Category { Id = 2, Name = "Category 2" });
            context.Posts.Add(new Post { Id = 1, Title = "Post 1", Content = "Content 1", AuthorId = 1, CategoryId = 1 });
            context.Posts.Add(new Post { Id = 2, Title = "Post 2", Content = "Content 2", AuthorId = 1, CategoryId = 1 });
            context.Posts.Add(new Post { Id = 3, Title = "Post 3", Content = "Content 3", AuthorId = 2, CategoryId = 2 });
            await context.SaveChangesAsync();

            return context;
        }

        [Fact]
        public async Task GetPostByIdAsync_ShouldReturnPost()
        {
            var context = await GetInMemoryDbContext();
            IPostRepository repository = new PostRepository(context);

            var post = await repository.GetPostByIdAsync(1);

            Assert.NotNull(post);
            Assert.Equal(1, post.Id);
            Assert.Equal("Post 1", post.Title);
            Assert.Equal("Content 1", post.Content);
        }

        [Fact]
        public async Task GetPostsByAuthorAsync_ShouldReturnPosts()
        {
            var context = await GetInMemoryDbContext();
            IPostRepository repository = new PostRepository(context);

            var posts = await repository.GetPostsByAuthorAsync(1);

            Assert.NotEmpty(posts);
            Assert.Equal(2, posts.Count()); // Should match count 2 as there are 2 Posts with AuthorId 1
        }
        [Fact]
        public async Task GetCategoryPostCoutsAsync_ShouldReturnCorrectCounts()
        {
            var context = await GetInMemoryDbContext();
            IPostRepository repository = new PostRepository(context);

            var result = await repository.GetCategoryPostCoutsAsync();

            Assert.Equal(2, result.Count); // Should be 2 as 2 type of Category Posts Exists
            Assert.Contains(result, c => c.Category == "Category 1" && c.PostCount == 2);
            Assert.Contains(result, c => c.Category == "Category 2" && c.PostCount == 1);
        }
        [Fact]
        public async Task UpdatePostCategory_ShouldUpdateCategory()
        {
            var context = await GetInMemoryDbContext();
            IPostRepository repository = new PostRepository(context);

            var updateVM = new UpdatePostCategoryVM 
            { 
                PostId = 1,
                CategoryId = 2 
            };
            var updatedPost = await repository.UpdatePostCategory(updateVM);

            Assert.NotNull(updatedPost);
            Assert.Equal(2, updatedPost.CategoryId);

            var post = await repository.GetPostByIdAsync(1);
            Assert.Equal(2, post.CategoryId);
        }
    }
}