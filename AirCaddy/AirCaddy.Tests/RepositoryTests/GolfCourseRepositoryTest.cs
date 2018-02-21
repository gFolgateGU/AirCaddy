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
        /*public async Task ShouldAddNewGolfCourseIntoSystem()
        {
            var testGolfCourse = new GolfCourse
            {
                Name = "Irondequoit Country Club",
                Address = "4045 East Ave, Rochester, NY 14618",
                PhoneNumber = "5151234422",
                Type = "Private",
                UserId = "19a5ff6e-632f-409a-be75-19528d6408f4"
            };
            await _golfCourseRepository.AddNewGolfCourseWithDefaultVideos(testGolfCourse);
        }*/

        [TearDown]
        public void TearDown()
        {
            
        }
    }
}
