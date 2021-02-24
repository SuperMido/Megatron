using System.Security.Claims;
using System.Threading.Tasks;
using Megatron.Services;
using Megatron.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Megatron.Controllers
{
    public class ArticleController : Controller
    {

        private readonly IArticleRepository _articleRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;

        public ArticleController(IArticleRepository articleRepository, ICommentRepository commentRepository, IUserRepository userRepository)
        {
            _articleRepository = articleRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
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
        public IActionResult Details(int id)
        {
            var article = _articleRepository.GetArticleDetail(id);
            return View(article);
        }
        
        public IActionResult GetListArticles(int id)
        {
            var listArticles = _articleRepository.GetListArticlesByFaculty(id);
            return new JsonResult(listArticles);
        }
        
        [HttpPost]
        [Authorize(Roles = (SystemRoles.Administrator + "," + SystemRoles.MarketingCoordinator))]
        public IActionResult UpdateArticleStatus(int articleId, bool status, string statusMessage)
        {
            if (statusMessage == null)
            {
                return Json(new {success = false, message = "Please add a message!"});
            }
            if (_articleRepository.UpdateArticleStatus(articleId, status, statusMessage))
            {
                return Json(new {success = true, message = "Article has been approved"});
            }

            return Json(new {success = false, message = "There are some error when approve!"});
        }
        
        [HttpPost]
        [Authorize(Roles = (SystemRoles.Administrator + "," + SystemRoles.Student + "," + SystemRoles.MarketingCoordinator))]
        public async Task<IActionResult> SendMessage(int articleId, string message)
        {
            if (message == null)
            {
                return Json(new { success = false, message = "Please comment again!" });
            }
            var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userFullname = await _userRepository.GetUserFullName(currentUser);
            var sendMessage = await _commentRepository.SaveComment(articleId, currentUser, message);
            if (sendMessage)
            {
                return Json(new { success = true, userInput = userFullname, messageInput = message });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}