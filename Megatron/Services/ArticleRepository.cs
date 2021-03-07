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

        public IEnumerable<Article> GetListArticlesApproved()
        {
            List<Article> articles = _dbContext.Articles.Include(a => a.Faculty)
                .Where(a => a.Status == true)
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
            var documentUploaded = _dbContext.ArticleDocuments.Where(d => d.ArticleId == id).ToList();
            var model = new CommentArticleViewModel
            {
                Article = article,
                Document = documentUploaded,
                Comments = listComments
            };
            return model;
        }

        public bool UpdateArticleStatus(int id, bool status, string message, string changeStatusBy)
        {
            var articleInDb = _dbContext.Articles.FirstOrDefault(a => a.Id == id);
            if (articleInDb == null) return false;
            articleInDb.Status = status;
            articleInDb.StatusMessage = message;
            articleInDb.ChangeStatusBy = changeStatusBy;
            _dbContext.Articles.Update(articleInDb);
            _dbContext.SaveChanges();

            return true;
        }

        public IEnumerable<Faculty> GetFacultiesForMC(string id)
        {
            var listFaculty = (from user in _dbContext.ApplicationUsers
                    join userFaculty in _dbContext.UserFaculties on id equals userFaculty.UserId
                    join faculty in _dbContext.Faculties on userFaculty.FacultyId equals faculty.Id
                    select new
                    {
                        UserId = user.Id, User = user, FacultyId = faculty.Id, faculty.FacultyName,
                        DescriptionF = faculty.Description, DateCreate = faculty.CreateAt
                    })
                .Select(m => new Faculty
                {
                    Id = m.FacultyId,
                    FacultyName = m.FacultyName,
                    Description = m.DescriptionF,
                    CreateAt = m.DateCreate
                }).Distinct().ToList();
            return listFaculty;
        }
    }
}