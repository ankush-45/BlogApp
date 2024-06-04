using BlogApp.DataAccess.Data;
using BlogApp.DataAccess.Repository.IRepository;
using BlogApp.Models;
using BlogApp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.DataAccess.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly BlogAppDbContext _dbContext;
        public PostRepository(BlogAppDbContext dbContext)
        {
                _dbContext = dbContext;
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _dbContext.Posts.FindAsync(id);
        }

        public async Task<List<CategoryPostCountVM>> GetCategoryPostCoutsAsync()
        {
            return  await _dbContext.Posts.Include(p=>p.Category).GroupBy(p => new { p.CategoryId, p.Category.Name }).Select(grp =>
                new CategoryPostCountVM
                {
                    Category = grp.Key.Name,
                    PostCount = grp.Count()
                }).ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByAuthorAsync(int authorId)
        {
            return await _dbContext.Posts.Where(p => p.AuthorId == authorId).ToListAsync();
        }

        public async Task<Post> UpdatePostCategory(UpdatePostCategoryVM inputVM)
        {
            var post = await _dbContext.Posts.FindAsync(inputVM.PostId); 
            if(post != null)
            {
                post.CategoryId = inputVM.CategoryId;
                 _dbContext.Posts.Update(post);
                await _dbContext.SaveChangesAsync();
            }
            return post;
        }

    }
}
