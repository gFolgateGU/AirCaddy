var myCoursesViewModel = function(serverModel) {

    var vm = this;

    vm.myCourses = ko.observableArray(init(serverModel));

    function init(golfCourses) {
        var myGolfCourses = [];
        golfCourses.forEach(function(golfCourse) {
            var myCourse = new course(golfCourse);
            myGolfCourses.push(myCourse);
        });
        return myGolfCourses;
    }
}