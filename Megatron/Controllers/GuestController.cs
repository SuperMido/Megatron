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
    [Authorize(Roles = (SystemRoles.Guest))]
    public class GuestController : Controller
    {
        private readonly IChartRepository _chartRepository;
        public GuestController(IChartRepository chartRepository)
        {
            _chartRepository = chartRepository;
        }
        public IActionResult Index()
        {
            ViewBag.FacultyList = _chartRepository.GetFacultyList();
            ViewBag.CountArticleOfFaculty = _chartRepository.CountArticleOfFaculty();
            ViewBag.ArticleApprovedOfFaculty = _chartRepository.ArticleApprovedOfFaculty();

            ViewBag.CountArticleOfTopContributors = _chartRepository.CountArticleOfTopContributors();
            ViewBag.TopContributors = _chartRepository.GetContributorList();


            return View();
        }
    }
}
