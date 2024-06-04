using BlogApp.API.Controllers;
using BlogApp.DataAccess.Data;
using BlogApp.DataAccess.Repository;
using BlogApp.DataAccess.Repository.IRepository;
using BlogApp.Models;
using BlogApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Test
{
    public class PostsControllerTests
    {
        private readonly Mock<ILogger<PostsController>> _mockLogger;
        private readonly PostsController _controller;
        private readonly BlogAppDbContext _context;
        private readonly IPostRepository _repository;

        public PostsControllerTests()
        {
            //setting in memory db context
            var options = new DbContextOptionsBuilder<BlogAppDbContext>()
                .UseInMemoryDatabase(databaseName: "BlogAppTestDb")
                .Options;

            _context = new BlogAppDbContext(options);
            SeedData(_context);

            _repository = new PostRepository(_context);
            _mockLogger = new Mock<ILogger<PostsController>>();
            _controller = new PostsController(_repository, _mockLogger.Object);
        }
        //Seeding data into in memory database
        private void SeedData(BlogAppDbContext context)
        {
            var authors = new List<Author>
        {
            new Author { Id = 1, Name = "Author 1" },
            new Author { Id = 2, Name = "Author 2" }
        };

            var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Category 1" },
            new Category { Id = 2, Name = "Category 2" }
        };

            var posts = new List<Post>
        {
            new Post { Id = 1, Title = "Post 1", AuthorId = 1, CategoryId = 1, Content = "Content 1" },
            new Post { Id = 2, Title = "Post 2", AuthorId = 1, CategoryId = 2, Content = "Content 2" },
            new Post { Id = 3, Title = "Post 3", AuthorId = 2, CategoryId = 1, Content = "Content 3" }
        };

            context.Authors.AddRange(authors);
            context.Categories.AddRange(categories);
            context.Posts.AddRange(posts);
            context.SaveChanges();
        }

        [Fact]
        public async Task GetPostAsync_ShouldReturnOk_WithPost()
        {
            var result = await _controller.GetPostAsync(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPost = Assert.IsType<Post>(okResult.Value);
            Assert.Equal(1, returnedPost.Id);
            Assert.Equal("Post 1", returnedPost.Title);
        }

        [Fact]
        public async Task GetPostAsync_ShouldReturnNotFound_WhenPostDoesNotExist()
        {
            var result = await _controller.GetPostAsync(9); //No post seeded with ID 9

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetPostsByAuthorAsync_ShouldReturnOk_WithPosts()
        {
            var result = await _controller.GetPostsByAuthorAsync(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPosts = Assert.IsType<List<Post>>(okResult.Value);
            Assert.Equal(2, returnedPosts.Count);
            Assert.Contains(returnedPosts, p => p.Title == "Post 1" && p.AuthorId == 1);
            Assert.Contains(returnedPosts, p => p.Title == "Post 2" && p.AuthorId == 1);
        }

        [Fact]
        public async Task GetPostsByAuthorAsync_ShouldReturnNotFound_WhenNoPostsExist()
        {
            var result = await _controller.GetPostsByAuthorAsync(9);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task UpdatePostCategoryAsync_ShouldReturnOk_WithUpdatedPost()
        {
            var updateVM = new UpdatePostCategoryVM { PostId = 1, CategoryId = 2 };

            var result = await _controller.UpdatePostCategoryAsync(updateVM);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedPost = Assert.IsType<Post>(okResult.Value);
            Assert.Equal(1, updatedPost.Id);
            Assert.Equal(2, updatedPost.CategoryId);
        }

        [Fact]
        public async Task UpdatePostCategoryAsync_ShouldReturnNotFound_WhenPostDoesNotExist()
        {
            var updateVM = new UpdatePostCategoryVM { PostId = 9, CategoryId = 2 };

            var result = await _controller.UpdatePostCategoryAsync(updateVM);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetCategoryPostCoutsAsync_ShouldReturnOk_WithCategoryPostCounts()
        {
            var result = await _controller.GetCategoryPostCoutsAsync();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCounts = Assert.IsType<List<CategoryPostCountVM>>(okResult.Value);
            Assert.Equal(2, returnedCounts.Count);
            Assert.Contains(returnedCounts, c => c.Category == "Category 1" && c.PostCount == 2);
            Assert.Contains(returnedCounts, c => c.Category == "Category 2" && c.PostCount == 1);

        }
    }
}
