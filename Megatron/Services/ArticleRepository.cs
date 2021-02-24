using System.Collections.Generic;
using System.Linq;
using Megatron.Data;
using Megatron.Models;
using Megatron.ViewModels;
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

        public CommentArticleViewModel GetArticleDetail(int id)
        {
            var article = _dbContext.Articles.FirstOrDefault(a => a.Id == id);
            var listComments = _dbContext.CommentArticles.Include(u => u.ApplicationUser).Include(a => a.Article)
                .Where(c => c.ArticleId == id);

            var model = new CommentArticleViewModel
            {
                Article = article,
                Comments = listComments
            };
            return model;
        }

        public bool UpdateArticleStatus(int id, bool status, string message)
        {
            var articleInDb = _dbContext.Articles.FirstOrDefault(a => a.Id == id);
            if (articleInDb == null) return false;
            articleInDb.Status = status;
            articleInDb.StatusMessage = message;
            _dbContext.Articles.Update(articleInDb);
            _dbContext.SaveChanges();

            return true;
        }
    }
}