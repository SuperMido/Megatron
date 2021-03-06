using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Megatron.Models;
using Megatron.Services;
using Megatron.Utility;
using Megatron.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Megatron.Controllers
{
    public class StudentController : Controller
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IEmailSender _emailSender;
        private readonly IFacultyRepository _facultyRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StudentController(IStudentRepository studentRepository,
            IUserRepository userRepository, IFacultyRepository facultyRepository, IEmailSender emailSender,
            IDocumentRepository documentRepository, IWebHostEnvironment webHostEnvironment)
        {
            _studentRepository = studentRepository;
            _userRepository = userRepository;
            _facultyRepository = facultyRepository;
            _emailSender = emailSender;
            _documentRepository = documentRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize(Roles = SystemRoles.Administrator + "," + SystemRoles.Student)]
        public IActionResult Index()
        {
            return View();
        }

        //GET
        [Authorize(Roles = SystemRoles.Administrator + "," + SystemRoles.Student)]
        public IActionResult GetPersonalArticles()
        {
            var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userFullname = _userRepository.GetUserFullName(currentUser).Result;

            var listArticles = _studentRepository.GetPersonalArticles(userFullname);
            return new JsonResult(listArticles);
        }

        //GET
        [Authorize(Roles = SystemRoles.Administrator + "," + SystemRoles.Student)]
        public IActionResult SubmitArticle()
        {
            var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userFullname = _userRepository.GetUserFullName(currentUser).Result;
            var articleFacultyViewModel = _studentRepository.ArticleFacultyViewModel(userFullname);
            return View(articleFacultyViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SystemRoles.Administrator + "," + SystemRoles.Student)]
        public async Task<IActionResult> SubmitArticle(ArticleFacultyViewModel articleFacultyViewModel)
        {
            var files = Request.Form.Files;
            var articleSubmit = _studentRepository.SubmitArticle(articleFacultyViewModel);
            var templateFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "EmailTemplate");
            var emailSubmitTemplatePath = Path.Combine(templateFolderPath, "ArticleSubmit.html");
            string emailTemplate;

            using (var sourceReader = System.IO.File.OpenText(emailSubmitTemplatePath))
            {
                emailTemplate = await sourceReader.ReadToEndAsync();
            }

            if (articleSubmit == null)
            {
                if (files.Any()) _documentRepository.UploadDocument(files, articleFacultyViewModel.Article.Id);
                var listUserInSelectedFaculty =
                    _studentRepository.GetUserInFacultyByFacultyId(articleFacultyViewModel.Article.FacultyId);

                var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userFullname = _userRepository.GetUserFullName(currentUser).Result;

                foreach (var applicationUser in listUserInSelectedFaculty)
                    await _emailSender.SendEmailAsync(applicationUser.Email,
                        "[Megatron] " + userFullname + " already submit article: " +
                        articleFacultyViewModel.Article.Title,
                        emailTemplate);
                return RedirectToAction(nameof(Index));
            }

            ViewData["Message"] = articleSubmit.StatusMessage;
            return View(articleSubmit);
        }

        [Authorize(Roles = SystemRoles.Administrator + "," + SystemRoles.Student)]
        public ActionResult EditArticle(int id)
        {
            var articleInDb = _studentRepository.GetArticleById(id);
            if (articleInDb == null) return NotFound();

            var articleVM = new ArticleFacultyViewModel
            {
                Article = articleInDb,
                Faculties = _facultyRepository.GetFaculties()
            };
            return View(articleVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SystemRoles.Administrator + "," + SystemRoles.Student)]
        public ActionResult EditArticle(Article article)
        {
            if (!ModelState.IsValid) return View();

            if (!_studentRepository.EditArticle(article)) throw new ArgumentException("...");

            return RedirectToAction("Index");
        }
    }
}