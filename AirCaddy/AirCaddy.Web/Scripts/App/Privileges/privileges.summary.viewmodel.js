var privilegesSummaryViewModel = function(serverModel, antiForgeryRequestToken) {

    var vm = this;

    vm.myPrivileges = ko.observableArray();
    vm.pendingPrivileges = ko.observableArray();

    vm.noPrivilegesVisible = ko.observable(false);
    vm.noPendingPrivilegesVisible = ko.observable(false);
    vm.privilegesVisible = ko.observable(false);
    vm.pendingPrivilegesVisible = ko.observable(false);

    vm.courseNameInFocus = ko.observable("");
    vm.idInFocus = ko.observable("");
    vm.courseContactInFocus = ko.observable("");
    vm.courseTypeInFocus = ko.observable("");

    init(serverModel);

    function init(serverDataModel) {
        var myPrivileges = [];
        var myPendingPrivileges = [];

        serverDataModel.MyCourses.forEach(function(myPrivilege) {
            myPrivileges.push(new privilege(myPrivilege, antiForgeryRequestToken));
        });

        serverDataModel.MyPendingCourses.forEach(function(pendingPrivilege) {
            myPendingPrivileges.push(new privilege(pendingPrivilege, antiForgeryRequestToken));
        });

        if (myPrivileges.length < 1) {
            vm.noPrivilegesVisible(true);
        } else {
            vm.privilegesVisible(true);
        }

        if (myPendingPrivileges.length < 1) {
            vm.noPendingPrivilegesVisible(true);
        } else {
            vm.pendingPrivilegesVisible(true);
        }

        vm.myPrivileges(myPrivileges);
        vm.pendingPrivileges(myPendingPrivileges);
    }

    vm.showDeleteCourse = function (id, courseName) {
        vm.idInFocus(id);
        vm.courseNameInFocus(courseName);
        $("#deletePopUp").modal('show');
    }

    vm.showEditCourse = function (id, courseName, coursePhone, courseType) {
        vm.idInFocus(id);
        vm.courseNameInFocus(courseName);
        vm.courseContactInFocus(coursePhone);
        vm.courseTypeInFocus(courseType);
        $("#editPopUp").modal('show');
    }

    self.deleteCourse = function () {
        $.ajax("/Privileges/DeleteExistingGolfCoursePrivilege",
            {
                type: "post",
                data: {
                    //__RequestVerificationToken: self.antiForgeryRequestToken,
                    id: vm.idInFocus()
                },
                success: function (data) {
                    if (data === 1) {
                        alert("You must be signed in.");
                    }
                    else if (data === 2) {
                        alert("You do not own that course");
                    }
                    else if (data === true) {
                        alert("The golf course has been deleted.");
                        window.location.reload();
                    }
                    else if (data === false) {
                        alert("There was an error deleting the golf course.");
                    }
                },
                error: function () {
                    //vm.errorShow(true);
                }
            });
    }
}