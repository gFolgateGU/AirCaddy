var manageRequestsViewModel = function(serverModel) {
    var vm = this;

    vm.requestsList = ko.observableArray(init(serverModel));

    function init(serverData) {
        var requestList = [];
        serverData.forEach(function (privilegeRequestEntry) {
            console.log(privilegeRequestEntry);
            var privRequest = new request(privilegeRequestEntry);
            requestList.push(privRequest);
        });
        return requestList;
    }
}