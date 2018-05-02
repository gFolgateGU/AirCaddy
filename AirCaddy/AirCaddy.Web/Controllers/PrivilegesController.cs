using AirCaddy.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AirCaddy.Domain.Services.GolfCourses;
using AirCaddy.Domain.Services.Privileges;
using AirCaddy.Domain.Special;
using AirCaddy.Domain.ViewModels;
using AirCaddy.Domain.ViewModels.Privileges;
using AirCaddy.Domain.ViewModels.GolfCourses;

namespace AirCaddy.Controllers
{
    public class PrivilegesController : Controller
    {
        private readonly ISessionMapperService _sessionMapperService;
        private readonly IPrivilegeRequestHandlerService _privilegeRequestHandlerService;
        private readonly ICourseBuilder _courseBuilder;
        private readonly IGolfCourseService _golfCourseService;

        public PrivilegesController(ISessionMapperService sessionMapperService, 
            IPrivilegeRequestHandlerService privilegeRequestHandlerService,
            ICourseBuilder courseBuilder, IGolfCourseService golfCourseService)
        {
            _sessionMapperService = sessionMapperService;
            _privilegeRequestHandlerService = privilegeRequestHandlerService;
            _courseBuilder = courseBuilder;
            _golfCourseService = golfCourseService;
        }

        // GET: Index
        [HttpGet]
        [Authorize(Roles = "User, GolfCourseOwner, Admin")]
        public async Task<ActionResult> Index()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = _sessionMapperService.MapUserIdFromSessionUsername(Session["Username"].ToString());
            var vm = await _privilegeRequestHandlerService.GetPrivilegesSummaryForUserAsync(userId);
            return View(vm);
        }

        // GET: ManageRequests
        [HttpGet]
        [Authorize(Roles =("Admin"))]
        public async Task<ActionResult> ManageRequests()
        {
            var vm = await _privilegeRequestHandlerService.RetrievePendingPrivilegeRequestsAsync();
            return View(vm);
        }

        // GET: MakeRequest
        [HttpGet]
        [Authorize(Roles = "User, GolfCourseOwner, Admin")]
        public ActionResult MakeRequest()
        {
            return View();
        }

        // POST: MakeRquest
        [HttpPost]
        [Authorize(Roles="User, GolfCourseOwner, Admin")]
        public async Task<ActionResult> MakeRequest(PrivilegeRequestViewModel privilegeData)
        {
            if (Session["Username"] == null)
            {
                return Json(false);
            }
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

        [HttpPost]
        [Authorize(Roles =("Admin"))]
        public async Task<ActionResult> AcceptRequest(int id)
        {
            await _courseBuilder.BuildCourse(id);
            var golfCourseWithDefaultVideos = _courseBuilder.GetCourseAndDefaultVideos();
            _golfCourseService.RequestAddGolfCourseToSystem(golfCourseWithDefaultVideos);
            await _privilegeRequestHandlerService.RequestAcceptAsync(id);
            
            return Json(1);
        }


        [HttpPost]
        [Authorize(Roles =("Admin"))]
        public async Task<ActionResult> DenyRequest(int id)
        {
            await _privilegeRequestHandlerService.RequestDeleteAsync(id);
            
            return Json(1);
        }

        [HttpPost]
        [Authorize(Roles="User, GolfCourseOwner, Admin")]
        public async Task<ActionResult> DeleteExistingGolfCoursePrivilege(int id)
        {
            if (Session["Username"] == null)
            {
                return Json(1);
            }
            var userId = _sessionMapperService.MapUserIdFromSessionUsername(Session["Username"].ToString());

            var courseOwnedByUser = await _golfCourseService.RequestCourseOwnedByUser(id, userId);

            if (courseOwnedByUser == null)
            {
                var privRequestOwnedByUser = await _privilegeRequestHandlerService.RequestUserOwnsPrivilegeRequest(id, userId);
                if (privRequestOwnedByUser == null)
                {
                    //That user really does not own that course or request then..
                    return Json(2);
                }
                else
                {
                    var privilegeRequestDeletedResult = await _privilegeRequestHandlerService.RequestDeletePrivilegeRequest(id);
                    return Json(privilegeRequestDeletedResult);
                }
            }

            var golfCourseDeleteRequestResult = await _golfCourseService.RequestDeleteGolfCourse(id);
            return Json(golfCourseDeleteRequestResult);
        }

        [HttpPost]
        [Authorize(Roles=("User, GolfCourseOwner, Admin"))]
        public async Task<ActionResult> EditExistingGolfCoursePrivilege(EditCourseViewModel editCourseViewModel)
        {
            if (Session["Username"] == null)
            {
                return Json(1);
            }
            var userId = _sessionMapperService.MapUserIdFromSessionUsername(Session["Username"].ToString());

            var courseOwnedByUser = await _golfCourseService.RequestCourseOwnedByUser(editCourseViewModel.CourseId, userId);
            if (courseOwnedByUser == null)
            {
                //That user does not own that course but we are not giving up, he may have a pending privilege.
                var privRequestOwnedByUser = await _privilegeRequestHandlerService.RequestUserOwnsPrivilegeRequest(editCourseViewModel.CourseId, userId);
                if (privRequestOwnedByUser == null)
                {
                    //That user really does not own that course or request then..
                    return Json(2);
                }
                else
                {
                    var privilegeRequestUpdatedResult = await _privilegeRequestHandlerService.RequestEditPrivilegeRequest(editCourseViewModel, privRequestOwnedByUser);
                    return Json(privilegeRequestUpdatedResult);
                }

            }

            var golfCourseUpdatedResult = await _golfCourseService.RequestEditGolfCourseProperties(editCourseViewModel, courseOwnedByUser);

            return Json(golfCourseUpdatedResult);
        }
    }
}