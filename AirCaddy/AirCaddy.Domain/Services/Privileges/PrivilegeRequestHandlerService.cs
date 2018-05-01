using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirCaddy.Data;
using AirCaddy.Data.CustomDataModels;
using AirCaddy.Data.Repositories;
using AirCaddy.Domain.ViewModels.GolfCourses;
using AirCaddy.Domain.ViewModels.Privileges;
using MAX.USPS;

namespace AirCaddy.Domain.Services.Privileges
{
    public interface IPrivilegeRequestHandlerService
    {
        Task<bool> IsDuplicateEntryAsync(PrivilegeRequestViewModel pendingRequest);

        Task MakeCoursePrivilegeRequestAsync(PrivilegeRequestViewModel privilegeRequestVm, string userId);

        bool ValidateGolfCourseAddress(PrivilegeRequestViewModel privilegeRequestVm);

        Task<IEnumerable<UserPrivilegeRequest>> RetrievePendingPrivilegeRequestsAsync();

        Task<PrivilegesSummaryViewModel> GetPrivilegesSummaryForUserAsync(string userId);
        
        Task RequestDeleteAsync(int id);

        Task RequestAcceptAsync(int id);

        Task<PrivilegeRequest> RequestUserOwnsPrivilegeRequest(int privilegeId, string userId);

        Task<bool> RequestEditPrivilegeRequest(EditCourseViewModel editCourseVm, PrivilegeRequest privRequestInFocus);
    }

    public class PrivilegeRequestHandlerService : IPrivilegeRequestHandlerService
    {
        private readonly IPrivilegeRepository _privilegeRepository;
        private readonly IGolfCourseRepository _golfCourseRepository;
        private readonly string _uspsUserId;

        public PrivilegeRequestHandlerService(IPrivilegeRepository privilegeRepository,
            IGolfCourseRepository golfCourseRepository, string uspsUserId)
        {
            _privilegeRepository = privilegeRepository;
            _golfCourseRepository = golfCourseRepository;
            _uspsUserId = uspsUserId;
        }

        public async Task<bool> IsDuplicateEntryAsync(PrivilegeRequestViewModel pendingRequest)
        {
            var courseRequestExists =
                await _privilegeRepository.ExistsCourseRequestAsync(pendingRequest.CourseName, pendingRequest.CourseAddress);
            var courseExists =
                await _golfCourseRepository.ExistsGolfCourseEntryAsync(pendingRequest.CourseName, pendingRequest.CourseAddress);
            return courseRequestExists || courseExists;
        }

        public async Task MakeCoursePrivilegeRequestAsync(PrivilegeRequestViewModel privilegeRequestVm, string userId)
        {
            var concatenatedAddress = ConcatGolfCourseAddressInformation(privilegeRequestVm.CourseAddress,
                privilegeRequestVm.City,
                privilegeRequestVm.StateCode, privilegeRequestVm.Zip);

            var courseRequest = new PrivilegeRequest
            {
                GolfCourseName = privilegeRequestVm.CourseName,
                GolfCourseAddress = concatenatedAddress,
                Reason = privilegeRequestVm.Reason,
                CoursePhoneNumber = privilegeRequestVm.CoursePhoneNumber,
                GolfCourseType = privilegeRequestVm.CourseType,
                Verified = false,
                UserId = userId
            };
            await _privilegeRepository.AddCourseRequestAsync(courseRequest);
        }

        public bool ValidateGolfCourseAddress(PrivilegeRequestViewModel privilegeRequestVm)
        {
            var address = new Address
            {
                Address2 = privilegeRequestVm.CourseAddress,
                City = privilegeRequestVm.City,
                State = privilegeRequestVm.StateCode,
                Zip = privilegeRequestVm.Zip,
            };

            var uspsManager = new USPSManager(_uspsUserId);
            try
            {
                var validatedAddress = uspsManager.ValidateAddress(address);
            }
            catch (USPSManagerException uspsManagerException)
            {
                return false;
            }
            return true;
        }

        public async Task<IEnumerable<UserPrivilegeRequest>> RetrievePendingPrivilegeRequestsAsync()
        {
            var pendingRequestData = await _privilegeRepository.GetAllPendingRequestsAsync();
            return pendingRequestData;
        }

        public async Task<PrivilegesSummaryViewModel> GetPrivilegesSummaryForUserAsync(string userId)
        {
            var golfCoursesOwnedByUser = await _golfCourseRepository.GetGolfCoursesOwnedByUserAsync(userId);
            var pendingRequests = await _privilegeRepository.GetAllPendingRequestsForUserAsync(userId);
            var privilegesSummary =
                MapUserCoursesAndRequestsToSummaryViewModel(golfCoursesOwnedByUser, pendingRequests);
            return privilegesSummary;
        }

        private string ConcatGolfCourseAddressInformation(string address, string city, string stateCode, string zip)
        {
            var courseAddressConcat = address + ", " + city + ", " + stateCode + " " + zip;
            return courseAddressConcat;
        }

        private PrivilegesSummaryViewModel MapUserCoursesAndRequestsToSummaryViewModel(
            IEnumerable<GolfCourse> userGolfCourses, IEnumerable<PrivilegeRequest> userPendingRequests)
        {
            var privilegeSummaryVm = new PrivilegesSummaryViewModel
            {
                MyCourses = new List<GolfCourseInfoViewModel>(),
                MyPendingCourses = new List<GolfCourseInfoViewModel>()
            };

            foreach (var userGolfCourse in userGolfCourses)
            {
                var golfCourseInfoVm = new GolfCourseInfoViewModel
                {
                    CourseId = userGolfCourse.Id,
                    CourseName = userGolfCourse.Name,
                    CourseAddress = userGolfCourse.Address,
                    CourseType = userGolfCourse.Type,
                    PrimaryContact = userGolfCourse.PhoneNumber
                };
                privilegeSummaryVm.MyCourses.Add(golfCourseInfoVm);
            }
            foreach (var userPendingRequest in userPendingRequests)
            {
                var golfCourseInfoVm = new GolfCourseInfoViewModel
                {
                    //This is really the privilege idenitifer...
                    CourseId = userPendingRequest.Id,
                    CourseName = userPendingRequest.GolfCourseName,
                    CourseAddress = userPendingRequest.GolfCourseAddress,
                    CourseType = userPendingRequest.GolfCourseType,
                    PrimaryContact = userPendingRequest.CoursePhoneNumber
                };
                privilegeSummaryVm.MyPendingCourses.Add(golfCourseInfoVm);
            }

            return privilegeSummaryVm;
        }

        public async Task RequestDeleteAsync(int id)
        {
            await _privilegeRepository.DeleteRequestAsync(id);
        }
        
        public async Task RequestAcceptAsync(int id)
        {
            await _privilegeRepository.AcceptRequestAsync(id);
        }

        public async Task<PrivilegeRequest> RequestUserOwnsPrivilegeRequest(int privilegeId, string userId)
        {
            var privilegeRequest = await _privilegeRepository.CheckIfPrivilegeRequestIsOwnedByUser(privilegeId, userId);
            return privilegeRequest;
        }

        public async Task<bool> RequestEditPrivilegeRequest(EditCourseViewModel editCourseVm, PrivilegeRequest privRequestInFocus)
        {
            if (privRequestInFocus.GolfCourseName != editCourseVm.NewCourseName)
            {
                privRequestInFocus.GolfCourseName = editCourseVm.NewCourseName;
            }
            if (privRequestInFocus.CoursePhoneNumber != editCourseVm.NewCoursePhone)
            {
                privRequestInFocus.CoursePhoneNumber = editCourseVm.NewCoursePhone;
            }
            if (privRequestInFocus.GolfCourseType != editCourseVm.NewCourseType)
            {
                privRequestInFocus.GolfCourseType = editCourseVm.NewCourseType;
            }

            var result = await _privilegeRepository.EditPrivilegeRequest(privRequestInFocus);

            return result;
        }
    }
}
