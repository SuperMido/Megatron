using Megatron.Services;
using Megatron.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using Megatron.Extensions;
using Microsoft.AspNetCore.Hosting;

namespace Megatron.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StudentController(IStudentRepository studentRepository, IWebHostEnvironment webHostEnvironment)
        {
            _studentRepository = studentRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        //GET
        public IActionResult SubmitArticle()
        {
            ArticleFacultyViewModel articleFacultyViewModel = _studentRepository.ArticleFacultyViewModel();
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