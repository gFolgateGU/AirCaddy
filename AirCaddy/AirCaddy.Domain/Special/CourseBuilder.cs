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
    public abstract class CourseBuilder
    {
        protected GolfCourse _golfCourse;
        protected IYelpGolfCourseReviewService _yelpGolfCourseReviewService;
        protected IPrivilegeRepository _privilegeRepository;

        protected CourseBuilder()
        {
            _golfCourse = new GolfCourse();
        }

        protected abstract void BuildCourse(int requestId);

        protected GolfCourse GetCourse()
        {
            return _golfCourse;
        }

    }
}
