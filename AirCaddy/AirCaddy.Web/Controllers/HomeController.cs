using AirCaddy.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AirCaddy.Domain.Services.GolfCourses;
using AirCaddy.Domain.ViewModels.GolfCourses;
using Newtonsoft.Json.Linq;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AirCaddy.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGolfCourseService _golfCourseService;

        public HomeController(IGolfCourseService golfCourseService)
        {
            _golfCourseService = golfCourseService;
        }

        public async Task<ActionResult> Index()
        {
            var golfCourseViewModel = await _golfCourseService.GetExistingGolfCoursesViewModelAsync();
            return View(golfCourseViewModel);
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}