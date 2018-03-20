using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirCaddy.Domain.ViewModels.GolfCourses
{
    public class GolfCourseHoleRatingViewModel
    {
        public int Id { get; set; }

        public int HoleNumber { get; set; }
        
        public int GolfCourseId { get; set; }

        public int Difficulty { get; set; }

        public string Comment { get; set; }

        public string Username { get; set; }
    }
}
