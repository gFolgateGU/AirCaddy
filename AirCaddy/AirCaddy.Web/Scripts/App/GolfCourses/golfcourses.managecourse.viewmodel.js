var manageCourseViewModel = function (serverModel) {

    var vm = this;

    //constants
    var uploadAreaSpinner = document.getElementById("uploadSpinArea");
    var youtubeEmbeddedUrlBase = "https://www.youtube.com/embed/";

    //Main Attributes
    vm.video = ko.observable(null);
    vm.holeInFocus = ko.observable(1);
    vm.courseId = ko.observable(serverModel.GolfCourseId);
    vm.courseName = ko.observable(serverModel.GolfCourseName);
    vm.uploadedCourseFile = ko.observable(null);
    vm.currentHoleYoutubeApiSrc = ko.observable("https://www.youtube.com/embed/xdc1H9-XUrk");

    //Visible Trigger Attributes
    vm.submitCourseFootageUploadAreaVisible = ko.observable(false);
    vm.uploadCourseFootageAreaVisible = ko.observable(true);
    vm.uploadingAreaVisible = ko.observable(false);
    vm.manageAreaVisible = ko.observable(false);

    //Additional UI Elements
    vm.videoUploadSpinner = new Spinner();

    vm.changeHoleInFocus = function(newHoleNumber) {
        vm.holeInFocus(newHoleNumber);
    }

    vm.uploadedYouTubeVideo = ko.computed(function() {
        var holeInFocus = vm.holeInFocus();
        vm.uploadCourseFootageAreaVisible(false);
        vm.manageAreaVisible(false);

        serverModel.GolfCourseHoleVideos.forEach(function(courseVideo) {
            if (courseVideo.CourseHoleNumber === holeInFocus) {
                if (courseVideo.YouTubeVideoId === "") {
                    //vm.currentHoleInFocusYouTubeVideoId("There is currently no uploaded video.");
                    vm.uploadCourseFootageAreaVisible(true);
                    //vm.manageAreaVisible(false);
                } else {
                    vm.currentHoleYoutubeApiSrc(youtubeEmbeddedUrlBase + courseVideo.YouTubeVideoId);
                    //vm.uploadingAreaVisible(false);
                    vm.manageAreaVisible(true);
                }
            }
        });
    });

    vm.uploadVideo = function(file) {
        var fd = new FormData();
        fd.append("file", file);

        vm.uploadedCourseFile(fd);
    }

    vm.submitUpload = function () {
        if (vm.uploadedCourseFile() === null) {
            return;
        } else {
            $("#reasonPopUp").modal('hide');

            vm.uploadCourseFootageAreaVisible(false);
            vm.uploadingAreaVisible(true);
            vm.videoUploadSpinner.spin(uploadAreaSpinner);

            $.ajax({
                type: "POST",
                url: '/GolfCourses/UploadCourseFootage?courseId=' + vm.courseId() + '&holeNumber=' + vm.holeInFocus(),
                contentType: false,
                processData: false,
                data: vm.uploadedCourseFile(),
                success: function (result) {
                    console.log("The video was successfully uploaded!");
                    vm.videoUploadSpinner.stop();
                },
                error: function () {
                    console.log("help me");
                    vm.videoUploadSpinner.stop();
                }
            });
        }
    }

    vm.submitDelete = function() {
        $.ajax({
            type: "POST",
            url: '/GolfCourses/Modify?courseId=' + vm.courseId() + '&holeNumber=' + vm.holeInFocus(),
            contentType: false,
            processData: false,
            data: vm.uploadedCourseFile(),
            success: function (result) {
                console.log("The video was modified!");
                vm.videoUploadSpinner.stop();
            },
            error: function () {
                console.log("help me");
                vm.videoUploadSpinner.stop();
            }
        });
    }

    vm.showModal = function () {
        $("#reasonPopUp").modal('show');
    }
}