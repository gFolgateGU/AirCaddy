var courseGalleryViewModel = function(serverModel, requestUrl) {

    var vm = this;

    vm.searchTerm = ko.observable("");
    vm.noResultsFound = ko.observable(false);
    vm.numberShowing = ko.observable();
    vm.exploreUrl = requestUrl;

    console.log(vm.exploreUrl);

    init(serverModel);

    vm.coursesDisplayList = ko.computed(function () {
        vm.noResultsFound(false);
        if (vm.searchTerm().length < 3) {
            var unfilteredList = init(serverModel);
            return unfilteredList;
        } else {
            var courses = [];
            serverModel.forEach(function (individualCourse) {
                var courseNameToLower = individualCourse.CourseName.toLowerCase();
                if (courseNameToLower.includes(vm.searchTerm().toLowerCase())) {
                    courses.push(new course(individualCourse, vm.exploreUrl));
                }
            });
            if (courses.length < 1) {
                vm.noResultsFound(true);
            } else {
                vm.noResultsFound(false);
            }
            vm.numberShowing(courses.length);
            return courses;
        }
    });

    function init(courseDataList) {
        var courses = [];
        courseDataList.forEach(function (individualCourse) {
            courses.push(new course(individualCourse, vm.exploreUrl));
        });
        vm.numberShowing(courses.length);
        return courses;
    }

}
