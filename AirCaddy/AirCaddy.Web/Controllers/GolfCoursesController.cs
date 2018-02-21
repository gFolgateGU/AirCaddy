using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AirCaddy.Domain.Services;
using AirCaddy.Domain.Services.GolfCourses;
using System.IO;
using System.Net;
using AirCaddy.Domain.ViewModels.GolfCourses;

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
        public async Task<ActionResult> ManageMyCourse(int courseId)
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

        [HttpPost]
        [Authorize(Roles = ("User, GolfCourseOwner, Admin"))]
        public async Task<ActionResult> UploadCourseFootage()
        {
            try
            {
                var content = Request.Files[0];
                if (content != null && content.ContentLength > 0)
                {
                    var stream = content.InputStream;
                    var fileName = Path.GetFileName(Request.Files[0].FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/TempFootageUploads"), fileName);
                    using (var fileStream = System.IO.File.Create(path))
                    {
                        stream.CopyTo(fileStream);
                    }
                    var xy = new UploadCourseViewModel
                    {
                        CourseId = "12",
                        CourseName = "Elk Valley Golf Course",
                        HoleNumber = 17,
                        HoleVideoPath = path
                    };
                    var xyz = new YoutubeGolfService();
                    var x2 = await xyz.UploadCourseFootageAsync(xy);
                    if (x2 == true)
                    {
                        return Json("Hell yeah bae bae!");
                    }
                    System.IO.File.Delete(path);
                }
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }

            return Json("File uploaded successfully");
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