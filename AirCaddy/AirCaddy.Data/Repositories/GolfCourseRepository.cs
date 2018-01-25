using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirCaddy.Data.Repositories
{
    public interface IGolfCourseRepository
    {
        Task<bool> ExistsGolfCourseEntryAsync(string courseName, string courseAddress);

        Task<IEnumerable<GolfCourse>> GetGolfCoursesOwnedByUserAsync(string userId);
    }

    public class GolfCourseRepository : BaseRepository, IGolfCourseRepository
    {
        public GolfCourseRepository() : base() { }

        public async Task<bool> ExistsGolfCourseEntryAsync(string courseName, string courseAddress)
        {
            var golfCourse = await _dataEntities.GolfCourses.FirstOrDefaultAsync(gc => gc.Name.Contains(courseName)
                                                             && gc.Address.Contains(courseAddress));
            return golfCourse != null;
        }

        public async Task<IEnumerable<GolfCourse>> GetGolfCoursesOwnedByUserAsync(string userId)
        {
            var userGolfCourses = await _dataEntities.GolfCourses
                                                     .Where(gc => gc.UserId.Contains(userId)).ToListAsync();
            return userGolfCourses;
        }
    }
}
