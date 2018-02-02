using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirCaddy.Data;
using AirCaddy.Data.Repositories;
using AirCaddy.Domain.ViewModels.GolfCourses;

namespace AirCaddy.Domain.Services.GolfCourses
{
    public interface IGolfCourseService
    {
        Task<IEnumerable<GolfCourseViewModel>> GetExistingGolfCoursesViewModelAsync();
    }

    public class GolfCourseService : IGolfCourseService
    {
        private readonly IGolfCourseRepository _golfCourseRepository;

        public GolfCourseService(IGolfCourseRepository golfCourseRepository)
        {
            _golfCourseRepository = golfCourseRepository;
        }

        public async Task<IEnumerable<GolfCourseViewModel>> GetExistingGolfCoursesViewModelAsync()
        {
            var existingCourses = await _golfCourseRepository.GetVerifiedGolfCoursesAsync();
            var coursesViewModel = MapGolfEntityModelToGolfViewModel(existingCourses);
            return coursesViewModel;
        }

        private IEnumerable<GolfCourseViewModel> MapGolfEntityModelToGolfViewModel(
            IEnumerable<GolfCourse> golfCourseEntities)
        {
            var golfCoursesVm = golfCourseEntities.Select(golfCourse => new GolfCourseViewModel
                {
                    Id = golfCourse.Id,
                    CourseName = golfCourse.Name,
                    CourseAddress = golfCourse.Address,
                    CoursePrimaryContact = golfCourse.PhoneNumber,
                    CourseType = golfCourse.Type,
                    CourseOwnerId = golfCourse.UserId
                })
                .ToList();

            return golfCoursesVm;
        }
    }
}
