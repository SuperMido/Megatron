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
        private readonly IReportRepository _reportRepository;

        public HomeController(ILogger<HomeController> logger, IReportRepository reportRepository)
        {
            _logger = logger;
            _reportRepository = reportRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = SystemRoles.Administrator + "," + SystemRoles.MarketingCoordinator + "," +
                           SystemRoles.MarketingManager + "," + SystemRoles.Student + "," + SystemRoles.Guest)]
        public IActionResult Welcome()
        {
            var report = _reportRepository.HomeViewModel();

            return View(report);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}