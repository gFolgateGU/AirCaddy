using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirCaddy.Data;
using AirCaddy.Data.Repositories;
using AirCaddy.Domain.ViewModels.Privileges;
using MAX.USPS;

namespace AirCaddy.Domain.Services.Privileges
{
    public interface IPrivilegeRequestHandlerService
    {
        Task<bool> IsDuplicateEntryAsync(PrivilegeRequestViewModel pendingRequest);

        Task MakeCoursePrivilegeRequestAsync(PrivilegeRequestViewModel privilegeRequestVm, string userId);

        bool ValidateGolfCourseAddress(PrivilegeRequestViewModel privilegeRequestVm);
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

        private string ConcatGolfCourseAddressInformation(string address, string city, string stateCode, string zip)
        {
            var courseAddressConcat = address + ", " + city + ", " + stateCode + " " + zip;
            return courseAddressConcat;
        }
    }
}
