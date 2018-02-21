using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirCaddy.Data;

namespace AirCaddy.Domain.Special
{
    public interface ICourseBuilder
    {
        Task BuildCourse(int requestId);

        GolfCourse GetCourse();

        Tuple<GolfCourse, List<GolfCourseVideo>> GetCourseAndDefaultVideos();
    }
}
