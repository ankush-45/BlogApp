using BlogApp.DataAccess.Repository.IRepository;
using BlogApp.Models;
using BlogApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IPostRepository _repository;

        public PostsController(IPostRepository repository, ILogger<PostsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostAsync(int id)
        {
            _logger.LogInformation("Getting Post by ID {Id}", id);

            try
            {
                var post = await _repository.GetPostByIdAsync(id);
                if (post == null)
                {
                    _logger.LogInformation("No Post found by ID {Id}", id);
                    return NotFound(new { Message = "Post not found" });
                }

                return Ok(post);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error getting Post with ID {Id}", id);
                return StatusCode(500, new { Message = "An error occurred", Details = ex.Message });
            }
        }
        [HttpGet("getPosts/{authorId}")]
        public async Task<IActionResult> GetPostsByAuthorAsync(int authorId)
        {
            _logger.LogInformation("Getting Posts by Author with ID {AuthorId}", authorId);

            try
            {
                var posts = await _repository.GetPostsByAuthorAsync(authorId);
                if (posts == null || !posts.Any())
                {
                    _logger.LogWarning("No Posts found for Author with ID {AuthorId}", authorId);
                    return NotFound(new { Message = "No Posts found for the specified Author" });
                }

                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Posts for Author with ID {AuthorId}", authorId);
                return StatusCode(500, new { Message = "An error occurred", Details = ex.Message });
            }
        }
        [HttpPut("updatePostCategory")]
        public async Task<IActionResult> UpdatePostCategoryAsync(UpdatePostCategoryVM inputVM)
        {
            _logger.LogInformation("Updating Post Category");
            try
            {
                var updatedPost = await _repository.UpdatePostCategory(inputVM);
                if(updatedPost == null)
                {
                    _logger.LogWarning("No Post found with ID {PostId}", inputVM.PostId);
                    return NotFound(new { Message = "No Post found with the specified ID" });
                }
                return Ok(updatedPost);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Post's Category");
                return StatusCode(500, new { Message = "An error occurred", Details = ex.Message });
            }
        }
        [HttpGet("getCategoryPostsCount")]
        public async Task<IActionResult> GetCategoryPostCoutsAsync()
        {
            _logger.LogInformation("Getting Post count with Category");
            try
            {
                var postCountWithCategory = await _repository.GetCategoryPostCoutsAsync();
                return Ok(postCountWithCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting Category Post count");
                return StatusCode(500, new { Message = "An error occurred", Details = ex.Message });
            }
        }

    }
}
