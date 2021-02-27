using Megatron.Models;
using Megatron.Services;
using Megatron.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Megatron.Controllers
{
    [Authorize(Roles = (SystemRoles.Administrator))]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
      if(!ModelState.IsValid)
			{
        return View();
			}
      if(!_userRepository.EditAccount(applicationUsers))
			{
        throw new ArgumentException("Error");
			}
      return RedirectToAction("Index");
		}
    }
}