using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirCaddy.Data.CustomDataModels
{
    public class UserPrivilegeRequest
    { 
        public string UserName { get; set; }

        public int Id { get; set; }

        public string GolfCourseName { get; set; }

        public string GolfCourseAddress { get; set; }

        public string CoursePhoneNumber { get; set; }

        public string GolfCourseType { get; set; }

        public string Reason { get; set; }

        public bool Verified { get; set; }

    }
}

