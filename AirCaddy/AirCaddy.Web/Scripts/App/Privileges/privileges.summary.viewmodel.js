var privilegesSummaryViewModel = function(serverModel, antiForgeryRequestToken) {

    console.log("boss");
    console.log(antiForgeryRequestToken);

    var vm = this;

    vm.myPrivileges = ko.observableArray();
    vm.pendingPrivileges = ko.observableArray();

    vm.noPrivilegesVisible = ko.observable(false);
    vm.noPendingPrivilegesVisible = ko.observable(false);
    vm.privilegesVisible = ko.observable(false);
    vm.pendingPrivilegesVisible = ko.observable(false);
    vm.courseNameInFocus = ko.observable("");
    vm.idInFocus = ko.observable("");

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
}