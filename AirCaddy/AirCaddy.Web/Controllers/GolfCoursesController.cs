using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AirCaddy.Domain.Services.GolfCourses;

namespace AirCaddy.Controllers
{
    public class GolfCoursesController : Controller
    {
        private readonly IYelpGolfCourseReviewService _yelpGolfCourseReviewservice;
        private readonly IGolfCourseService _golfCourseService;

        public GolfCoursesController(IYelpGolfCourseReviewService yelpGolfCourseReviewservice, IGolfCourseService golfCourseService)
        {
            _yelpGolfCourseReviewservice = yelpGolfCourseReviewservice;
            _golfCourseService = golfCourseService;
        }
        // GET: GolfCourses
        public ActionResult Index()
        {
            return View();
        }

        // GET Explore
        [HttpGet]
        public async Task<ActionResult> Explore(int golfCourseId)
        {
            var vm = await _golfCourseService.GetCourseOverviewViewModel(golfCourseId);
            return View(vm);
        }

        public ActionResult VirtualTour()
        {
            return View();
        }
    }
    
}