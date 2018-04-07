var privilegesSummaryViewModel = function(serverModel) {

    var vm = this;

    vm.myPrivileges = ko.observableArray();
    vm.pendingPrivileges = ko.observableArray();

    vm.noPrivilegesVisible = ko.observable(false);
    vm.noPendingPrivilegesVisible = ko.observable(false);
    vm.privilegesVisible = ko.observable(false);
    vm.pendingPrivilegesVisible = ko.observable(false);

    init(serverModel);

    function init(serverDataModel) {
        var myPrivileges = [];
        var myPendingPrivileges = [];

        serverDataModel.MyCourses.forEach(function(myPrivilege) {
            myPrivileges.push(new privilege(myPrivilege));
        });

        serverDataModel.MyPendingCourses.forEach(function(pendingPrivilege) {
            myPendingPrivileges.push(new privilege(pendingPrivilege));
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
}