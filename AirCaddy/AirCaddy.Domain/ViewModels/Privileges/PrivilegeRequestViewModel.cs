using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirCaddy.Domain.ViewModels.Privileges
{
    public class PrivilegeRequestViewModel
    {
        public string CourseName { get; set; }

        public string CourseAddress { get; set; }

        public string CoursePhoneNumber { get; set; }

        public string Reason { get; set; }
    }
}
