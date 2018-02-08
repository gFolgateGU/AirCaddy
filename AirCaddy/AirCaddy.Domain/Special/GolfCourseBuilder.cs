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

        protected override void BuildCourse(int requestId)
        {
            throw new NotImplementedException();
        }

        private async Task<GolfCourse> MapPrivilegeRequestoGolfCourseInfo(int requestId)
        {
            var privRequestInfo = await _privilegeRepository.GetPrivilegeRequestAsync(requestId);
            _golfCourse.Name = privRequestInfo.GolfCourseName;
            _golfCourse.Address = privRequestInfo.GolfCourseAddress;
            _golfCourse.Type = privRequestInfo.GolfCourseType;
            _golfCourse.PhoneNumber = privRequestInfo.CoursePhoneNumber;
            _golfCourse.UserId = privRequestInfo.UserId;
            return null;
        }
    }
}
