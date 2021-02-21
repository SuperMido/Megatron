using Megatron.Services;
using Megatron.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    }
}