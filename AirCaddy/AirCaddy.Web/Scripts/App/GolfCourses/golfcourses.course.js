var course = function (data, requestUrl) {

    var self = this;

    self.id = data.Id;
    self.name = data.CourseName;
    self.address = data.CourseAddress;
    self.phoneNumber = data.CoursePrimaryContact;
    self.type = data.CourseType;
    self.userId = data.CourseOwnerId;

    self.exploreRequestUrl = requestUrl;

    console.log(self.exploreRequestUrl);

    self.exploreCourse = function ()
    {
        //window.location.href = '/GolfCourses/Explore/?golfCourseId=' + self.id;
        window.location.href = self.exploreRequestUrl + "?golfCourseId=" + self.id;
    }
}