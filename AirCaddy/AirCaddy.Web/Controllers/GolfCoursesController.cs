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
using System.Net.Http;
using System.Security.Permissions;
using AirCaddy.Domain.ViewModels.GolfCourses;

namespace AirCaddy.Controllers
{
    public class GolfCoursesController : BaseController
    {
        private readonly IGolfCourseService _golfCourseService;
        private readonly ISessionMapperService _sessionMapperService;
        private readonly IVimeoFootageService _vimeoFootageService;

        public GolfCoursesController(IGolfCourseService golfCourseService,
            ISessionMapperService sessionMapperService, IVimeoFootageService vimeoFootageService)
        {
            _golfCourseService = golfCourseService;
            _sessionMapperService = sessionMapperService;
            _vimeoFootageService = vimeoFootageService;
        }

        [HttpGet]
        [Authorize(Roles=("User, GolfCourseOwner, Admin"))]
        public async Task<ActionResult> MyCourses()
        {
            if (Session["Username"] == null)
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
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = _sessionMapperService.MapUserIdFromSessionUsername(Session["Username"].ToString());

            var courseOwnedByUser = await _golfCourseService.RequestCourseOwnedByUser(courseId, userId);

            if (courseOwnedByUser == null)
            {
                //That user does not own that course.
                return RedirectToAction("Browse", "GolfCourses");
            }

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
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = _sessionMapperService.MapUserIdFromSessionUsername(Session["Username"].ToString());

            var courseOwnedByUser = await _golfCourseService.RequestCourseOwnedByUser(courseId, userId);

            if (courseOwnedByUser == null)
            {
                //That user does not own that course.
                return RedirectToAction("Browse", "GolfCourses");
            }

            var path = "";
            var badResponseStatusCode = 200;

            try
            {
                var content = Request.Files[0];

                if (content == null || content.ContentLength <= 0) return Json("You must upload a video file.");

                var stream = content.InputStream;
                var fileName = Path.GetFileName(content.FileName);

                if (!_golfCourseService.IsProperVideoFileExtension(fileName))
                {
                    return Json("The video file supplied is not a supported type");
                }

                path = Path.Combine(Server.MapPath("~/App_Data/TempFootageUploads"), fileName);
                using (var fileStream = System.IO.File.Create(path))
                {
                    stream.CopyTo(fileStream);
                }
                var uploadedVideoModel = _golfCourseService.GetGolfCourseUploadViewModel(courseId, holeNumber, path);
                var vimeoId = await _vimeoFootageService.UploadCourseFootageAsync(uploadedVideoModel);
                System.IO.File.Delete(path);
                if (vimeoId != string.Empty)
                {
                    uploadedVideoModel.YouTubeVideoId = vimeoId;
                    await _golfCourseService.StoreCourseFootageForHole(uploadedVideoModel);
                    return Json("Your video file was successfully uploaded to YouTube");
                }
                return Json("There was an error uploading your footage to vimeo");
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
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = _sessionMapperService.MapUserIdFromSessionUsername(Session["Username"].ToString());

            var courseOwnedByUser = await _golfCourseService.RequestCourseOwnedByUser(courseId, userId);

            if (courseOwnedByUser == null)
            {
                //That user does not own that course.
                return RedirectToAction("Browse", "GolfCourses");
            }

            //grab that video id from the db
            var vimeoIdForHole =
                await _golfCourseService.RequestVideoIdAssociatedWithGolfCourseHole(courseId, holeNumber);
            if (vimeoIdForHole == null)
            {
                return Json("Error: There is no Vimeo Video Id associated with this hole.");
            }

            var vimeoResponse = await _vimeoFootageService.DeleteCourseFootageAsync(vimeoIdForHole);
            if (vimeoResponse == false)
            {
                return Json("Error: This video could not be removed from vimeo at this time.  Please try again");
            }

            await _golfCourseService.RequestVideoIdDeletion(vimeoIdForHole);

            var path = "";
            var badResponseStatusCode = 200;

            //upload portion
            try
            {
                var content = Request.Files[0];
                if (content == null || content.ContentLength <= 0) return Json("You must upload a video file (mp4).");

                var stream = content.InputStream;
                var fileName = Path.GetFileName(content.FileName);

                if (!_golfCourseService.IsProperVideoFileExtension(fileName))
                {
                    return Json("The video file supplied is not a supported type");
                }

                path = Path.Combine(Server.MapPath("~/App_Data/TempFootageUploads"), fileName);
                using (var fileStream = System.IO.File.Create(path))
                {
                    stream.CopyTo(fileStream);
                }
                var uploadCourseFootageViewModel =
                    _golfCourseService.GetGolfCourseUploadViewModel(courseId, holeNumber, path);
                var uploadedVimeoId = await _vimeoFootageService.UploadCourseFootageAsync(uploadCourseFootageViewModel);

                if (uploadedVimeoId != string.Empty)
                {
                    uploadCourseFootageViewModel.YouTubeVideoId = uploadedVimeoId;
                    await _golfCourseService.StoreCourseFootageForHole(uploadCourseFootageViewModel);
                }

                System.IO.File.Delete(path);

                return Json((uploadedVimeoId != string.Empty)
                    ? "Your video file was successfully uploaded to Vimeo!"
                    : "There was an error uploading the video file to Vimeo.");
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
        public async Task<ActionResult> DeleteCourseFootage(int courseId, int holeNumber)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = _sessionMapperService.MapUserIdFromSessionUsername(Session["Username"].ToString());

            var courseOwnedByUser = await _golfCourseService.RequestCourseOwnedByUser(courseId, userId);

            if (courseOwnedByUser == null)
            {
                //That user does not own that course.
                return RedirectToAction("Browse", "GolfCourses");
            }

            //grab that video id from the 
            var vimeoIdForHole =
                await _golfCourseService.RequestVideoIdAssociatedWithGolfCourseHole(courseId, holeNumber);
            if (vimeoIdForHole == null)
            {
                return Json("Error: There is no Vimeo Video Id associated with this hole.");
            }

            var result =  await _vimeoFootageService.DeleteCourseFootageAsync(vimeoIdForHole);

            if (result != true)
            {
                return Json("There was a problem removing this video from vimeo..");
            }
            await _golfCourseService.RequestVideoIdDeletion(vimeoIdForHole);

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

        [HttpPost]
        [Authorize(Roles=("User, GolfCourseOwner, Admin"))]
        public async Task<ActionResult> PostDifficultyRatingForHole(GolfCourseHoleRatingViewModel difficultyRating)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = _sessionMapperService.MapUserIdFromSessionUsername(Session["Username"].ToString());
            var resultStatus =
                await _golfCourseService.RequestDifficultyRatingPost(difficultyRating, userId);

            return Json(resultStatus);
        }

        [HttpPost]
        [Authorize(Roles=("User, GolfCourseOwner, Admin"))]
        public async Task<ActionResult> DeleteGolfCourse(int golfCourseId)
        {
            var resultStatus = false;
            return Json(resultStatus);
        }
    }  
}