var privilege = function(priv, antiForgeryRequestToken) {

    var self = this;

    self.antiForgeryReqToken = antiForgeryRequestToken;
    self.id = priv.CourseId;
    self.courseName = priv.CourseName;
    self.courseAddress = priv.CourseAddress;
    self.courseContact = priv.PrimaryContact;
    self.type = priv.CourseType;

    self.deleteCourse = function() {
        $.ajax("/Privileges/DeleteExistingGolfCoursePrivilege",
            {
                type: "post",
                data: {
                    //__RequestVerificationToken: self.antiForgeryRequestToken,
                    idd: self.id
                },
                success: function (data) {
                    if (data === 1) {
                        alert("The course has been denied.");
                        //course is a duplicate entry
                        //vm.duplicateEntryShow(true);
                        //resetFields();
                    }
                },
                error: function () {

                    //vm.errorShow(true);
                }
            });
    }
}