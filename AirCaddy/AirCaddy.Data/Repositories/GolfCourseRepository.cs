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

        Task AddNewGolfCourse(GolfCourse golfCourse);

        Task<IEnumerable<GolfCourse>> GetVerifiedGolfCoursesAsync();

        string GetGolfCourseName(int courseId);

        string GetExistingGolfCourseYelpApiKey(int courseId);

        Task<GolfCourse> GetCourseOwnedByUser(int courseId, string userId);
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

        public async Task AddNewGolfCourse(GolfCourse golfCourse)
        {
            _dataEntities.GolfCourses.Add(golfCourse);
            await _dataEntities.SaveChangesAsync();
        }

        public async Task<IEnumerable<GolfCourse>> GetVerifiedGolfCoursesAsync()
        {
            var golfCourses = await _dataEntities.GolfCourses.ToListAsync();
            return golfCourses;
        }

        public string GetGolfCourseName(int courseId)
        {
            var golfCourse = _dataEntities.GolfCourses.FirstOrDefault(gc => gc.Id.Equals(courseId));
            return golfCourse.Name;
        }

        public string GetExistingGolfCourseYelpApiKey(int courseId)
        {
            var golfCourse = _dataEntities.GolfCourses.FirstOrDefault(gc => gc.Id.Equals(courseId));
            return golfCourse.YelpApiCourseId;
        }

        public async Task<GolfCourse> GetCourseOwnedByUser(int courseId, string userId)
        {
            var golfCourse = await _dataEntities.GolfCourses.Where(gc => gc.Id.Equals(courseId)).FirstOrDefaultAsync();
            return golfCourse.UserId == userId ? golfCourse : null;
        }
    }
}
