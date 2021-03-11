using Megatron.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Megatron.Services;
using Megatron.Utility;
using Microsoft.AspNetCore.Authorization;

namespace Megatron.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISemesterRepository _semesterRepository;

        public HomeController(ILogger<HomeController> logger, ISemesterRepository semesterRepository)
        {
            _logger = logger;
            _semesterRepository = semesterRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Welcome()
        {
            if(User.IsInRole(SystemRoles.Student))
            {
                var currentDate = new DateTime();
                var semesterActive = _semesterRepository.GetActiveSemester();
                if (semesterActive != null)
                {
                    if (currentDate < semesterActive.SemesterClosureDate &&
                        currentDate < semesterActive.SemesterEndDate)
                    {
                        ViewBag.Results = "Submit & Edit";
                    }

                    if (currentDate > semesterActive.SemesterClosureDate &&
                        currentDate < semesterActive.SemesterEndDate)
                    {
                        ViewBag.Results = "Edit";
                    }

                    if (currentDate > semesterActive.SemesterClosureDate &&
                        currentDate > semesterActive.SemesterEndDate)
                    {
                        ViewBag.Results = "Cannot";
                    }
                    return View(semesterActive);
                } 
                return View();
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
