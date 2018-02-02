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

    //Fields
    vm.courseName = ko.observable("");
    vm.coursePrimaryContact = ko.observable("");
    vm.courseReason = ko.observable("");
    vm.states = ko.observableArray(stateCodes);
    vm.addressLine = ko.observable("");
    vm.selectedState = ko.observable();
    vm.city = ko.observable("");
    vm.zip = ko.observable("");
    vm.courseType = ko.observable("Public");
    vm.reason = ko.observable("");

    //Error Messages
    vm.badField = false;
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
    vm.badAddressPopUpMessage =
        ko.observable("The Address provided is not valid.  Please double check for any mistakes.");
    vm.duplicateEntryPopUpMessage = ko.observable("This course has already been added to the system.");
    vm.successShow = ko.observable(false);
    vm.errorShow = ko.observable(false);
    vm.badAddressShow = ko.observable(false);
    vm.duplicateEntryShow = ko.observable(false);

    vm.submit = function () {
        if (validateReasonDetails()) {
            //ajax submit to server.
            var privilegeRequestModel = {
                CourseName: vm.courseName(),
                CourseAddress: vm.addressLine(),
                City: vm.city(),
                StateCode: vm.selectedState(),
                Zip: vm.zip(),
                CoursePhoneNumber: vm.coursePrimaryContact(),
                CourseType: vm.courseType(),
                Reason: vm.reason()
            };

            $.ajax("/Privileges/MakeRequest",
                {
                    type: "post",
                    data: {
                        __RequestVerificationToken: vm.requestToken,
                        privilegeData: privilegeRequestModel
                    },
                    success: function (data) {
                        if (data === 1) {
                            //course is a duplicate entry
                            vm.duplicateEntryShow(true);
                            resetFields();
                        }
                        else if (data === 2) {
                            vm.badAddressPopUpMessage("The Address provided is not valid.  Please double check for any mistakes.");
                            vm.badAddressShow(true);
                        } else {
                            resetFields();
                            vm.successShow(true);
                        }
                    },
                    error: function() {
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

    vm.badAddressClose = function() {
        vm.badAddressShow(false);
    }

    vm.duplicateEntryClose = function () {
        vm.duplicateEntryShow(false);
    }


    function validateReasonDetails() {
        resetErrorMessages();
        if (existsNonCompletedFields()) {
            return false;
        }
        return true;
    }

    function existsNonCompletedFields() {
        if (vm.courseName() === "") {
            vm.courseNameErrorMssg("A course name is required.");
            vm.badField = true;
        }
        if (vm.coursePrimaryContact() === "") {
            vm.coursePrimaryContactErrorMssg("A primary contact is required.");
            vm.badField = true;
        }
        if (vm.addressLine() === "") {
            vm.courseAddressLineErrorMssg("You must enter the course address.");
            vm.badField = true;
        }
        if (vm.city() === "") {
            vm.courseCityErrorMssg("You must enter a city.");
            vm.badField = true;
        }
        if (vm.zip() === "") {
            vm.courseZipErrorMssg("You must enter a zip code.");
            vm.badField = true;
        }
        if (vm.selectedState() === "" || vm.selectedState() === "--") {
            vm.courseStateCodeErrorMssg("You must select a state.");
            vm.badField = true;
        }
        if (vm.reason().length < 20) {
            vm.courseReasonErrorMssg("The reason must be at least 20 characters long.");
            vm.badField = true;
        }
        if (vm.badField) {
            return true;
        }
        return false;
    }

    function resetErrorMessages() {
        vm.badField = false;
        vm.courseNameErrorMssg("");
        vm.coursePrimaryContactErrorMssg("");
        vm.courseAddressLineErrorMssg("");
        vm.courseCityErrorMssg("");
        vm.courseStateCodeErrorMssg("");
        vm.courseZipErrorMssg("");
        vm.courseTypeErrorMssg("");
        vm.courseReasonErrorMssg("");
    }

    function resetFields() {
        vm.courseName("");
        vm.coursePrimaryContact("");
        vm.courseReason("");
        vm.addressLine("");
        vm.selectedState();
        vm.city("");
        vm.zip("");
        vm.reason("");
    }   
}