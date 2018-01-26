using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirCaddy.Data;
using AirCaddy.Data.Repositories;

namespace AirCaddy.Domain.Services.GolfCourses
{
    public interface IGolfCourseService
    {
        Task<IEnumerable<GolfCourse>> GetExistingGolfCoursesAsync();
    }

    public class GolfCourseService : IGolfCourseService
    {
        private readonly IGolfCourseRepository _golfCourseRepository;

        public GolfCourseService(IGolfCourseRepository golfCourseRepository)
        {
            _golfCourseRepository = golfCourseRepository;
        }

        public async Task<IEnumerable<GolfCourse>> GetExistingGolfCoursesAsync()
        {
            var existingCourses = await _golfCourseRepository.GetVerifiedGolfCoursesAsync();
            return existingCourses;
        }
    }
}
