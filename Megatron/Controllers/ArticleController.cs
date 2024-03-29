using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using GroupDocs.Viewer;
using GroupDocs.Viewer.Options;
using Megatron.Services;
using Megatron.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Megatron.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly ILogger<ArticleController> _logger;

        public ArticleController(IArticleRepository articleRepository, ICommentRepository commentRepository,
            IUserRepository userRepository, IDocumentRepository documentRepository,
            ISemesterRepository semesterRepository, ILogger<ArticleController> logger)
        {
            _articleRepository = articleRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _documentRepository = documentRepository;
            _semesterRepository = semesterRepository;
            _logger = logger;
        }

        // GET
        [Authorize(Roles = SystemRoles.Administrator + "," + SystemRoles.MarketingCoordinator + "," +
                           SystemRoles.MarketingManager)]
        public IActionResult Index()
        {
            if (!User.IsInRole(SystemRoles.MarketingCoordinator))
            {
                _logger.LogInformation("Display list of faculties");
                return View(_articleRepository.GetFaculties());
            }

            var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var facultyOfMc = _articleRepository.GetFacultiesForMC(currentUser);
            _logger.LogInformation("Display list of faculties for Marketing Coordinator!");
            return View(facultyOfMc);
        }

        [Authorize(Roles = SystemRoles.Administrator + "," + SystemRoles.MarketingCoordinator + "," +
                           SystemRoles.MarketingManager)]
        public IActionResult ListArticles(int id)
        {
            var listArticles = _articleRepository.GetListArticlesByFaculty(id);
            _logger.LogInformation("Display list articles!");
            return View(listArticles);
        }

        [Authorize(Roles = SystemRoles.Administrator + "," + SystemRoles.MarketingManager)]
        public IActionResult ListArticlesApproved()
        {
            var listSemesterAfterFinalDate = _semesterRepository.GetListSemestersAfterFinalDate();
            _logger.LogInformation("Display List articles approved!");
            return View(listSemesterAfterFinalDate);
        }

        public IActionResult GetArticlesApproved(int id)
        {
            var listArticlesApproved = _articleRepository.GetListArticlesApprovedByFaculty(id);
            _logger.LogInformation("Json List articles approved!");
            return new JsonResult(listArticlesApproved);
        }

        [Authorize(Roles = SystemRoles.Administrator + "," + SystemRoles.MarketingCoordinator + "," +
                           SystemRoles.MarketingManager + "," + SystemRoles.Student)]
        public IActionResult Details(int id)
        {
            var article = _articleRepository.GetArticleDetail(id);
            // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
            if (article.Article != null)
                _logger.LogInformation($"Open Article: {article.Article.Title}");
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
            if (statusMessage == null)
            {
                _logger.LogWarning("Message is empty when update article status!");
                return Json(new {success = false, message = "Please add a message!"});
            }

            if (_articleRepository.UpdateArticleStatus(articleId, status, statusMessage, userFullname))
            {
                _logger.LogInformation("Article has been approved!");
                return Json(new {success = true, message = "Article has been approved"});
            }

            _logger.LogInformation("There are some error when approve!");
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
            if (!sendMessage) return RedirectToAction(nameof(Index));
            // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
            _logger.LogInformation($"{userFullname} has been sent a message: {message}!");
            return Json(new {success = true, userInput = userFullname, messageInput = message});
        }

        [HttpPost]
        public FileResult DownloadArticle(int id, int semesterId)
        {
            _documentRepository.ConvertHtmlToDoc(id, semesterId);
            _documentRepository.DownloadZip(id, semesterId);
            var finalResult = _documentRepository.FinalResult(id, semesterId);
            var zipFileName = _documentRepository.FileZipName(id, semesterId);
            _logger.LogInformation("Articles downloaded!");
            return File(finalResult, "application/zip", zipFileName);
        }

        public FileResult DownloadDocument(string fileName)
        {
            var fileBytes = _documentRepository.GetDocumentByName(fileName);
            return File(fileBytes, "application/force-download", fileName);
        }
        
        [HttpPost]
        public IActionResult ViewDocument(string fileName)
        {
            var filePath = _documentRepository.GetFilePath(fileName);
            var outputFilePath = _documentRepository.GetOutPutDirectory();
            var viewer = new Viewer(filePath);
            var options = new PdfViewOptions(outputFilePath);
            viewer.View(options);

            var fileStream = new FileStream(outputFilePath, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fileStream, "application/pdf");

        }
    }
}