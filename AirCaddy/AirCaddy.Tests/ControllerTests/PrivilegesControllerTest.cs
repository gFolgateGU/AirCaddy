using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using AirCaddy.Domain.Services;
using AirCaddy.Domain.Services.Privileges;
using AirCaddy.Controllers;
using AirCaddy.Domain.Services.GolfCourses;
using AirCaddy.Domain.Special;
using AirCaddy.Domain.ViewModels.GolfCourses;

namespace AirCaddy.Tests.ControllerTests
{
    [TestFixture]
    public class PrivilegesControllerTest
    {
        /*private Mock<ISessionMapperService> _mockSessionMapperService;
        private Mock<IPrivilegeRequestHandlerService> _mockPrivilegeRequestHandlerService;
        private Mock<IGolfCourseService> _mockGolfCourseService;
        private Mock<ICourseBuilder> _mockCourseBuilder;
        private PrivilegesController _privilegesController;

        [SetUp]
        public void SetUp()
        {
            _mockSessionMapperService = new Mock<ISessionMapperService>();
            _mockPrivilegeRequestHandlerService = new Mock<IPrivilegeRequestHandlerService>();
            _mockGolfCourseService = new Mock<IGolfCourseService>();
            _mockCourseBuilder = new Mock<ICourseBuilder>();
            _privilegesController = new PrivilegesController(_mockSessionMapperService.Object, _mockPrivilegeRequestHandlerService.Object,
                _mockCourseBuilder.Object, _mockGolfCourseService.Object);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(p => p.HttpContext.Session["email"]).Returns("test@test.com");
            _privilegesController.ControllerContext = controllerContext.Object;
        }

        [Test]
        public void ShouldShowMakeRequestView()
        {
            var result = _privilegesController.MakeRequest();
            Assert.AreNotEqual(result, null);
        }*/

        [Test]
        public async Task YoutubeTest()
        {
            var testObj = new YoutubeGolfService();
            var testUpload = new UploadCourseViewModel
            {
                CourseId = "1",
                CourseName = "Elk Valley Golf Course",
                HoleNumber = 3,
                HoleVideoPath = @"C:\Users\gfolg\Desktop\SampleGolfCourseHole.mp4"
            };
            await testObj.UploadCourseFootageAsync(testUpload);
        }
    }
}
