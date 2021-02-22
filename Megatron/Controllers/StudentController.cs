using Megatron.Services;
using Megatron.ViewModels;
using Megatron.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using Megatron.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Megatron.Utility;

namespace Megatron.Controllers
{
	[Authorize(Roles = (SystemRoles.Administrator))]
	public class StudentController : Controller
	{
		private readonly IStudentRepository _studentRepository;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IFacultyRepository _facultyRepository;

		public StudentController(IStudentRepository studentRepository, IWebHostEnvironment webHostEnvironment, IFacultyRepository facultyRepository)
		{
			_studentRepository = studentRepository;
			_webHostEnvironment = webHostEnvironment;
			_facultyRepository = facultyRepository;
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

		[HttpGet]
		public ActionResult EditArticle(int id)
		{
			var articleInDb = _studentRepository.GetArticleById(id);
			if(articleInDb == null)
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
		public ActionResult EditArticle(Article article)
		{
			if (!ModelState.IsValid)
			{
				return View(article);
			}
			if (!_studentRepository.EditArticle(article))
			{
				throw new ArgumentException("Error");
			}

			return RedirectToAction("Index");
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
			var saveDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");
			var savePath = Path.Combine(saveDir, fileName);
			var previewPath = "/images/" + fileName;

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
			catch (Exception )
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