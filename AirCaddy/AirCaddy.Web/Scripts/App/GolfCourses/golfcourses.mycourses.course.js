var course = function(courseData) {
    var self = this;

    self.id = courseData.Id;
    self.courseName = courseData.CourseName;
    self.courseAddress = courseData.CourseAddress;
    self.coursePrimaryContact = courseData.CoursePrimaryContact;
    self.courseType = courseData.CourseType;
    self.courseOwnerId = courseData.CourseOwnerId;

    self.manageCourseLink = function (courseId) {
        var url = '/GolfCourses/ManageMyCourse/?courseId=' + courseId;
        window.location.href = url;
    }
}