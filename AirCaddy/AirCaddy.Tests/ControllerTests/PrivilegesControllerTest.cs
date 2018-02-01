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

namespace AirCaddy.Tests.ControllerTests
{
    [TestFixture]
    public class PrivilegesControllerTest
    {
        private Mock<ISessionMapperService> _mockSessionMapperService;
        private Mock<IPrivilegeRequestHandlerService> _mockPrivilegeRequestHandlerService;
        private PrivilegesController _privilegesController;

        [SetUp]
        public void SetUp()
        {
            _mockSessionMapperService = new Mock<ISessionMapperService>();
            _mockPrivilegeRequestHandlerService = new Mock<IPrivilegeRequestHandlerService>();
            _privilegesController = new PrivilegesController(_mockSessionMapperService.Object, _mockPrivilegeRequestHandlerService.Object);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(p => p.HttpContext.Session["email"]).Returns("test@test.com");
            _privilegesController.ControllerContext = controllerContext.Object;
        }

        [Test]
        public void ShouldShowMakeRequestView()
        {
            var result = _privilegesController.MakeRequest();
            Assert.AreNotEqual(result, null);
        }
    }
}
