using Megatron.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Megatron.Utility;
using Megatron.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Megatron.Controllers
{
    [Authorize(Roles = (SystemRoles.Administrator + "," + SystemRoles.MarketingManager))]
    public class SemesterController : Controller
    {
        private readonly ISemesterRepository _semesterRepository;

        public SemesterController(ISemesterRepository semesterRepository)
        {
            _semesterRepository = semesterRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetSemesters()
        {
            var semester = _semesterRepository.GetAllSemesters();
            return new JsonResult(semester);
        }

        [Authorize(Roles = (SystemRoles.Administrator))]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = (SystemRoles.Administrator))]
        public ActionResult Create(SemesterViewModel semesterViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var semester = _semesterRepository.CreateSemester(semesterViewModel);

            if (semester == null)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewData["Message"] = semester.StatusMessage;
            return View(semesterViewModel);
        }

        [Authorize(Roles = (SystemRoles.Administrator))]
        public ActionResult Delete(int id)
        {
            if (!_semesterRepository.DeleteSemester(id))
            {
                throw new ArgumentException("Error when delete Semester");
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = (SystemRoles.Administrator))]
        public ActionResult Edit(int id)
        {
            var semesterViewModelInDb = _semesterRepository.GetSemesterViewModel(id);
            if (semesterViewModelInDb == null)
            {
                throw new ArgumentException("Cannot find Semester");
            }

            return View(semesterViewModelInDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = (SystemRoles.Administrator))]
        public ActionResult Edit(SemesterViewModel semesterViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var semester = _semesterRepository.EditSemester(semesterViewModel);

            if (semester == null)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewData["Message"] = semester.StatusMessage;
            return View(semesterViewModel);
        }
    }
}