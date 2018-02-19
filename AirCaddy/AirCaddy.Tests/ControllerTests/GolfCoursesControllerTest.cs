using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirCaddy.Controllers;
using AirCaddy.Domain.Services;
using AirCaddy.Domain.Services.GolfCourses;
using AirCaddy.Domain.ViewModels.GolfCourses;
using Moq;
using NUnit.Framework;

namespace AirCaddy.Tests.ControllerTests
{
    [TestFixture]
    public class GolfCoursesControllerTest
    {
        private Mock<IYelpGolfCourseReviewService> _mockYelpGolfCourseReviewService;
        private Mock<IGolfCourseService> _mockGolfCourseService;
        private Mock<ISessionMapperService> _mockSessionMapperService;
        private Mock<IYoutubeGolfService> _mockYoutubeGolfService;
        private GolfCoursesController _golfCoursesController;

        [SetUp]
        public void SetUp()
        {
            _mockYelpGolfCourseReviewService = new Mock<IYelpGolfCourseReviewService>();
            _mockGolfCourseService = new Mock<IGolfCourseService>();
            _mockSessionMapperService = new Mock<ISessionMapperService>();
            _mockYoutubeGolfService = new Mock<IYoutubeGolfService>();
            _golfCoursesController = new GolfCoursesController(_mockYelpGolfCourseReviewService.Object, _mockGolfCourseService.Object,
                _mockSessionMapperService.Object, _mockYoutubeGolfService.Object);
        }

        [Test]
        public void ShouldUploadCourseFootage()
        {
            var testObject = new YoutubeGolfService();
            var uploadedCourse = new UploadCourseViewModel
            {
                CourseId = "123",
                GolfCourseName = "Elk Valley Golf Course",
                HoleNumber = "10",
                VideoFilePath = @"C:\Users\gfolg\Desktop\SampleGolfCourseHole.mp4"
            };
            var result = testObject.UploadCourseFootage(uploadedCourse);
            Assert.AreEqual(result, true);
        }
    }
}
