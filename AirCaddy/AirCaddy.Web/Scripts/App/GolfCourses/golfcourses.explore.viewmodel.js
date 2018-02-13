var golfCoursesExploreViewModel = function(serverModel) {
    var vm = this;

    vm.courseName = ko.observable(serverModel.CourseName);
    vm.courseYelpReviews = ko.observableArray();

    initCourseYelpReviews(serverModel.CourseReviews);

    function initCourseYelpReviews(courseReviewData) {
        courseReviewData.forEach(function(reviewInfo) {
            vm.courseYelpReviews.push(new yelpReview(reviewInfo));
        });
    }

    vm.averageRating = ko.computed(function() {
        var runningTotal = 0;
        var numberOfReviews = vm.courseYelpReviews().length;
        vm.courseYelpReviews().forEach(function (review) {
            runningTotal += parseInt(review.rating);
        });
        return runningTotal / numberOfReviews;
    });
}