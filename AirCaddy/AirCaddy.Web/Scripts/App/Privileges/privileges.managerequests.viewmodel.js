var manageRequestsViewModel = function(serverModel, antiForgeryToken) {
    var vm = this;

    //used to help prevent cross side scripting.
    vm.requestToken = antiForgeryToken;

    vm.requestsList = ko.observableArray(init(serverModel));
    vm.reasonInFocus = ko.observable("");
    vm.courseNameInFocus = ko.observable("");
    vm.idInFocus = ko.observable("");
    vm.requestType = ko.observable("");

    function init(serverData) {
        var requestList = [];
        serverData.forEach(function (privilegeRequestEntry) {
            console.log(privilegeRequestEntry);
            var privRequest = new request(privilegeRequestEntry);
            requestList.push(privRequest);
        });
        return requestList;
    }

    vm.showReason = function (reason, courseName) {
        vm.reasonInFocus(reason);
        vm.courseNameInFocus(courseName);
        $("#reasonPopUp").modal('show');
    }

    vm.showDeny = function (id, courseName, typeOfRequest) {
        vm.requestType(typeOfRequest);
        vm.idInFocus(id);
        vm.courseNameInFocus(courseName);
        $("#denyPopUp").modal('show');
    }

    vm.showAccept = function (id, courseName, typeOfRequest) {
        vm.requestType(typeOfRequest);
        vm.idInFocus(id);
        vm.courseNameInFocus(courseName);
        $("#acceptPopUp").modal('show');
    }

    vm.denyRequest = function (id) {
        $.ajax("/Privileges/DenyRequest",
            {
                type: "post",
                data: {
                    __RequestVerificationToken: vm.requestToken,
                    id: vm.idInFocus()
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
                    alert("Yikes we fucked up man");
                    //vm.errorShow(true);
                }
            });

        vm.acceptRequest = function (id) {
            $.ajax("/Privileges/AcceptRequest",
                {
                    type: "post",
                    data: {
                        __RequestVerificationToken: vm.requestToken,
                        id: vm.idInFocus()
                    },
                    success: function (data) {
                        if (data === 1) {
                            alert("The course has been accepted.");
                            //course is a duplicate entry
                            //vm.duplicateEntryShow(true);
                            //resetFields();
                        }
                    },
                    error: function () {
                        alert("Yikes we fucked up man");
                        //vm.errorShow(true);
                    }
                });

        }
    }
}
    