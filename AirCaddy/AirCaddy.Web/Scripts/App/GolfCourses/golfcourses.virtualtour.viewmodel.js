var virtualTourViewModel = function(serverModel) {

    var vm = this;

    //other constants
    var youtubeEmbeddedUrlBase = "https://www.youtube.com/embed/";
    var difficultyLevels = ["--", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

    //constant course attributes
    vm.courseId = serverModel.GolfCourseId;
    vm.courseName = serverModel.GolfCourseName;
    vm.courseAddress = serverModel.GolfCourseAddress;
    vm.coursePhone = serverModel.GolfCoursePhone;
    vm.courseType = serverModel.GolfCourseType;
    vm.courseOwner = serverModel.GolfCourseOwnerId;

    //virtual tour viewmodel specific attributes
    vm.holeInFocus = ko.observable(1);
    vm.holeSpecificCourseRatingComment = ko.observable("");
    vm.difficultyRatingLevels = ko.observableArray(difficultyLevels);
    vm.selectedDifficultyRatingLevel = ko.observable();

    //visible trigger attributes
    vm.footageAvailableVisible = ko.observable(false);
    vm.footageUnavailableVisible = ko.observable(true);

    //functionality
    vm.showRatingPopUp = function () {
        $("#submitRatingPopUp").modal("show");
    }

    vm.hideRatingPopUp = function () {
        $("#submitRatingPopUp").modal("hide");
    }

    vm.cancelRatingSubmit = function () {
        
    }

    vm.submitRating = function () {

    }

    vm.showSideBar = function() {
        document.getElementById("main").style.marginLeft = "25%";
        document.getElementById("mySidebar").style.width = "25%";
        document.getElementById("mySidebar").style.display = "block";
        document.getElementById("openNav").style.display = "none";
    }

    vm.hideSideBar = function() {
        document.getElementById("main").style.marginLeft = "0%";
        document.getElementById("mySidebar").style.display = "none";
        document.getElementById("openNav").style.display = "inline-block";
    }

}