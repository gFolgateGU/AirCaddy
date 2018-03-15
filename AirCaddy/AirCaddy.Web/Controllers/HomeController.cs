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

        public async Task<ActionResult> About()
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("ejbyrd@hotmail.com", "Hey Mer - Right Dude Here");
            var subject = "I am currently SENDING IT with C# SendGrid";
            var to = new EmailAddress("morse005@knights.gannon.edu", "Example User");
            var plainTextContent = "and easy to do anywhere, even with C#";
            string htmlContent = "<strong>yeaboiwemadeit</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}