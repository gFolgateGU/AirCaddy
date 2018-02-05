using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirCaddy.Domain.ViewModels.GolfCourses
{
    public class YelpGolfCourseReview
    {
        public string Rating { get; set; }

        public string User { get; set; }

        public string ReviewText { get; set; }

        public DateTime ReviewDate { get; set; }
    }
}
