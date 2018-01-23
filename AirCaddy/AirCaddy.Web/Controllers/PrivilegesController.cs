using AirCaddy.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AirCaddy.Domain.Services.Privileges;
using AirCaddy.Domain.ViewModels;
using AirCaddy.Domain.ViewModels.Privileges;

namespace AirCaddy.Controllers
{
    public class PrivilegesController : Controller
    {
        private readonly ISessionMapperService _sessionMapperService;
        private readonly IPrivilegeRequestHandlerService _privilegeRequestHandlerService;

        public PrivilegesController(ISessionMapperService sessionMapperService, 
            IPrivilegeRequestHandlerService privilegeRequestHandlerService)
        {
            _sessionMapperService = sessionMapperService;
            _privilegeRequestHandlerService = privilegeRequestHandlerService;
        }

        // GET: Index
        public ActionResult Index()
        {
            return View();
        }

        // GET: ManageRequests
        [HttpGet]
        public ActionResult ManageRequests()
        {
            return View();
        }

        // GET: MakeRequest
        [HttpGet]
        public ActionResult MakeRequest()
        {
            return View();
        }

        // POST: MakeRquest
        [HttpPost]
        public async Task<ActionResult> MakeRequest(PrivilegeRequestViewModel privilegeData)
        {
            var userId = _sessionMapperService.MapUserIdFromSessionUsername(Session["Username"].ToString());
            if (await _privilegeRequestHandlerService.IsDuplicateEntryAsync(privilegeData))
            {
                //course is duplicate entry
                return Json(1);
            }
            if (!_privilegeRequestHandlerService.ValidateGolfCourseAddress(privilegeData))
            {
                //course address is not valid
                return Json(2);
            }
            await _privilegeRequestHandlerService.MakeCoursePrivilegeRequestAsync(privilegeData, userId);
            return Json(3);
        }
    }
}