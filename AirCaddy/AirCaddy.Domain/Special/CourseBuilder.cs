using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirCaddy.Data;
using AirCaddy.Data.Repositories;
using AirCaddy.Domain.Services.GolfCourses;

namespace AirCaddy.Domain.Special
{
    public abstract class CourseBuilder : ICourseBuilder
    {
        protected GolfCourse GolfCourse;

        protected CourseBuilder()
        {
            GolfCourse = new GolfCourse();
        }

        public abstract Task BuildCourse(int requestId);

        public GolfCourse GetCourse()
        {
            return GolfCourse;
        }

        public Tuple<GolfCourse, List<GolfCourseVideo>> GetCourseAndDefaultVideos()
        {
            var golfCourseVideos = new List<GolfCourseVideo>();
            const int holesInGolfCourse = 18;

            for (var i = 1; i <= holesInGolfCourse; i++)
            {
                golfCourseVideos.Add(new GolfCourseVideo
                {
                    GolfCourseId = GolfCourse.Id,
                    HoleNumber = i,
                    YoutubeHoleVideoId = ""
                });
            }

            var golfCourseAndVideos = new Tuple<GolfCourse, List<GolfCourseVideo>>(GolfCourse, golfCourseVideos);
            return golfCourseAndVideos;
        }
    }
}
