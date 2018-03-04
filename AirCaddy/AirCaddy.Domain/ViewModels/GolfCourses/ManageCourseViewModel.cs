using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirCaddy.Data;

namespace AirCaddy.Domain.ViewModels.GolfCourses
{
    public class ManageCourseViewModel
    {
        public ManageCourseViewModel()
        {
            GolfCourseHoleVideos = new List<CourseVideoViewModel>();
        }

        public int GolfCourseId { get; set; }

        public string GolfCourseName { get; set; }

        public string GolfCourseAddress { get; set; }

        public string GolfCoursePhone { get; set; }

        public string GolfCourseType { get; set; }

        public string GolfCourseOwnerId { get; set; }

        public List<CourseVideoViewModel> GolfCourseHoleVideos { get; set; }
    }
}
