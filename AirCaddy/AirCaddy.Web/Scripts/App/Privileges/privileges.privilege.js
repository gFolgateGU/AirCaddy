var privilege = function(priv, antiForgeryRequestToken, manageRequestLink) {

    var self = this;

    self.antiForgeryReqToken = antiForgeryRequestToken;
    self.id = priv.CourseId;
    self.courseName = priv.CourseName;
    self.courseAddress = priv.CourseAddress;
    self.courseContact = priv.PrimaryContact;
    self.type = priv.CourseType;

    self.manageRequestLinkBase = manageRequestLink;

    self.manageMyCourse = function() {
        var url = self.manageRequestLinkBase + "?courseId=" + self.id;
        window.location.href = url;
    }
}