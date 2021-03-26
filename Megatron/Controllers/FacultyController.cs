using Megatron.Models;
using Megatron.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Megatron.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Megatron.Controllers
{
    [Authorize(Roles = (SystemRoles.Administrator))]
    public class FacultyController : Controller
    {
        private readonly IFacultyRepository _facultyRepository;
        private readonly ILogger<FacultyController> _logger;

        public FacultyController(IFacultyRepository facultyRepository, ILogger<FacultyController> logger)
        {
            _facultyRepository = facultyRepository;
            _logger = logger;
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

            _logger.LogInformation("Create new faculty!");
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

            _logger.LogInformation("Updated faculty!");
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            _facultyRepository.DeleteFaculty(id);

            _logger.LogInformation($"Delete faculty with id: {id}!");
            return RedirectToAction("Index");
        }
    }
}