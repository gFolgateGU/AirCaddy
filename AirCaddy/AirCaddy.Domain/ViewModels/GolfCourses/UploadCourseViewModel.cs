using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirCaddy.Domain.ViewModels.GolfCourses
{
    public class UploadCourseViewModel
    {
        public string CourseId { get; set; }

        public string CourseName { get; set; }

        public int HoleNumber { get; set; }

        public string HoleVideoPath { get; set; }
    }
}
