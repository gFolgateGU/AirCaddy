var makeRequestViewModel = function (antiForgeryRequestToken) {

    var vm = this;

    //used to help prevent cross side scripting.
    vm.requestToken = antiForgeryRequestToken;

    vm.eventName = ko.observable("");
    vm.eventDescription = ko.observable("");
    vm.startDateTime = ko.observable("");
    vm.numberOfHours = ko.observable("");

    //Error Messages
    vm. = ko.observable("");
    vm.eventDescriptionErrorMssg = ko.observable("");
    vm.startDateTimeErrorMssg = ko.observable("");
    vm.numberOfHoursErrorMssg = ko.observable("");

    //Pop Ups
    vm.successPopUpMessage = ko.observable("Your golf course owner request has been submitted successfully.");
    vm.errorPopUpMessage = ko.observable("There was an unexpected error submitting your request to the server.");
    vm.successShow = ko.observable(false);
    vm.errorShow = ko.observable(false);



    vm.submit = function () {
        if (validateEventDetails()) {
            //ajax submit to server.
            var serviceEventModel = {
                EventName: vm.eventName(),
                EventDescription: vm.eventDescription(),
                StartDateTime: vm.startDateTime(),
                NumberOfHours: vm.numberOfHours()
            };

            $.ajax("/Service/SubmitServiceEventAsync",
                {
                    type: "post",
                    data: {
                        __RequestVerificationToken: vm.requestToken,
                        serviceEvent: serviceEventModel
                    },
                    success: function () {
                        resetFields();
                        vm.successShow(true);
                    },
                    error: function () {
                        vm.errorShow(true);
                    }
                });
        }
    }

    vm.errorClose = function () {
        vm.errorShow(false);
    }

    vm.successClose = function () {
        vm.successShow(false);
    }

    function validateEventDetails() {
        resetErrorMessages();
        if (existsNonCompletedFields()) {
            return false;
        }
        else if (!validateDateExpression()) {
            return false;
        } else {
            return true;
        }
    }

    function existsNonCompletedFields() {
        if (vm.eventName() === "") {
            vm.eventNameErrorMssg("Event Name is a required field");
            return true;
        }
        else if (vm.eventDescription() === "") {
            vm.eventDescriptionErrorMssg("Event Description is a required field");
            return true;
        }
        else if (vm.startDateTime() === "") {
            vm.startDateTimeErrorMssg("A Date is required.");
            return true;
        }
        else if (vm.numberOfHours() === "") {
            vm.numberOfHoursErrorMssg("The Number of Hours is a required field");
            return true;
        }

        return false;
    }

    function validateDateExpression() {
        var dateRegex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
        if (!(dateRegex.test(vm.startDateTime()))) {
            vm.startDateTimeErrorMssg("The date must be in form of MM/DD/YYYY");
            return false;
        }
        return true;
    }

    function resetErrorMessages() {
        vm.eventNameErrorMssg("");
        vm.eventDescriptionErrorMssg("");
        vm.numberOfHoursErrorMssg("");
        vm.startDateTimeErrorMssg("");
    }

    function resetFields() {
        vm.eventName("");
        vm.eventDescription("");
        vm.startDateTime("");
        vm.numberOfHours("");
    }
}