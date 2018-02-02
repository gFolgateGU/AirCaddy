using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirCaddy.Domain.Services.GolfCourses;

namespace AirCaddy.Controllers
{
    public class GolfCoursesController : Controller
    {
        private readonly IYelpGolfCourseReviewService _yelpGolfCourseReviewService;

        public GolfCoursesController(IYelpGolfCourseReviewService yelpGolfCourseReviewService)
        {
            _yelpGolfCourseReviewService = yelpGolfCourseReviewService;
        }
        // GET: GolfCourses
        public ActionResult Index()
        {
            return View();
        }
    }
}