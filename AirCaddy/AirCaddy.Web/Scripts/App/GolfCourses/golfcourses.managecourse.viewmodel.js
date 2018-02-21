var manageCourseViewModel = function (serverModel) {

    var vm = this;

    vm.video = ko.observable(null);

    vm.holeInFocus = ko.observable(1);

    vm.changeHoleInFocus = function(newHoleNumber) {
        vm.holeInFocus(newHoleNumber);
    }

    vm.uploadVideo = function(file) {
        var fd = new FormData();
        fd.append("file", file);

        //'/GolfCourses/UploadFile?id=' + myID,

        $.ajax({
            type: "POST",
            url: '/GolfCourses/UploadCourseFootage',
            contentType: false,
            processData: false,
            data: fd,
            success: function (result) {
                console.log(result);
            },
            error: function () {
                console.log("help me");
            }
        });
    }
}