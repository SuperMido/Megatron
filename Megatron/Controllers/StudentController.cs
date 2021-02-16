using Megatron.Models;
using Megatron.Services;
using Megatron.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Megatron.Controllers
{
	public class StudentController : Controller
	{
		private readonly IStudentRepository _studentRepository;

		public StudentController(IStudentRepository studentRepository)
		{
			_studentRepository = studentRepository;
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
		public IActionResult EditArticle()
		{
			ArticleFacultyViewModel articleFacultyViewModel = _studentRepository.ArticleFacultyViewModel();
			return View(articleFacultyViewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult EditArticle(ArticleFacultyViewModel articleFacultyViewModel)
		{
			var articleEdit = _studentRepository.EditArticle(articleFacultyViewModel);
			if(articleEdit == null)
			{
				return RedirectToAction(nameof(Index));
			}
			ViewData["Message"] = articleEdit.StatusMessage;
			return View(articleEdit);
		}
	}
}