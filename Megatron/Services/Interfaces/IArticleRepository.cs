using System.Collections.Generic;
using Megatron.Models;
using Megatron.ViewModels;

namespace Megatron.Services
{
    public interface IArticleRepository
    {
        IEnumerable<Article> GetAllArticle();

        IEnumerable<Article> GetListArticlesByFaculty(int facultyId);
        IEnumerable<Article> GetListArticlesApprovedByFaculty(int facultyId);
        IEnumerable<Article> GetListArticlesApprovedAfterFinalDateByFaculty(int facultyId, int semesterId);

        IEnumerable<Article> GetListArticleWithoutComment();
        IEnumerable<Article> GetListArticleWithoutComment14Days();

        IEnumerable<Faculty> GetFaculties();

        IEnumerable<Faculty> GetFacultiesForMC(string id);

        CommentArticleViewModel GetArticleDetail(int id);

        bool UpdateArticleStatus(int id, bool status, string message, string changeStatusBy);
    }
}