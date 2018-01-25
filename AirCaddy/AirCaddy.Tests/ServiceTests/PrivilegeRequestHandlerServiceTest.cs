using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AirCaddy.Domain.Services.Privileges;
using Moq;
using NUnit.Framework;
using AirCaddy.Data.Repositories;
using AirCaddy.Domain.ViewModels.Privileges;

namespace AirCaddy.Tests.ServiceTests
{
    [TestFixture]
    public class PrivilegeRequestHandlerServiceTest
    {
        private Mock<IGolfCourseRepository> _mockGolfCourseRepository;
        private Mock<IPrivilegeRepository> _mockPrivilegeRepository;
        private string _uspsUserId;
        private PrivilegeRequestHandlerService _privilegeRequestHandlerService;

        [SetUp]
        public void SetUp()
        {
            _mockGolfCourseRepository = new Mock<IGolfCourseRepository>();
            _mockPrivilegeRepository = new Mock<IPrivilegeRepository>();
            _uspsUserId = "088GANNO3834";
            _privilegeRequestHandlerService = new PrivilegeRequestHandlerService(_mockPrivilegeRepository.Object,
                _mockGolfCourseRepository.Object, _uspsUserId);
        }

        [Test]
        public async Task ShouldNotBeADuplicateEntry()
        {
            var validVm = new PrivilegeRequestViewModel
            {
                CourseName = "Elk Valley Golf Course",
                CoursePhoneNumber = "7771113333",
                CourseType = "Public",
                CourseAddress = "7085 Van Camp Rd",
                City = "Girard",
                StateCode = "PA",
                Zip = "16417"
            };

            _mockGolfCourseRepository.Setup(gcr => gcr.ExistsGolfCourseEntryAsync(validVm.CourseName, validVm.CourseAddress))
                .ReturnsAsync(false);
            _mockPrivilegeRepository.Setup(pr => pr.ExistsCourseRequestAsync(validVm.CourseName, validVm.CourseAddress))
                .ReturnsAsync(false);
            var result = await _privilegeRequestHandlerService.IsDuplicateEntryAsync(validVm);
            Assert.AreEqual(result, false);
        }

        [TestCase(false, true, true)]
        [TestCase(true, false, true)]
        [TestCase(true, true, true)]
        public async Task ShouldBeADuplicateEntry(bool existsRequestEntry, bool existsCourse, bool actual)
        {
            var validVm = new PrivilegeRequestViewModel
            {
                CourseName = "Elk Valley Golf Course",
                CoursePhoneNumber = "7771113333",
                CourseType = "Public",
                CourseAddress = "7085 Van Camp Rd",
                City = "Girard",
                StateCode = "PA",
                Zip = "16417"
            };

            _mockGolfCourseRepository.Setup(gcr => gcr.ExistsGolfCourseEntryAsync(validVm.CourseName, validVm.CourseAddress))
                .ReturnsAsync(existsRequestEntry);
            _mockPrivilegeRepository.Setup(pr => pr.ExistsCourseRequestAsync(validVm.CourseName, validVm.CourseAddress))
                .ReturnsAsync(existsCourse);
            var result = await _privilegeRequestHandlerService.IsDuplicateEntryAsync(validVm);
            Assert.AreEqual(result, actual);
        }

        [Test]
        public void ShouldSucceedBecauseValidGolfCourseAddressWasSupplied()
        {
            var validVm = new PrivilegeRequestViewModel
            {
                CourseName = "Elk Valley Golf Course",
                CoursePhoneNumber = "7771113333",
                CourseType = "Public",
                CourseAddress = "7085 Van Camp Rd",
                City = "Girard",
                StateCode = "PA",
                Zip = "16417"
            };

            var result = _privilegeRequestHandlerService.ValidateGolfCourseAddress(validVm);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void ShouldNotSucceedBecauseInvalidGolfCourseAddressWasSupplied()
        {
            var invalidVm = new PrivilegeRequestViewModel
            {
                CourseName = "Elk Valley Golf Course",
                CoursePhoneNumber = "7771113333",
                CourseType = "Public",
                CourseAddress = "69 Dong Dude Drive",
                City = "Girard",
                StateCode = "PA",
                Zip = "16417"
            };

            var result = _privilegeRequestHandlerService.ValidateGolfCourseAddress(invalidVm);
            Assert.AreEqual(false, result);
        }
    }
}
