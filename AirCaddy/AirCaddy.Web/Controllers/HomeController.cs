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

        public async Task<ActionResult> About()
        {
            var testObj = new YoutubeGolfService();
            var testUpload = new UploadCourseViewModel
            {
                CourseId = "1",
                CourseName = "Elk Valley Golf Course",
                HoleNumber = 17,
                HoleVideoPath = @"C:\Users\gfolg\Desktop\SampleGolfCourseHole.mp4"
            };
            await testObj.UploadCourseFootageAsync(testUpload);

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}