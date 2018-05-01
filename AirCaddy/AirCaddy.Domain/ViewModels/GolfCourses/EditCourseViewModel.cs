using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirCaddy.Domain.ViewModels.GolfCourses
{
    public class EditCourseViewModel
    {
        public int CourseId { get; set; }

        public string NewCourseName { get; set; }

        public string NewCoursePhone { get; set; }

        public string NewCourseType { get; set; }
    }
}
