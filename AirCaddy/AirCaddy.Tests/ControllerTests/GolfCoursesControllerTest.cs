using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AirCaddy.Controllers;
using AirCaddy.Domain.Services;
using AirCaddy.Domain.Services.GolfCourses;
using NUnit.Framework;
using Moq;

namespace AirCaddy.Tests.ControllerTests
{
    [TestFixture]
    public class GolfCoursesControllerTest
    {
        private Mock<IYelpGolfCourseReviewService> _mockYelpGolfCourseReviewService;
        private Mock<IGolfCourseService> _mockGolfCourseService;
        private Mock<ISessionMapperService> _mockSessionMapperService;
        private IYoutubeGolfService _youtubeGolfService;
        private GolfCoursesController _golfCoursesController;

        [SetUp]
        public void SetUp()
        {
            _mockYelpGolfCourseReviewService = new Mock<IYelpGolfCourseReviewService>();
            _mockGolfCourseService = new Mock<IGolfCourseService>();
            _mockSessionMapperService = new Mock<ISessionMapperService>();
            _youtubeGolfService = new YoutubeGolfService();

            _golfCoursesController = new GolfCoursesController(_mockYelpGolfCourseReviewService.Object,
                _mockGolfCourseService.Object, _mockSessionMapperService.Object, _youtubeGolfService);

            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(p => p.HttpContext.Session["Username"]).Returns("test@test.com");
            _golfCoursesController.ControllerContext = controllerContext.Object;
        }

        [Test]
        public async Task ShouldDeleteCourseFootageVideoForHoleFromYouTube()
        {
            const string fakeUserId = "fds21-dfsj32-kfdjs2-234ja-cv22j";
            const string youtubeVideoId = "f2kuqBO-1uA";
            _mockSessionMapperService
                .Setup(msmr =>
                    msmr.MapUserIdFromSessionUsername(_golfCoursesController.ControllerContext.HttpContext
                        .Session["Username"].ToString())).Returns(fakeUserId);
            _mockGolfCourseService
                .Setup(mgcs => mgcs.RequestVideoIdAssociatedWithGolfCourseHole(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(youtubeVideoId);

            var response = await _golfCoursesController.Modify(10, 17);
            Assert.AreEqual(null, response);
        }
    }
}
