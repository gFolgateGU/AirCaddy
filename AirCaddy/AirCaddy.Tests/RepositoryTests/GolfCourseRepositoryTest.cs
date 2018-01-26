using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirCaddy.Data;
using AirCaddy.Data.Repositories;
using NUnit.Framework;

namespace AirCaddy.Tests.RepositoryTests
{
    [TestFixture]
    public class GolfCourseRepositoryTest
    {
        private GolfCourseRepository _golfCourseRepository;

        [SetUp]
        public void SetUp()
        {
            _golfCourseRepository = new GolfCourseRepository();
        }

        [Test]
        public async Task ShouldAddNewGolfCourseIntoSystem()
        {
            var testGolfCourse = new GolfCourse
            {
                Name = "Country Club of Rochester",
                Address = "2935 East Ave, Rochester, NY 14610",
                PhoneNumber = "5151239812",
                Type = "Private",
                UserId = "19a5ff6e-632f-409a-be75-19528d6408f4"
            };
            await _golfCourseRepository.AddNewGolfCourse(testGolfCourse);
        }

        [TearDown]
        public void TearDown()
        {
            
        }
    }
}
