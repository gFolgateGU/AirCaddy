var course = function(data) {

    var self = this;

    self.id = data.Id;
    self.name = data.CourseName;
    self.address = data.CourseAddress;
    self.phoneNumber = data.CoursePrimaryContact;
    self.type = data.CourseType;
    self.userId = data.CourseOwnerId;
}