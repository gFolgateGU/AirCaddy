using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirCaddy.Domain.ViewModels.GolfCourses
{
    public class CourseOverviewViewModel
    {
        public string CourseName { get; set; }

        public List<YelpGolfCourseReview> CourseReviews { get; set; }
    }
}
