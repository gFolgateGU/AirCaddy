var course = function(courseData, requestUrl) {
    var self = this;

    self.id = courseData.Id;
    self.courseName = courseData.CourseName;
    self.courseAddress = courseData.CourseAddress;
    self.coursePrimaryContact = courseData.CoursePrimaryContact;
    self.courseType = courseData.CourseType;
    self.courseOwnerId = courseData.CourseOwnerId;
    self.manageCourseUrl = requestUrl;

    self.manageCourseLink = function (courseId) {
        var url = self.manageCourseUrl + "?courseId=" + courseId;
        window.location.href = url;
    }
}