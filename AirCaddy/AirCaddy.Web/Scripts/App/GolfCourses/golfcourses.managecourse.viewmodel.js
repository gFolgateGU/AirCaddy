var manageCourseViewModel = function (serverModel) {

    var vm = this;

    vm.video = ko.observable(null);

    vm.uploadVideo = function(file) {
        console.log("weird dude here!");

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