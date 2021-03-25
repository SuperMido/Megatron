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
        private readonly IDocumentRepository _documentRepository;
        private readonly ISemesterRepository _semesterRepository;

        public ArticleController(IArticleRepository articleRepository, ICommentRepository commentRepository,
            IUserRepository userRepository, IDocumentRepository documentRepository,
            ISemesterRepository semesterRepository)
        {
            _articleRepository = articleRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _documentRepository = documentRepository;
            _semesterRepository = semesterRepository;
        }

        // GET
        [Authorize(Roles = SystemRoles.Administrator + "," + SystemRoles.MarketingCoordinator + "," +
                           SystemRoles.MarketingManager)]
        public IActionResult Index()
        {
            if (User.IsInRole(SystemRoles.MarketingCoordinator))
            {
                var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var facultyOfMc = _articleRepository.GetFacultiesForMC(currentUser);
                return View(facultyOfMc);
            }

            return View(_articleRepository.GetFaculties());
        }

        [Authorize(Roles = SystemRoles.Administrator + "," + SystemRoles.MarketingCoordinator + "," +
                           SystemRoles.MarketingManager)]
        public IActionResult ListArticles(int id)
        {
            var listArticles = _articleRepository.GetListArticlesByFaculty(id);
            return View(listArticles);
        }

        [Authorize(Roles = SystemRoles.Administrator + "," + SystemRoles.MarketingManager)]
        public IActionResult ListArticlesApproved()
        {
            var listSemesterAfterFinalDate = _semesterRepository.GetListSemestersAfterFinalDate();
            return View(listSemesterAfterFinalDate);
        }

        public IActionResult GetArticlesApproved(int id)
        {
            var listArticlesApproved = _articleRepository.GetListArticlesApprovedByFaculty(id);
            return new JsonResult(listArticlesApproved);
        }

        [Authorize(Roles = SystemRoles.Administrator + "," + SystemRoles.MarketingCoordinator + "," +
                           SystemRoles.MarketingManager + "," + SystemRoles.Student)]
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
        [Authorize(Roles = SystemRoles.Administrator + "," + SystemRoles.MarketingCoordinator)]
        public IActionResult UpdateArticleStatus(int articleId, bool status, string statusMessage)
        {
            var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userFullname = _userRepository.GetUserFullName(currentUser).Result;
            if (statusMessage == null) return Json(new {success = false, message = "Please add a message!"});
            if (_articleRepository.UpdateArticleStatus(articleId, status, statusMessage, userFullname))
                return Json(new {success = true, message = "Article has been approved"});

            return Json(new {success = false, message = "There are some error when approve!"});
        }

        [HttpPost]
        [Authorize(Roles =
            SystemRoles.Administrator + "," + SystemRoles.Student + "," + SystemRoles.MarketingCoordinator)]
        public async Task<IActionResult> SendMessage(int articleId, string message)
        {
            if (message == null) return Json(new {success = false, message = "Please comment again!"});
            var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userFullname = await _userRepository.GetUserFullName(currentUser);
            var sendMessage = await _commentRepository.SaveComment(articleId, currentUser, message);
            if (sendMessage) return Json(new {success = true, userInput = userFullname, messageInput = message});

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public FileResult DownloadArticle(int id, int semesterId)
        {
            _documentRepository.ConvertHtmlToDoc(id, semesterId);
            _documentRepository.DownloadZip(id, semesterId);
            var finalResult = _documentRepository.FinalResult(id, semesterId);
            var zipFileName = _documentRepository.FileZipName(id, semesterId);
            return File(finalResult, "application/zip", zipFileName);
        }

        public FileResult DownloadDocument(string fileName)
        {
            var fileBytes = _documentRepository.GetDocumentByName(fileName);
            return File(fileBytes, "application/force-download", fileName);
        }
    }
}