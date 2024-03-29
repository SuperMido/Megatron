﻿using System;
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
using Microsoft.Extensions.Logging;

namespace Megatron.Controllers
{
    public class StudentController : Controller
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IEmailSender _emailSender;
        private readonly IFacultyRepository _facultyRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<StudentController> _logger;

        public StudentController(IStudentRepository studentRepository, ISemesterRepository semesterRepository,
            IUserRepository userRepository, IFacultyRepository facultyRepository, IEmailSender emailSender,
            IDocumentRepository documentRepository, IWebHostEnvironment webHostEnvironment,
            ILogger<StudentController> logger)
        {
            _studentRepository = studentRepository;
            _userRepository = userRepository;
            _facultyRepository = facultyRepository;
            _semesterRepository = semesterRepository;
            _emailSender = emailSender;
            _documentRepository = documentRepository;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
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
            var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userFullname = _userRepository.GetUserFullName(currentUser).Result;
            var articleFacultyViewModelError = _studentRepository.ArticleFacultyViewModel(userFullname);
            
            if (!files.Any() && articleFacultyViewModel.Article.Content == null)
            {
                ViewData["Message"] = "Error: Content or file must be input!";
                return View(articleFacultyViewModelError);
            }
            
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

                foreach (var applicationUser in listUserInSelectedFaculty)
                    await _emailSender.SendEmailAsync(applicationUser.Email,
                        "[Megatron] " + userFullname + " already submit article: " +
                        articleFacultyViewModel.Article.Title,
                        emailTemplate);
                _logger.LogInformation("User has been submitted an new article!");
                return RedirectToAction(nameof(Index));
            }

            ViewData["Message"] = articleSubmit.StatusMessage;
            return View(articleFacultyViewModelError);
        }

        [Authorize(Roles = SystemRoles.Administrator + "," + SystemRoles.Student)]
        public ActionResult EditArticle(int id)
        {
            var articleInDb = _studentRepository.GetArticleById(id);
            if (articleInDb == null) return NotFound();
            var semesterActiveForEdit = _semesterRepository.GetSemesterForArticle(id);
            var articleVm = new ArticleFacultyViewModel()
            {
                Article = articleInDb,
                Semester = semesterActiveForEdit,
                Faculties = _facultyRepository.GetFaculties()
            };
            return View(articleVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SystemRoles.Administrator + "," + SystemRoles.Student)]
        public ActionResult EditArticle(ArticleFacultyViewModel articleFacultyViewModel)
        {
            var files = Request.Form.Files;
            if (files.Any()) _documentRepository.UploadDocument(files, articleFacultyViewModel.Article.Id);
            var articleToEdit = _studentRepository.EditArticle(articleFacultyViewModel);

            if (articleToEdit == null)
            {
                _logger.LogInformation("Article has been updated!");
                return RedirectToAction(nameof(Index));
            }

            ViewData["Message"] = articleToEdit.StatusMessage;
            return View(articleToEdit);
        }

        public IActionResult DeleteDocument(string name)
        {
            if (name == null)
            {
                return NotFound();
            }

            _documentRepository.DeleteDocumentByName(name);
            _logger.LogInformation($"Remove document {name}!");
            return RedirectToAction(nameof(Index));
        }
    }
}