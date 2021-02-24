using System.Collections.Generic;
using Megatron.Models;
using Megatron.ViewModels;

namespace Megatron.Services
{
    public interface IArticleRepository
    {
        IEnumerable<Article> GetAllArticle();
        
        IEnumerable<Article> GetListArticlesByFaculty(int facultyId);
        
        IEnumerable<Faculty> GetFaculties();

        CommentArticleViewModel GetArticleDetail(int id);

        bool UpdateArticleStatus(int id, bool status, string message);
    }
}