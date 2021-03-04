﻿using Megatron.Services;
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
            ViewBag.CountContributorsOfFaculty = _chartRepository.CountContributorsOfFaculty();
            return View();
        }
        [HttpPost]
        public ActionResult Index(int yearSelected)
        {
        
            ViewBag.FacultyList = _chartRepository.GetFacultyList();
            ViewBag.CountArticleOfFaculty = _chartRepository.CountArticleOfFaculty();
            ViewBag.PercentContributionsOfFaculty = _chartRepository.PercentContributionsOfFaculty(yearSelected);
            ViewBag.CountContributorsOfFaculty = _chartRepository.CountContributorsOfFaculty();

            ViewBag.Year = yearSelected;
            return View();
        }

    }
}