using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirCaddy.Domain.ViewModels.GolfCourses
{
    public class GolfCourseViewModel
    {
        public int Id { get; set; }

        public string CourseName { get; set; }

        public string CourseAddress { get; set; }

        public string CoursePrimaryContact { get; set; }

        public string CourseType { get; set; }

        public string CourseOwnerId { get; set; }
    }
}
