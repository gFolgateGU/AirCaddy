var makeRequestViewModel = function (antiForgeryRequestToken) {

    var vm = this;

    //used to help prevent cross side scripting.
    vm.requestToken = antiForgeryRequestToken;

    var stateCodes = [
        '--', 'AL', 'AK', 'AS', 'AZ', 'AR', 'CA', 'CO',
        'CT', 'DE', 'DC', 'FM', 'FL', 'GA', 'GU', 'HI', 'ID', 'IL', 'IN',
        'IA', 'KS', 'KY', 'LA', 'ME', 'MH', 'MD', 'MI', 'MN', 'MS', 'MO', 'MT',
        'NE', 'NV', 'NH', 'NH', 'NJ', 'NM', 'NY', 'NC', 'ND', 'MP', 'OH', 'OK', 'OR', 'PW',
        'PA', 'PR', 'RI', 'SC', 'SD', 'TN', 'TX', 'UT', 'VT', 'VI', 'VA', 'WA', 'WV', 'WI', 'WY'
    ];

    vm.courseName = ko.observable("");
    vm.coursePrimaryContact = ko.observable("");
    vm.courseReason = ko.observable("");
    vm.states = ko.observableArray(stateCodes);
    vm.selectedState = ko.observable();

    //Error Messages
    vm.courseNameErrorMssg = ko.observable("");
    vm.coursePrimaryContactErrorMssg = ko.observable("");
    vm.courseAddressLineErrorMssg = ko.observable("");
    vm.courseCityErrorMssg = ko.observable("");
    vm.courseStateCodeErrorMssg = ko.observable("");
    vm.courseZipErrorMssg = ko.observable("");
    vm.courseTypeErrorMssg = ko.observable("");
    vm.courseReasonErrorMssg = ko.observable("");

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

    function resetErrorMessages() {
        vm.courseNameErrorMssg = ko.observable("");
        vm.coursePrimaryContactErrorMssg = ko.observable("");
        vm.courseAddressLineErrorMssg = ko.observable("");
        vm.courseCityErrorMssg = ko.observable("");
        vm.courseStateCodeErrorMssg = ko.observable("");
        vm.courseZipErrorMssg = ko.observable("");
        vm.courseTypeErrorMssg = ko.observable("");
        vm.courseReasonErrorMssg = ko.observable("");
    }

    function resetFields() {
        vm.eventName("");
        vm.eventDescription("");
        vm.startDateTime("");
        vm.numberOfHours("");
    }

    
}