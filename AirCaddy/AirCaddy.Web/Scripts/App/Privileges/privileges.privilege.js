var privilege = function(priv, antiForgeryRequestToken) {

    var self = this;

    self.antiForgeryReqToken = antiForgeryRequestToken;
    self.id = priv.CourseId;
    self.courseName = priv.CourseName;
    self.courseAddress = priv.CourseAddress;
    self.courseContact = priv.PrimaryContact;
    self.type = priv.CourseType;
}