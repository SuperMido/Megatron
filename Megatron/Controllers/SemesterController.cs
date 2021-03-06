using Megatron.Models;
using Megatron.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Megatron.Utility;
using Microsoft.AspNetCore.Authorization;

namespace Megatron.Controllers
{
    [Authorize(Roles = (SystemRoles.Administrator))]
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
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Semester semester)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!_semesterRepository.CreateSemester(semester))
            {
                throw new ArgumentException("Cannot create Semester");
            }

            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            if (!_semesterRepository.DeleteSemester(id))
            {
                throw new ArgumentException("Error when delete Semester");
            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            var semesterInDb = _semesterRepository.GetSemesterById(id);
            if (semesterInDb == null)
            {
                throw new ArgumentException("Cannot find Semester");
            }
            return View(semesterInDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Semester semester)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!_semesterRepository.EditSemester(semester))
            {
                throw new ArgumentException("Cannot edit this Semester");
            }

            return RedirectToAction("Index");
        }
    }
}
