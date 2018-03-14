var manageCourseViewModel = function(serverModel) {

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
    vm.currentHoleYoutubeApiSrc = ko.observable("");

    //Visible Trigger Attributes
    vm.submitCourseFootageUploadAreaVisible = ko.observable(false);
    vm.uploadCourseFootageAreaVisible = ko.observable(true);
    vm.uploadingAreaVisible = ko.observable(false);
    vm.manageAreaVisible = ko.observable(false);
    vm.successfullyUploadedVisible = ko.observable(false);
    vm.failedUploadVisible = ko.observable(false);

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
            $("#uploadPopUp").modal('hide');

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
                    vm.uploadingAreaVisible(false);
                    vm.successfullyUploadedVisible(true);
                },
                error: function () {
                    vm.videoUploadSpinner.stop();
                    vm.uploadingAreaVisible(false);
                }
            });
        }
    }

    vm.submitDelete = function () {
        $("#deletePopUp").modal('hide');

        $.ajax({
            type: "POST",
            url: '/GolfCourses/DeleteCourseFootage?courseId=' + vm.courseId() + '&holeNumber=' + vm.holeInFocus(),
            contentType: false,
            processData: false,
            data: vm.uploadedCourseFile(),
            success: function (result) {
                vm.manageAreaVisible(false);
                vm.uploadCourseFootageAreaVisible(true);
            },
            error: function () {
                alert("There was an error deleting the video..");
            }
        });

    }

    vm.submitModify = function() {
        if (vm.uploadedCourseFile() === null) {
            return;
        } else {
            $("#modifyPopUp").modal('hide');

            vm.uploadCourseFootageAreaVisible(false);
            vm.uploadingAreaVisible(true);
            vm.videoUploadSpinner.spin(uploadAreaSpinner);

            $.ajax({
                type: "POST",
                url: '/GolfCourses/ModifyCourseFootage?courseId=' + vm.courseId() + '&holeNumber=' + vm.holeInFocus(),
                contentType: false,
                processData: false,
                data: vm.uploadedCourseFile(),
                success: function (result) {
                    console.log("The video was successfully uploaded!");
                    vm.uploadingAreaVisible(false);
                    vm.successfullyUploadedVisible(true);
                    vm.videoUploadSpinner.stop();
                },
                error: function () {
                    console.log("help me");
                    vm.videoUploadSpinner.stop();
                    vm.failedUploadVisible(true);
                }
            });
        }       
    }

    vm.showUploadModal = function () {
        $("#uploadPopUp").modal('show');
    }

    vm.hideUploadModal = function() {
        $("#uploadPopUp").modal('hide');
    }

    vm.showModifyModal = function() {
        $("#modifyPopUp").modal('show');
    }

    vm.hideModifyModal = function() {
        $("#modifyPopUp").modal('hide');
    }

    vm.showDeleteModal = function() {
        $("#deletePopUp").modal('show');
    }

    vm.hideDeleteModal = function() {
        $("#deletePopUp").modal('hide');
    }
}