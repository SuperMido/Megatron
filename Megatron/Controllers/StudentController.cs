using Megatron.Services;
using Megatron.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUserRepository _userRepository;
        private readonly IFacultyRepository _facultyRepository;

        public StudentController(IStudentRepository studentRepository, IWebHostEnvironment webHostEnvironment,
            IUserRepository userRepository, IFacultyRepository facultyRepository)
        {
            _studentRepository = studentRepository;
            _webHostEnvironment = webHostEnvironment;
            _userRepository = userRepository;
            _facultyRepository = facultyRepository;
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
            var articleSubmit = _studentRepository.SubmitArticle(articleFacultyViewModel);
            if (articleSubmit == null)
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