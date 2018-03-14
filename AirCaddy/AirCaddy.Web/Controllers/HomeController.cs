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
        public HomeController()
        {

        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> About()
        {

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}