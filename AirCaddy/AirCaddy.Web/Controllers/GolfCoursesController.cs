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
    public class GolfCoursesController : BaseController
    {
        private readonly IYelpGolfCourseReviewService _yelpGolfCourseReviewservice;
        private readonly IGolfCourseService _golfCourseService;
        private readonly ISessionMapperService _sessionMapperService;
        private readonly IYoutubeGolfService _youtubeGolfService;

        public GolfCoursesController(IYelpGolfCourseReviewService yelpGolfCourseReviewservice, IGolfCourseService golfCourseService,
            ISessionMapperService sessionMapperService, IYoutubeGolfService youtubeGolfService)
        {
            _yelpGolfCourseReviewservice = yelpGolfCourseReviewservice;
            _golfCourseService = golfCourseService;
            _sessionMapperService = sessionMapperService;
            _youtubeGolfService = youtubeGolfService;
        }

        //// GET: GolfCourses
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        [Authorize(Roles=("User, GolfCourseOwner, Admin"))]
        public async Task<ActionResult> MyCourses()
        {
            if (!SessionTimeOutVerification(Session["Username"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = _sessionMapperService.MapUserIdFromSessionUsername(Session["Username"].ToString());
            var myCoursesVm = await _golfCourseService.GetMyCourses(userId);
            return View(myCoursesVm); 
        }

        [HttpGet]
        [Authorize(Roles = ("User, GolfCourseOwner, Admin"))]
        public async Task<ActionResult> ManageMyCourse(int courseId)
        {
            if (!SessionTimeOutVerification(Session["Username"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = _sessionMapperService.MapUserIdFromSessionUsername(Session["Username"].ToString());
            var manageMyCourseViewModel = await _golfCourseService.GetManageCourseViewModel(courseId);
            if (manageMyCourseViewModel == null)
            {
                return RedirectToAction("MyCourses");
            }

            return View(manageMyCourseViewModel);
        }

        [HttpPost]
        [Authorize(Roles = ("User, GolfCourseOwner, Admin"))]
        public async Task<ActionResult> UploadCourseFootage(int courseId, int holeNumber)
        {
            if (!SessionTimeOutVerification(Session["Username"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            var path = "";
            var badResponseStatusCode = 200;

            try
            {
                var content = Request.Files[0];
                if (content == null || content.ContentLength <= 0) return Json("You must upload a video file (mp4).");

                var stream = content.InputStream;
                var fileName = Path.GetFileName(content.FileName);
                path = Path.Combine(Server.MapPath("~/App_Data/TempFootageUploads"), fileName);
                using (var fileStream = System.IO.File.Create(path))
                {
                    stream.CopyTo(fileStream);
                }
                var uploadCourseFootageViewModel =
                    _golfCourseService.GetGolfCourseUploadViewModel(courseId, holeNumber, path);
                var uploadSuccess = await _youtubeGolfService.UploadCourseFootageAsync(uploadCourseFootageViewModel);

                if (uploadSuccess)
                {
                    uploadCourseFootageViewModel.YouTubeVideoId =
                        _youtubeGolfService.GetUploadedVideoYouTubeIdentifier();
                    await _golfCourseService.StoreCourseFootageForHole(uploadCourseFootageViewModel);
                }

                System.IO.File.Delete(path);

                return Json(uploadSuccess
                    ? "Your video file was successfully uploaded to YouTube!"
                    : "There was an error uploading the video file to YouTube.");
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                badResponseStatusCode = Response.StatusCode;
            }

            System.IO.File.Delete(path);

            return badResponseStatusCode == 400 ? Json("Upload failed with + " + badResponseStatusCode.ToString()) : Json("Please try again and verify your internet connection has not been interrupted");
        }

        [HttpPost]
        [Authorize(Roles = ("User, GolfCourseOwner, Admin"))]
        public async Task<ActionResult> ModifyCourseFootage(int courseId, int holeNumber)
        {
            if (!SessionTimeOutVerification(Session["Username"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }

            //grab that video id from the db
            var youtubeVideoIdForHole =
                await _golfCourseService.RequestVideoIdAssociatedWithGolfCourseHole(courseId, holeNumber);
            if (youtubeVideoIdForHole == null)
            {
                return Json("Error: There is no YouTube Video Id associated with this hole.");
            }

            //THIS PART IS NOT WORKING. DELETE FROM YOUTUBE redirect_uri_mismatch
            //var youtubeResponse = await _youtubeGolfService.DeleteCourseFootageAsync(youtubeVideoIdForHole);

            await _golfCourseService.RequestVideoIdDeletion(youtubeVideoIdForHole);

            var path = "";
            var badResponseStatusCode = 200;

            try
            {
                var content = Request.Files[0];
                if (content == null || content.ContentLength <= 0) return Json("You must upload a video file (mp4).");

                var stream = content.InputStream;
                var fileName = Path.GetFileName(content.FileName);
                path = Path.Combine(Server.MapPath("~/App_Data/TempFootageUploads"), fileName);
                using (var fileStream = System.IO.File.Create(path))
                {
                    stream.CopyTo(fileStream);
                }
                var uploadCourseFootageViewModel =
                    _golfCourseService.GetGolfCourseUploadViewModel(courseId, holeNumber, path);
                var uploadSuccess = await _youtubeGolfService.UploadCourseFootageAsync(uploadCourseFootageViewModel);

                if (uploadSuccess)
                {
                    uploadCourseFootageViewModel.YouTubeVideoId =
                        _youtubeGolfService.GetUploadedVideoYouTubeIdentifier();
                    await _golfCourseService.StoreCourseFootageForHole(uploadCourseFootageViewModel);
                }

                System.IO.File.Delete(path);

                return Json(uploadSuccess
                    ? "Your video file was successfully uploaded to YouTube!"
                    : "There was an error uploading the video file to YouTube.");
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                badResponseStatusCode = Response.StatusCode;
            }

            System.IO.File.Delete(path);

            return badResponseStatusCode == 400 ? Json("Upload failed with + " + badResponseStatusCode.ToString()) : Json("Please try again and verify your internet connection has not been interrupted");
            //delete that video id from YouTube

            //get uploaded video file submitted from client

            //submit that video to YouTube

            //modify video id in CourseVideoTable
        }

        [HttpPost]
        [Authorize(Roles = ("User, GolfCourseOwner, Admin"))]
        public async Task<ActionResult> DeleteCourseFootage(int courseId, int holeNumber)
        {
            if (!SessionTimeOutVerification(Session["Username"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }

            //grab that video id from the 
            var youtubeVideoIdForHole =
                await _golfCourseService.RequestVideoIdAssociatedWithGolfCourseHole(courseId, holeNumber);
            if (youtubeVideoIdForHole == null)
            {
                return Json("Error: There is no YouTube Video Id associated with this hole.");
            }

            //THIS PART IS NOT WORKING. DELETE FROM YOUTUBE redirect_uri_mismatch
            //var youtubeResponse = await _youtubeGolfService.DeleteCourseFootageAsync(youtubeVideoIdForHole);

            await _golfCourseService.RequestVideoIdDeletion(youtubeVideoIdForHole);

            return Json("The course footage has been deleted.");
        }

        [HttpGet]
        public async Task<ActionResult> Browse()
        {
            var golfCourseViewModel = await _golfCourseService.GetExistingGolfCoursesViewModelAsync();
            return View(golfCourseViewModel);
        }

        // GET Explore
        [HttpGet]
        public async Task<ActionResult> Explore(int golfCourseId)
        {
            var vm = await _golfCourseService.GetCourseOverviewViewModel(golfCourseId);
            return View(vm);
        }

        [HttpGet]
        public async Task<ActionResult> VirtualTour(int golfCourseId)
        {
            var vm = await _golfCourseService.GetVirtualTourViewModel(golfCourseId);
            return View(vm);
        }
    }
    
}