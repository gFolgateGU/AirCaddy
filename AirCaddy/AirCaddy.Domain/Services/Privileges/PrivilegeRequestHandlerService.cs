using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirCaddy.Data;
using AirCaddy.Data.Repositories;
using AirCaddy.Domain.ViewModels.Privileges;

namespace AirCaddy.Domain.Services.Privileges
{
    public interface IPrivilegeRequestHandlerService
    {
        Task<bool> IsDuplicateEntryAsync(PrivilegeRequestViewModel pendingRequest);

        Task MakeCoursePrivilegeRequestAsync(PrivilegeRequestViewModel privilegeRequestVm, string userId);
    }

    public class PrivilegeRequestHandlerService : IPrivilegeRequestHandlerService
    {
        private readonly IPrivilegeRepository _privilegeRepository;
        private readonly IGolfCourseRepository _golfCourseRepository;

        public PrivilegeRequestHandlerService(IPrivilegeRepository privilegeRepository,
            IGolfCourseRepository golfCourseRepository)
        {
            _privilegeRepository = privilegeRepository;
            _golfCourseRepository = golfCourseRepository;
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
            var courseRequest = new PrivilegeRequest
            {
                GolfCourseName = privilegeRequestVm.CourseName,
                GolfCourseAddress = privilegeRequestVm.CourseAddress,
                Reason = privilegeRequestVm.Reason,
                CoursePhoneNumber = privilegeRequestVm.CoursePhoneNumber,
                GolfCourseType = privilegeRequestVm.CourseType,
                Verified = false,
                UserId = userId
            };
            await _privilegeRepository.AddCourseRequestAsync(courseRequest);
        }
    }
}
