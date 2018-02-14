using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirCaddy.Data;
using AirCaddy.Data.Repositories;
using AirCaddy.Domain.Services.GolfCourses;


namespace AirCaddy.Domain.Special
{
    public class GolfCourseBuilder : CourseBuilder
    {
        private readonly IPrivilegeRepository _privilegeRepository;
        private readonly IYelpGolfCourseReviewService _yelpGolfCourseReviewService;

        public GolfCourseBuilder(IPrivilegeRepository privilegeRepository,
            IYelpGolfCourseReviewService yelpGolfCourseReviewService) : base()
        {
            _privilegeRepository = privilegeRepository;
            _yelpGolfCourseReviewService = yelpGolfCourseReviewService;
        }

        public override async Task BuildCourse(int requestId)
        {
            await MapPrivilegeRequestDataToGolfCourse(requestId);
            await LookUpAndStoreGolfCourseYelpApiId();
        }

        private async Task MapPrivilegeRequestDataToGolfCourse(int requestId)
        {
            var requestInfo = await _privilegeRepository.GetPrivilegeRequest(requestId);
            GolfCourse.Name = requestInfo.GolfCourseName;
            GolfCourse.Address = requestInfo.GolfCourseAddress;
            GolfCourse.PhoneNumber = requestInfo.CoursePhoneNumber;
            GolfCourse.Type = requestInfo.GolfCourseType;
            GolfCourse.UserId = requestInfo.UserId;           
        }

        private async Task LookUpAndStoreGolfCourseYelpApiId()
        {
            var yelpApiId =
                await _yelpGolfCourseReviewService.FindGolfCourseGivenSearchName(GolfCourse.Address, GolfCourse.Name);
            GolfCourse.YelpApiCourseId = yelpApiId;
        }
    }
}
