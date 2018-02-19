using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AirCaddy.Domain.Services;
using AirCaddy.Domain.Services.GolfCourses;

namespace AirCaddy.Controllers
{
    public class GolfCoursesController : Controller
    {
        private readonly IYelpGolfCourseReviewService _yelpGolfCourseReviewservice;
        private readonly IGolfCourseService _golfCourseService;
        private readonly ISessionMapperService _sessionMapperService;

        public GolfCoursesController(IYelpGolfCourseReviewService yelpGolfCourseReviewservice, IGolfCourseService golfCourseService,
            ISessionMapperService sessionMapperService)
        {
            _yelpGolfCourseReviewservice = yelpGolfCourseReviewservice;
            _golfCourseService = golfCourseService;
            _sessionMapperService = sessionMapperService;
        }
        // GET: GolfCourses
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles=("User, GolfCourseOwner, Admin"))]
        public async Task<ActionResult> MyCourses()
        {
            var userId = _sessionMapperService.MapUserIdFromSessionUsername(Session["Username"].ToString());
            var myCoursesVm = await _golfCourseService.GetMyCourses(userId);
            return View(myCoursesVm);
        }

        [HttpGet]
        [Authorize(Roles = ("User, GolfCourseOwner, Admin"))]
        public async Task<ActionResult> ManageMyCourse(string courseId)
        {
            var userId = _sessionMapperService.MapUserIdFromSessionUsername(Session["Username"].ToString());
            var myCourse = await _golfCourseService.RequestCourseOwnedByUser(courseId, userId);
            if (myCourse == null)
            {
                return RedirectToAction("MyCourses");
            }
            else
            {
                return View();
            }
        }

        // GET Explore
        [HttpGet]
        public async Task<ActionResult> Explore(int golfCourseId)
        {
            var vm = await _golfCourseService.GetCourseOverviewViewModel(golfCourseId);
            return View(vm);
        }
    }
}