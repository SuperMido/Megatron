using System;
using System.Collections.Generic;
using System.Linq;
using Megatron.Data;
using Megatron.Models;
using Megatron.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Megatron.Services
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ISemesterRepository _semesterRepository;

        public StudentRepository(ApplicationDbContext dbContext, ISemesterRepository semesterRepository)
        {
            _dbContext = dbContext;
            _semesterRepository = semesterRepository;
        }

        public ArticleFacultyViewModel ArticleFacultyViewModel(string userName)
        {
            var activeSemester = _semesterRepository.GetActiveSemester();
            var articleFacultyViewModel = new ArticleFacultyViewModel
            {
                Article = new Article(),
                Semester = activeSemester,
                Faculties = _dbContext.Faculties.OrderBy(f => f.FacultyName).Distinct().ToList(),
                UserFullName = userName
            };
            return articleFacultyViewModel;
        }

        public IEnumerable<ArticleSemesterFacultyViewModel> GetPersonalArticles(string userName)
        {
            var listPersonalArticles = (from article in _dbContext.Articles
                where article.Author.Contains(userName)
                join semesterArticle in _dbContext.SemesterArticles on article.Id equals semesterArticle.ArticleId
                join semester in _dbContext.Semesters on semesterArticle.SemesterId equals semester.Id
                join faculty in _dbContext.Faculties on article.FacultyId equals faculty.Id
                select new
                {
                    Article = article, SemesterEndDate = semester.SemesterEndDate, Faculty = faculty
                })
                .Select(a => new ArticleSemesterFacultyViewModel()
            {
                Article = a.Article,
                Faculties = a.Faculty,
                CheckValidEdit = CheckValidEditArticle(a.SemesterEndDate),
            }).Distinct().ToList();
            return listPersonalArticles;
        }

        public ArticleFacultyViewModel SubmitArticle(ArticleFacultyViewModel articleFacultyViewModel)
        {
            var doesArticleExists = _dbContext.Articles.Include(a => a.Faculty)
                .Where(a => a.Title == articleFacultyViewModel.Article.Title &&
                            a.Faculty.Id == articleFacultyViewModel.Article.FacultyId);

            if (doesArticleExists.Any())
            {
                // Message
            }
            else
            {
                var activeSemester = _semesterRepository.GetActiveSemester();
                var articleResult = _dbContext.Articles.Add(articleFacultyViewModel.Article);
                _dbContext.SaveChanges();
                _semesterRepository.AddArticleSemester(articleResult.Entity.Id, activeSemester.Id);
                return null;
            }

            var model = new ArticleFacultyViewModel
            {
                Article = articleFacultyViewModel.Article,
                Faculties = _dbContext.Faculties.ToList(),
                StatusMessage = "Error: Article already exists in " + _dbContext.Faculties
                    .SingleOrDefault(f => f.Id == articleFacultyViewModel.Article.FacultyId).FacultyName.ToString()
            };

            return model;
        }

        public Article GetArticleById(int id)
        {
            return _dbContext.Articles.SingleOrDefault(a => a.Id == id);
        }

        public IEnumerable<ApplicationUser> GetUserInFacultyByFacultyId(int id)
        {
            var listUserIdInSelectedFaculty = _dbContext.UserFaculties
                .Where(userFaculty => userFaculty.FacultyId == id).Select(u => u.UserId).ToArray();

            var listUserInSelectedFaculty = _dbContext.ApplicationUsers
                .Where(user => listUserIdInSelectedFaculty.Any(userId => userId.Equals(user.Id))).ToList();

            return listUserInSelectedFaculty;
        }

        public ArticleFacultyViewModel EditArticle(ArticleFacultyViewModel articleFacultyViewModel)
        {
            var doesArticleExists = _dbContext.Articles.Include(a => a.Faculty)
                .Where(a => a.Title == articleFacultyViewModel.Article.Title &&
                            a.Faculty.Id == articleFacultyViewModel.Article.FacultyId);
            var articleInDb = GetArticleById(articleFacultyViewModel.Article.Id);
            if (doesArticleExists.Any() && articleInDb.Title != articleFacultyViewModel.Article.Title)
            {
                // Message
            }
            else
            {
                articleInDb.Title = articleFacultyViewModel.Article.Title;
                articleInDb.Content = articleFacultyViewModel.Article.Content;

                _dbContext.SaveChanges();
                return null;
            }

            var model = new ArticleFacultyViewModel
            {
                Article = articleFacultyViewModel.Article,
                Faculties = _dbContext.Faculties.ToList(),
                StatusMessage = "Error: Article already exists in " + _dbContext.Faculties
                    .SingleOrDefault(f => f.Id == articleFacultyViewModel.Article.FacultyId).FacultyName.ToString()
            };

            return model;
        }

        private static bool CheckValidEditArticle(DateTime semesterEndDate)
        {
            var currentDate = DateTime.Now;
            if (currentDate < semesterEndDate)
            {
                return true;
            }
            return false;
        }
    }
}