﻿using Megatron.Data;
using Megatron.Models;
using Megatron.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Megatron.Services
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public StudentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ArticleFacultyViewModel ArticleFacultyViewModel(string userName)
        {
            var articleFacultyViewModel = new ArticleFacultyViewModel()
            {
                Article = new Models.Article(),
                Faculties = _dbContext.Faculties.OrderBy(f => f.FacultyName).Distinct().ToList(),
                UserFullName = userName
            };
            return articleFacultyViewModel;
        }

        public IEnumerable<Article> GetPersonalArticles(string userName)
        {
            return _dbContext.Articles.Where(a => a.Author == userName).ToList();
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

            var model = new ArticleFacultyViewModel()
            {
                Article = articleFacultyViewModel.Article,
                Faculties = _dbContext.Faculties.ToList(),
                StatusMessage = "Error: Article already exists in " + _dbContext.Faculties.SingleOrDefault(f => f.Id == articleFacultyViewModel.Article.FacultyId).FacultyName.ToString()
            };

            return model;
        }
    public Article GetArticleById(int id)
    {
      return _dbContext.Articles.SingleOrDefault(a => a.Id == id);
    }

    public bool EditArticle(Article article)
    {
      var articleInDb = GetArticleById(article.Id);
      if (articleInDb == null) return false;
      articleInDb.Title = article.Title;
      articleInDb.Author = article.Author;
      articleInDb.Content = article.Content;
      articleInDb.FacultyId = article.FacultyId;
      articleInDb.Faculty = article.Faculty;
      articleInDb.Image = article.Image;
      articleInDb.Status = article.Status;

      _dbContext.Articles.Update(articleInDb);
      _dbContext.SaveChanges();
      return true;
    }
    }
}