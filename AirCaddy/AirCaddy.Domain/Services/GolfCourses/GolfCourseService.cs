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

        void RequestAddGolfCourseToSystem(Tuple<GolfCourse, List<GolfCourseVideo>> golfCourseWithDefaultVideos);

        Task<CourseOverviewViewModel> GetCourseOverviewViewModel(int courseId);

        Task<IEnumerable<GolfCourseViewModel>> GetMyCourses(string userId);

        Task<GolfCourse> RequestCourseOwnedByUser(int courseId, string userId);

        Task<ManageCourseViewModel> GetManageCourseViewModel(int golfCourseId);

        UploadCourseVideoViewModel GetGolfCourseUploadViewModel(int courseId, int holeNumber, string videoPath);

        Task StoreCourseFootageForHole(UploadCourseVideoViewModel successfulCourseFootageUpload);

        Task<string> RequestVideoIdAssociatedWithGolfCourseHole(int courseId, int holeNumber);

        Task RequestVideoIdDeletion(string youtubeVideoId);

        Task<VirtualTourViewModel> GetVirtualTourViewModel(int courseId);
    }

    public class GolfCourseService : IGolfCourseService
    {
        private readonly IGolfCourseRepository _golfCourseRepository;
        private readonly IYelpGolfCourseReviewService _yelpGolfCourseReviewservice;

        public GolfCourseService(IGolfCourseRepository golfCourseRepository, IYelpGolfCourseReviewService yelpGolfCourseReviewservice)
        {
            _golfCourseRepository = golfCourseRepository;
            _yelpGolfCourseReviewservice = yelpGolfCourseReviewservice;
        }

        public async Task<GolfCourse> RequestCourseOwnedByUser(int courseId, string userId)
        {
            return await _golfCourseRepository.GetCourseOwnedByUser(courseId, userId);
        }

        public async Task<ManageCourseViewModel> GetManageCourseViewModel(int golfCourseId)
        {
            var manageCourseViewModel = new ManageCourseViewModel();

            var golfCourseWithVideosInfo = await _golfCourseRepository.GetGolfCourseAndCourseVideoInfo(golfCourseId);

            if (golfCourseWithVideosInfo == null)
            {
                return null;
            }

            manageCourseViewModel.GolfCourseId = golfCourseWithVideosInfo.Item1.Id;
            manageCourseViewModel.GolfCourseName = golfCourseWithVideosInfo.Item1.Name;
            manageCourseViewModel.GolfCoursePhone = golfCourseWithVideosInfo.Item1.PhoneNumber;
            manageCourseViewModel.GolfCourseAddress = golfCourseWithVideosInfo.Item1.Address;
            manageCourseViewModel.GolfCourseType = golfCourseWithVideosInfo.Item1.Type;
            manageCourseViewModel.GolfCourseOwnerId = golfCourseWithVideosInfo.Item1.UserId;
            manageCourseViewModel.GolfCourseHoleVideos =
                (List<CourseVideoViewModel>) MapVideoDataToVideoViewModelList(golfCourseWithVideosInfo.Item2);

            return manageCourseViewModel;
        }

        public async Task<IEnumerable<GolfCourseViewModel>> GetMyCourses(string userId)
        {
            var myCourses = await _golfCourseRepository.GetGolfCoursesOwnedByUserAsync(userId);
            var myCoursesVm = MapGolfEntityModelToGolfViewModel(myCourses);
            return myCoursesVm;
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

        public void RequestAddGolfCourseToSystem(Tuple<GolfCourse, List<GolfCourseVideo>> golfCourseWithDefaultVideos)
        {
            _golfCourseRepository.AddNewGolfCourseWithDefaultVideos(golfCourseWithDefaultVideos);
        }

        public UploadCourseVideoViewModel GetGolfCourseUploadViewModel(int courseId, int holeNumber, string videoPath)
        {
            var courseName = _golfCourseRepository.GetGolfCourseName(courseId);
            return new UploadCourseVideoViewModel
            {
                CourseId = courseId,
                CourseName = courseName,
                HoleNumber = holeNumber,
                HoleVideoPath = videoPath,
                YouTubeVideoId = string.Empty
            };
        }

        public async Task StoreCourseFootageForHole(UploadCourseVideoViewModel successfulCourseFootageUpload)
        {
            await _golfCourseRepository.AddCourseFootageForHole(successfulCourseFootageUpload.CourseId,
                successfulCourseFootageUpload.HoleNumber, successfulCourseFootageUpload.YouTubeVideoId);
        }

        public async Task<string> RequestVideoIdAssociatedWithGolfCourseHole(int courseId, int holeNumber)
        {
            return await _golfCourseRepository.GetCourseFootageYouTubeVideoIdForHole(courseId, holeNumber);
        }

        public async Task RequestVideoIdDeletion(string youtubeVideoId)
        {
            await _golfCourseRepository.DeleteCourseFootageId(youtubeVideoId);
        }

        public async Task<VirtualTourViewModel> GetVirtualTourViewModel(int courseId)
        {
            var virtualTourViewModel = new VirtualTourViewModel();
            var golfCourseHoleVideosAndGenInfo = await _golfCourseRepository.GetGolfCourseAndCourseVideoInfo(courseId);
            var golfCourseHoleReviews = await _golfCourseRepository.GetGolfCourseReviews(courseId);

            virtualTourViewModel.GolfCourseId = golfCourseHoleVideosAndGenInfo.Item1.Id;
            virtualTourViewModel.GolfCourseName = golfCourseHoleVideosAndGenInfo.Item1.Name;
            virtualTourViewModel.GolfCoursePhone = golfCourseHoleVideosAndGenInfo.Item1.PhoneNumber;
            virtualTourViewModel.GolfCourseAddress = golfCourseHoleVideosAndGenInfo.Item1.Address;
            virtualTourViewModel.GolfCourseType = golfCourseHoleVideosAndGenInfo.Item1.Type;
            virtualTourViewModel.GolfCourseOwnerId = golfCourseHoleVideosAndGenInfo.Item1.UserId;

            virtualTourViewModel.GolfCourseHoleVideos = (List<CourseVideoViewModel>) 
                MapVideoDataToVideoViewModelList(golfCourseHoleVideosAndGenInfo.Item2);
            virtualTourViewModel.GolfCourseHoleRatings = (List<GolfCourseHoleRatingViewModel>) 
                MapCourseReviewDataToViewModelList(golfCourseHoleReviews);

            return virtualTourViewModel;
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

        private IEnumerable<CourseVideoViewModel> MapVideoDataToVideoViewModelList(
            IEnumerable<GolfCourseVideo> courseVideoData)
        {
            return courseVideoData.Select(courseVideo => new CourseVideoViewModel
                {
                    VideoId = courseVideo.Id,
                    CourseHoleNumber = courseVideo.HoleNumber,
                    YouTubeVideoId = courseVideo.YoutubeHoleVideoId,
                    CourseId = courseVideo.GolfCourseId
                })
                .ToList();
        }

        private IEnumerable<GolfCourseHoleRatingViewModel> MapCourseReviewDataToViewModelList(
            IEnumerable<GolfCourseComment> golfCourseHoleRatings)
        {
            return golfCourseHoleRatings.Select(golfCourseHoleRating => new GolfCourseHoleRatingViewModel
            {
                Id = golfCourseHoleRating.Id,
                Comment = golfCourseHoleRating.HoleComment,
                Difficulty = golfCourseHoleRating.DifficultyRating,
                GolfCourseId = golfCourseHoleRating.GolfCourseId,
                HoleNumber = golfCourseHoleRating.HoleNumber,
                Username = golfCourseHoleRating.UserId
            })
            .ToList();
        }
    }
}
