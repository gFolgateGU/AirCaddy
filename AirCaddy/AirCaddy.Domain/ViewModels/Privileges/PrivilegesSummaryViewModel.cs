using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirCaddy.Domain.ViewModels.Privileges;

namespace AirCaddy.Domain.ViewModels.Privileges
{
    public class PrivilegesSummaryViewModel
    {
        public List<GolfCourseInfoViewModel> MyPendingCourses { get; set; }

        public List<GolfCourseInfoViewModel> MyCourses { get; set; }
    }
}
