using System.Collections.Generic;
using Megatron.Models;

namespace Megatron.Services
{
    public interface IArticleRepository
    {
        IEnumerable<Article> GetAllArticle();
        
        IEnumerable<Article> GetListArticlesByFaculty(int facultyId);
        
        IEnumerable<Faculty> GetFaculties();
        
    }
}