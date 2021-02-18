using Megatron.Services;
using Microsoft.AspNetCore.Mvc;

namespace Megatron.Controllers
{
    public class ArticleController : Controller
    {

        private readonly IArticleRepository _articleRepository;

        public ArticleController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        // GET
        public IActionResult Index()
        {
            return View(_articleRepository.GetFaculties());
        }

        public IActionResult ListArticles(int id)
        {
            var listArticles = _articleRepository.GetListArticlesByFaculty(id);
            return View(listArticles);
        }
        
        public IActionResult GetListArticles(int id)
        {
            var listArticles = _articleRepository.GetListArticlesByFaculty(id);
            return new JsonResult(listArticles);
        }
    }
}