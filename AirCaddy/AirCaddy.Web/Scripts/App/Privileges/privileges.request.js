var request = function(data) {
    var self = this;
    
    self.id = data.Id;
    self.username = data.UserName;
    self.courseName = data.GolfCourseName;
    self.courseAddress = data.GolfCourseAddress;
    self.phoneNumber = data.CoursePhoneNumber;
    self.courseType = data.GolfCourseType;
    self.reason = data.Reason;
    self.verified = data.Verified;
}