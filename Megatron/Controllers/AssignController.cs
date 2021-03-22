using Megatron.Services;
using Megatron.Utility;
using Megatron.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;

namespace Megatron.Controllers
{
    [Authorize(Roles = (SystemRoles.Administrator + "," + SystemRoles.MarketingManager))]
    public class AssignController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AssignController> _logger;

        public AssignController(IUserRepository userRepository, ILogger<AssignController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        // GET
        public IActionResult Index()
        {
            var usersInFaculty = _userRepository.GetUserInFaculty();
            _logger.LogInformation("Display list user in each faculty!");
            return View(usersInFaculty);
        }

        public IActionResult AssignMc()
        {
            var mcInFaculty = _userRepository.GetMcInFaculty();
            return View(mcInFaculty);
        }

        [HttpPost]
        public IActionResult AssignMc(UserFacultyViewModel model)
        {
            if (_userRepository.AssignMcToFaculty(model))
            {
                _logger.LogInformation("User has been assign into faculty!");
                return RedirectToAction(nameof(Index));
            }

            _logger.LogWarning("Error when assign user into faculty");
            ViewData["Message"] = "Error: Marketing Manager already exists in Faculty";
            return View(_userRepository.GetMcInFaculty());
        }

        public IActionResult AssignGuest()
        {
            var guestInFaculty = _userRepository.GetGuestInFaculty();
            return View(guestInFaculty);
        }

        [HttpPost]
        public IActionResult AssignGuest(UserFacultyViewModel model)
        {
            if (_userRepository.AssignGuestToFaculty(model))
            {
                _logger.LogInformation("User has been assign into faculty!");
                return RedirectToAction(nameof(Index));
            }

            _logger.LogWarning("Error when assign user into faculty");
            ViewData["Message"] = "Error: Guest already exists in Faculty";
            return View(_userRepository.GetGuestInFaculty());
        }

        public IActionResult GetUserFaculty()
        {
            var usersInFaculty = _userRepository.GetUserInFaculty();
            return new JsonResult(usersInFaculty);
        }

        public ActionResult DeleteUserInFaculty(int id)
        {
            if (!_userRepository.DeleteUserInFaculty(id))
            {
                _logger.LogWarning("Error when assign delete user in faculty");
                throw new ArgumentException("Error");
            }

            _logger.LogInformation("User has been removed from faculty!");
            return RedirectToAction("Index");
        }
    }
}