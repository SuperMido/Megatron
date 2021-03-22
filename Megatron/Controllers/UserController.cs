using Megatron.Models;
using Megatron.Services;
using Megatron.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;

namespace Megatron.Controllers
{
    [Authorize(Roles = (SystemRoles.Administrator))]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        // GET
        public IActionResult Index()
        {
            var users = _userRepository.GetAllUsers();
            return View(users);
        }

        public IActionResult GetUsers()
        {
            var users = _userRepository.GetAllUsers();

            return new JsonResult(users);
        }

        public ActionResult DeleteAccount(string id)
        {
            if (!_userRepository.DeleteAccount(id))
            {
                throw new ArgumentException("Error");
            }

            _logger.LogInformation($"Delete user account with id: {id}");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditAccount(string id)
        {
            var accountInDb = _userRepository.GetUserById(id);
            if (accountInDb == null)
            {
                return NotFound();
            }

            return View(accountInDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAccount(ApplicationUser applicationUsers)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!_userRepository.EditAccount(applicationUsers))
            {
                throw new ArgumentException("Error");
            }

            _logger.LogInformation($"User account updated!");
            return RedirectToAction("Index");
        }

        public IActionResult Lock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!_userRepository.LockUser(id))
            {
                return NotFound();
            }

            _logger.LogInformation($"Lock user with id: {id}");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult UnLock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!_userRepository.UnLockUser(id))
            {
                return NotFound();
            }

            _logger.LogInformation($"UnLock user with id: {id}");
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public IActionResult Avatar(string userId)
        {
            var user = _userRepository.GetUserById(userId);
            return Json(user.ImagePath == null ? new {avatar = "DefaultAvatar.jpeg"} : new {avatar = user.ImagePath});
        }
    }
}