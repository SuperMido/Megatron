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
        private readonly IArticleRepository _articleRepository;
        public GuestController(IChartRepository chartRepository, IArticleRepository articleRepository)
        {
            _chartRepository = chartRepository;
            _articleRepository = articleRepository;
        }
        public IActionResult Index()
        {
            ViewBag.FacultyList = _chartRepository.GetFacultyList();
            ViewBag.CountArticleOfFaculty = _chartRepository.CountArticleOfFaculty();
            ViewBag.CountContributorsOfFaculty = _chartRepository.CountContributorsOfFaculty();
            ViewBag.CountArticle = _chartRepository.GetArticleWithoutComment();
            ViewBag.CountArticle14Days = _chartRepository.GetArticlesWithoutComment14Days();
            return View();
        }
        [HttpPost]
        public ActionResult Index(int yearSelected)
        {
            ViewBag.FacultyList = _chartRepository.GetFacultyList();
            ViewBag.CountArticleOfFaculty = _chartRepository.CountArticleOfFaculty();
            ViewBag.CountContributorsOfFaculty = _chartRepository.CountContributorsOfFaculty();
            ViewBag.CountArticle = _chartRepository.GetArticleWithoutComment();
            ViewBag.CountArticle14Days = _chartRepository.GetArticlesWithoutComment14Days();
            ViewBag.PercentContributionsOfFaculty = _chartRepository.PercentContributionsOfFaculty(yearSelected);

            ViewBag.Year = yearSelected;
            return View();
        }

        public IActionResult ListArticlesWithoutComment()
        {
            return View();
        }

        public IActionResult ListArticlesWithoutCommentAfter14Days()
        {
            return View();
        }
        public IActionResult GetArticlesWithoutComment()
        {
            var listArticlesWithoutComment = _articleRepository.GetListArticleWithoutComment();
            return new JsonResult(listArticlesWithoutComment);
        }

        public IActionResult GetArticlesWithoutCommentAfter14Days()
        {
            var listArticlesWithouCommentIn14Days = _articleRepository.GetListArticleWithoutComment14Days();
            return new JsonResult(listArticlesWithouCommentIn14Days);
        }

    }
}
