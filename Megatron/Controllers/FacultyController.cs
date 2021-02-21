using Megatron.Data;
using Megatron.Models;
using Megatron.Services;
using Megatron.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Megatron.Controllers
{
    public class FacultyController : Controller
    {
        private readonly IFacultyRepository _IFaculty;
        public FacultyController(IFacultyRepository IFaculty)
        {
            _IFaculty = IFaculty;
        }

        public IActionResult Index()
        {
            return View(_IFaculty.GetFaculties().ToList());
        }
        public IActionResult GetAllFaculty()
        {
            var allFaculty = _IFaculty.GetFaculties();
            return new JsonResult(allFaculty);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                _IFaculty.createFaculty(faculty);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var fal = _IFaculty.GetFacultyById(id);
            return View(fal);
        }

        public IActionResult Edit(int id)
        {
            var fal = _IFaculty.GetFacultyById(id);
            return View(fal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                _IFaculty.editFaculty(faculty);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            _IFaculty.deleteFaculty(id);
            return RedirectToAction("Index");
        }
    }
}