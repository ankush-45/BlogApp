using BlogApp.Models;
using BlogApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.DataAccess.Repository.IRepository
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetPostsByAuthorAsync(int authorId);
        Task<Post> GetPostByIdAsync(int id);
        Task<Post> UpdatePostCategory(UpdatePostCategoryVM inputVM);
        Task<List<CategoryPostCountVM>> GetCategoryPostCoutsAsync();
        //Task<IEnumerable<Post>> GetAllPostsAsync();
    }
}
