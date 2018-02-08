using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
