using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirCaddy.Domain.ViewModels.Privileges
{
    public class GolfCourseInfoViewModel
    {
        public int CourseId { get; set; }

        public string CourseName { get; set; }

        public string CourseAddress { get; set; }

        public string PrimaryContact { get; set; }

        public string CourseType { get; set; }
    }
}
