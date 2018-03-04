using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirCaddy.Domain.ViewModels.GolfCourses
{
    public class CourseVideoViewModel
    {
        public int VideoId { get; set; }

        public int CourseHoleNumber { get; set; }

        public string YouTubeVideoId { get; set; }

        public int CourseId { get; set; }
    }
}
