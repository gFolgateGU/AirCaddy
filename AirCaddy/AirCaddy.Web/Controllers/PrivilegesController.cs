using AirCaddy.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirCaddy.Domain.ViewModels;
using AirCaddy.Domain.ViewModels.Privileges;

namespace AirCaddy.Controllers
{
    public class PrivilegesController : Controller
    {
        private readonly ISessionMapperService _sessionMapperService;

        public PrivilegesController(ISessionMapperService sessionMapperService)
        {
            _sessionMapperService = sessionMapperService;
        }

        // GET: Index
        public ActionResult Index()
        {
            return View();
        }

        // GET: MakeRequest
        [HttpGet]
        public ActionResult MakeRequest()
        {
            MakeRequest(null);
            return View();
        }

        // POST: MakeRquest
        [HttpPost]
        public ActionResult MakeRequest(PrivilegeRequestViewModel privilegeData)
        {
            var userId = _sessionMapperService.MapUserIdFromSessionUsername(Session["Username"].ToString());

            return RedirectToAction("Index", "Home");
        }
    }
}