var virtualTourViewModel = function(serverModel) {

    var vm = this;

    //other constants
    var youtubeEmbeddedUrlBase = "https://www.youtube.com/embed/";

    //constant course attributes
    vm.courseId = serverModel.GolfCourseId;
    vm.courseName = serverModel.GolfCourseName;
    vm.courseAddress = serverModel.GolfCourseAddress;
    vm.coursePhone = serverModel.GolfCoursePhone;
    vm.courseType = serverModel.GolfCourseType;
    vm.courseOwner = serverModel.GolfCourseOwnerId;

    //virtual tour viewmodel specific attributes
    vm.holeInFocus = ko.observable(1);

    //visible trigger attributes
    vm.footageAvailableVisible = ko.observable(false);
    vm.footageUnavailableVisible = ko.observable(true);

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