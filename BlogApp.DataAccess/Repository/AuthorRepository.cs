using BlogApp.DataAccess.Data;
using BlogApp.DataAccess.Repository.IRepository;
using BlogApp.Models;

namespace BlogApp.DataAccess.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BlogAppDbContext _dbContext;
        public AuthorRepository(BlogAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(Author author)
        {
            _dbContext.Authors.Add(author);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var author = _dbContext.Authors.Find(id);
            if (author != null)
            {
                _dbContext.Authors.Remove(author);
                _dbContext.SaveChanges();
            }
        }

        public IEnumerable<Author> GetAll()
        {
            return _dbContext.Authors.ToList();
        }

        public Author GetById(int id)
        {
            return _dbContext.Authors.Find(id);
        }

        public void Update(Author author)
        {
            _dbContext.Authors.Add(author);
            _dbContext.SaveChanges();
        }
    }
}
