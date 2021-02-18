using Megatron.Data;
using Megatron.Models;
using Megatron.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Megatron.Services
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public StudentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<Article> GetAllArticles()
        {
            return _dbContext.Articles.ToList();
        }

        public ArticleFacultyViewModel ArticleFacultyViewModel()
        {
            ArticleFacultyViewModel articleFacultyViewModel = new ArticleFacultyViewModel()
            {
                Article = new Models.Article(),
                Faculties = _dbContext.Faculties.OrderBy(f => f.FacultyName).Distinct().ToList()
            };
            return articleFacultyViewModel;
        }

        public ArticleFacultyViewModel SubmitArticle(ArticleFacultyViewModel articleFacultyViewModel)
        {
            var doesArticleExists = _dbContext.Articles.Include(a => a.Faculty)
                                                        .Where(a => a.Title == articleFacultyViewModel.Article.Title && a.Faculty.Id == articleFacultyViewModel.Article.FacultyId);

            if (doesArticleExists.Any())
            {
                // Message
            }
            else
            {
                _dbContext.Articles.Add(articleFacultyViewModel.Article);
                _dbContext.SaveChanges();
                return null;
            }

            ArticleFacultyViewModel model = new ArticleFacultyViewModel()
            {
                Article = articleFacultyViewModel.Article,
                Faculties = _dbContext.Faculties.ToList(),
                StatusMessage = "Error: Article already exists in " + _dbContext.Faculties.SingleOrDefault(f => f.Id == articleFacultyViewModel.Article.FacultyId).FacultyName.ToString()
            };

            return model;
        }
    }
}