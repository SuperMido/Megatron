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

namespace Megatron.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUserRepository _userRepository;

        public StudentController(IStudentRepository studentRepository, IWebHostEnvironment webHostEnvironment, IUserRepository userRepository)
        {
            _studentRepository = studentRepository;
            _webHostEnvironment = webHostEnvironment;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
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
        
        [HttpPost]
        public ActionResult UploadImage()
        {                  
            var files = Request.Form.Files;
            if (files.Count == 0 || !files[0].IsImage())
            {
                var rError = new
                {
                    uploaded = false,
                    url = string.Empty
                };
                return Json(rError);
            }
            var formFile = files[0];
            var upFileName = formFile.FileName;
            var fileName = Guid.NewGuid() + Path.GetExtension(upFileName);
            var saveDir = Path.Combine(_webHostEnvironment.WebRootPath, "articleImages");
            var savePath = Path.Combine(saveDir, fileName);
            var previewPath = "/articleImages/" + fileName;
 
            var result = true;
            try
            {
                if (!Directory.Exists(saveDir))
                {
                    Directory.CreateDirectory(saveDir);
                }

                using var fs = System.IO.File.Create(savePath);
                formFile.CopyTo(fs);
                fs.Flush();
            }
            catch (Exception)
            {
                result = false;
            }
            var rUpload = new
            {
                uploaded = result,
                url = result ? previewPath : string.Empty
            };
            return Json(rUpload);
        }
    }
}