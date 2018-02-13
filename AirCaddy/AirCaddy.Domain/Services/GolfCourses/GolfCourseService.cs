using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        void RequestAddGolfCourseToSystem(GolfCourse golfCourse);

        Task<CourseOverviewViewModel> GetCourseOverviewViewModel(int courseId);

    }

    public class GolfCourseService : IGolfCourseService
    {
        private readonly IGolfCourseRepository _golfCourseRepository;
        private readonly IYelpGolfCourseReviewservice _yelpGolfCourseReviewservice;

        public GolfCourseService(IGolfCourseRepository golfCourseRepository, IYelpGolfCourseReviewservice yelpGolfCourseReviewservice)
        {
            _golfCourseRepository = golfCourseRepository;
            _yelpGolfCourseReviewservice = yelpGolfCourseReviewservice;
        }

        public async Task<IEnumerable<GolfCourseViewModel>> GetExistingGolfCoursesViewModelAsync()
        {
            var existingCourses = await _golfCourseRepository.GetVerifiedGolfCoursesAsync();
            var coursesViewModel = MapGolfEntityModelToGolfViewModel(existingCourses);
            return coursesViewModel;
        }

        public async Task<CourseOverviewViewModel> GetCourseOverviewViewModel(int courseId)
        {
            var viewModel = new CourseOverviewViewModel
            {
                CourseReviews = new List<YelpGolfCourseReview>()
            };
            var courseName = _golfCourseRepository.GetGolfCourseName(courseId);
            var yelpCourseApiKey = _golfCourseRepository.GetExistingGolfCourseYelpApiKey(courseId);
            var reviews = await _yelpGolfCourseReviewservice.GetGolfCourseReviewData(yelpCourseApiKey);
            viewModel.CourseName = courseName;
            viewModel.CourseReviews = reviews;
            return viewModel;
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

        public void RequestAddGolfCourseToSystem(GolfCourse golfCourse)
        {
            _golfCourseRepository.AddNewGolfCourse(golfCourse);
        }
    }
}
