using Megatron.Services;
using Megatron.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Megatron.Extensions;
using Megatron.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Megatron.Models;

namespace Megatron.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFacultyRepository _facultyRepository;
        private readonly IDocumentRepository _documentRepository;

        public StudentController(IStudentRepository studentRepository,
            IUserRepository userRepository, IFacultyRepository facultyRepository, IDocumentRepository documentRepository)
        {
            _studentRepository = studentRepository;
            _userRepository = userRepository;
            _facultyRepository = facultyRepository;
            _documentRepository = documentRepository;
        }

        [Authorize(Roles = (SystemRoles.Administrator + "," + SystemRoles.Student))]
        public IActionResult Index()
        {
            return View();
        }

        //GET
        [Authorize(Roles = (SystemRoles.Administrator + "," + SystemRoles.Student))]
        public IActionResult GetPersonalArticles()
        {
            var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userFullname = _userRepository.GetUserFullName(currentUser).Result;

            var listArticles = _studentRepository.GetPersonalArticles(userFullname);
            return new JsonResult(listArticles);
        }

        //GET
        [Authorize(Roles = (SystemRoles.Administrator + "," + SystemRoles.Student))]
        public IActionResult SubmitArticle()
        {
            var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userFullname = _userRepository.GetUserFullName(currentUser).Result;
            var articleFacultyViewModel = _studentRepository.ArticleFacultyViewModel(userFullname);
            return View(articleFacultyViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = (SystemRoles.Administrator + "," + SystemRoles.Student))]
        public IActionResult SubmitArticle(ArticleFacultyViewModel articleFacultyViewModel)
        {
            var files = Request.Form.Files;
            var articleSubmit = _studentRepository.SubmitArticle(articleFacultyViewModel);
            var result = true;
            
            if (files.Any())
            {
                result = _documentRepository.UploadDocument(files, articleFacultyViewModel.Article.Id);
            }
            
            if (articleSubmit == null || result)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewData["Message"] = articleSubmit.StatusMessage;
            return View(articleSubmit);
        }
        
        [Authorize(Roles = (SystemRoles.Administrator + "," + SystemRoles.Student))]
        public ActionResult EditArticle(int id)
        {
            var articleInDb = _studentRepository.GetArticleById(id);
            if (articleInDb == null)
            {
                return NotFound();
            }

            var articleVM = new ArticleFacultyViewModel
            {
                Article = articleInDb,
                Faculties = _facultyRepository.GetFaculties()
            };
            return View(articleVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = (SystemRoles.Administrator + "," + SystemRoles.Student))]
        public ActionResult EditArticle(Article article)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!_studentRepository.EditArticle(article))
            {
                throw new ArgumentException("...");
            }

            return RedirectToAction("Index");
        }
    }
}