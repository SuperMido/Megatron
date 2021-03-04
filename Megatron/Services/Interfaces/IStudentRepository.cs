using System.Collections.Generic;
using Megatron.Models;
using Megatron.ViewModels;

namespace Megatron.Services
{
    public interface IStudentRepository
    {
        ArticleFacultyViewModel ArticleFacultyViewModel(string userName);

        ArticleFacultyViewModel SubmitArticle(ArticleFacultyViewModel articleFacultyViewModel);

        IEnumerable<Article> GetPersonalArticles(string userName);
        bool EditArticle(Article article);
        Article GetArticleById(int Id);
        IEnumerable<ApplicationUser> GetUserInFacultyByFacultyId(int id);
    }
}