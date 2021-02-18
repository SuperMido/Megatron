using System.Collections.Generic;
using System.Linq;
using Megatron.Data;
using Megatron.Models;
using Microsoft.EntityFrameworkCore;

namespace Megatron.Services
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ArticleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Article> GetAllArticle()
        {
            var articles = _dbContext.Articles.ToList();
            return articles;
        }

        public IEnumerable<Article> GetListArticlesByFaculty(int facultyId)
        {
            var articles = _dbContext.Articles.Include(a => a.Faculty)
                .Where(a => a.FacultyId == facultyId)
                .ToList();
            return articles;
        }

        public IEnumerable<Faculty> GetFaculties()
        {
            var faculties = _dbContext.Faculties.ToList();
            return faculties;
        }

        public Article GetArticleDetail(int id)
        {
            var article = _dbContext.Articles.FirstOrDefault(a => a.Id == id);

            return article;
        }
    }
}