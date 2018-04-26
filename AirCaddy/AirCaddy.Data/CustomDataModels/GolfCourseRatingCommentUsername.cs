using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirCaddy.Data.CustomDataModels
{
    public class GolfCourseRatingCommentUsername
    {
        public int Id { get; set; }

        public int HoleNumber { get; set; }

        public int DifficultyRating { get; set; }

        public string HoleComment { get; set; }

        public int GolfCourseId { get; set; }

        public string Username { get; set; }
    }
}
