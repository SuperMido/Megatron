using Megatron.Services;
using Megatron.Utility;
using Megatron.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Megatron.Controllers
{
    [Authorize(Roles = (SystemRoles.Administrator + "," + SystemRoles.MarketingManager))]
    public class AssignController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AssignController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET
        public IActionResult Index()
        {
            var usersInFaculty = _userRepository.GetUserInFaculty();
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
                return RedirectToAction(nameof(Index));
            }
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
                return RedirectToAction(nameof(Index));
            }
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
                throw new ArgumentException("Error");
            }
            return RedirectToAction("Index");
        }
    }
}