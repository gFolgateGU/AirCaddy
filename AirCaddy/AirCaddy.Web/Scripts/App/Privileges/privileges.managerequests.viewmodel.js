var manageRequestsViewModel = function(serverModel) {
    var vm = this;

    vm.requestsList = ko.observableArray(init(serverModel));
    vm.reasonInFocus = ko.observable("");
    vm.courseNameInFocus = ko.observable("");

    function init(serverData) {
        var requestList = [];
        serverData.forEach(function (privilegeRequestEntry) {
            console.log(privilegeRequestEntry);
            var privRequest = new request(privilegeRequestEntry);
            requestList.push(privRequest);
        });
        return requestList;
    }

    vm.showReason = function(reason, courseName) {
        vm.reasonInFocus(reason);
        vm.courseNameInFocus(courseName);
        $("#reasonPopUp").modal('show');
    }
}