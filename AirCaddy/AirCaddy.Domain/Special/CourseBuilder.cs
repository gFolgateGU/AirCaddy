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
    public abstract class CourseBuilder : ICourseBuilder
    {
        protected GolfCourse GolfCourse;

        protected CourseBuilder()
        {
            GolfCourse = new GolfCourse();
        }

        public abstract Task BuildCourse(int requestId);

        public GolfCourse GetCourse()
        {
            return GolfCourse;
        }
    }
}
