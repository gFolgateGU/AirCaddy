using AirCaddy.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AirCaddy.Domain.Services.GolfCourses;

using Newtonsoft.Json.Linq;

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

        [Authorize(Roles="User")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public async Task<ActionResult> Contact()
        {
            //ViewBag.Message = "Your contact page.";
            var obj = new YelpGolfCourseReviewservice("yqCRRvNvxvLQi1f_EBTlaHXC7LURwVTt80PXTUxabfYPxvmsfQJXw6lFxyizBwCdaYsFxTkiy9fPGzdv_2C2Li6MfCAv1LFBL-HwrZTQjR1KUwZu1_GEgwO6LvUFWnYx");
            var x = await obj.GetGolfCourseReviewData("elk-valley-golf-course-girard");
            var y = x;

            return View();
        }
    }
}