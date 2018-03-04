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

        Task AddNewGolfCourseWithDefaultVideos(Tuple<GolfCourse, List<GolfCourseVideo>> golfCourseWithDefaultVideos);

        Task<IEnumerable<GolfCourse>> GetVerifiedGolfCoursesAsync();

        string GetGolfCourseName(int courseId);

        string GetExistingGolfCourseYelpApiKey(int courseId);

        Task<GolfCourse> GetCourseOwnedByUser(int courseId, string userId);

        Task<Tuple<GolfCourse, List<GolfCourseVideo>>> GetGolfCourseAndCourseVideoInfo(int golfCourseId);

        Task AddCourseFootageForHole(int courseId, int holeNumber, string youtubeVideoId);
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

        public async Task AddNewGolfCourseWithDefaultVideos(Tuple<GolfCourse, List<GolfCourseVideo>> golfCourseWithDefaultVideos)
        {
            var golfCourse = golfCourseWithDefaultVideos.Item1;
            var golfCourseDefaultVideos = golfCourseWithDefaultVideos.Item2;
            _dataEntities.GolfCourses.Add(golfCourse);
            _dataEntities.GolfCourseVideos.AddRange(golfCourseDefaultVideos);
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

        public async Task<Tuple<GolfCourse, List<GolfCourseVideo>>> GetGolfCourseAndCourseVideoInfo(int golfCourseId)
        {
            var golfCourseInfo = await _dataEntities.GolfCourses.Where(gc => gc.Id.Equals(golfCourseId))
                .FirstOrDefaultAsync();
            if (golfCourseInfo == null)
            {
                return null;
            }
            var golfCourseHoleVideos =
                await _dataEntities.GolfCourseVideos.Where(gcv => gcv.GolfCourseId.Equals(golfCourseId)).ToListAsync();
            var golfCourseWithVideoInfo =
                new Tuple<GolfCourse, List<GolfCourseVideo>>(golfCourseInfo, golfCourseHoleVideos);
            return golfCourseWithVideoInfo;
        }

        public async Task AddCourseFootageForHole(int courseId, int holeNumber, string youtubeVideoId)
        {
            var courseFootageEntry = await _dataEntities.GolfCourseVideos.Where(
                gcv => gcv.GolfCourseId.Equals(courseId) && gcv.HoleNumber.Equals(holeNumber)).FirstOrDefaultAsync();
            if (courseFootageEntry != null)
            {
                courseFootageEntry.YoutubeHoleVideoId = youtubeVideoId;
            }
            else
            {
                return;
            }
            await _dataEntities.SaveChangesAsync();
        }
    }
}
