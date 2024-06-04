using BlogApp.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
        [HttpGet(Name = "GetAuthorById")]
        public IActionResult GetAuthor(int id)
        {
            return Ok(_authorRepository.GetById(id));
        }
        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    return Ok(_authorRepository.GetAll());
        //}
    }
}