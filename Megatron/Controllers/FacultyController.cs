using Megatron.Models;
using Megatron.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Megatron.Controllers
{
    public class FacultyController : Controller
    {
        private readonly IFacultyRepository _facultyRepository;
        public FacultyController(IFacultyRepository facultyRepository)
        {
            _facultyRepository = facultyRepository;
        }

        public IActionResult Index()
        {
            return View(_facultyRepository.GetFaculties().ToList());
        }
        public IActionResult GetAllFaculty()
        {
            var faculties = _facultyRepository.GetFaculties();
            return new JsonResult(faculties);
        }
        
        public IActionResult Details(int id)
        {
            var facultyById = _facultyRepository.GetFacultyById(id);
            return View(facultyById);
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
                _facultyRepository.CreateFaculty(faculty);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var facultyById = _facultyRepository.GetFacultyById(id);
            return View(facultyById);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                _facultyRepository.EditFaculty(faculty);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            _facultyRepository.DeleteFaculty(id);
            return RedirectToAction("Index");
        }
    }
}