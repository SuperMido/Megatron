using Megatron.Models;
using Megatron.ViewModels;
using System.Collections.Generic;

namespace Megatron.Services
{
    public interface IStudentRepository
    {
        ArticleFacultyViewModel ArticleFacultyViewModel(string userName);

        ArticleFacultyViewModel SubmitArticle(ArticleFacultyViewModel articleFacultyViewModel);

        IEnumerable<Article> GetTheirArticles(string userName);
    }
}