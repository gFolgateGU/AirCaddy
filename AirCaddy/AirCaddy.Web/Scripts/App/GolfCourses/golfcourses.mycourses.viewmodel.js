var myCoursesViewModel = function(serverModel, requestUrl) {

    var vm = this;

    vm.myCourseRequestUrl = requestUrl;
    vm.myCourses = ko.observableArray(init(serverModel));

    function init(golfCourses) {
        var myGolfCourses = [];
        golfCourses.forEach(function(golfCourse) {
            var myCourse = new course(golfCourse, vm.myCourseRequestUrl);
            myGolfCourses.push(myCourse);
        });
        return myGolfCourses;
    }
}