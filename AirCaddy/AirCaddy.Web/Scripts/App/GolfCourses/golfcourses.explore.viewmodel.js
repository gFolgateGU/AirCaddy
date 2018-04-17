var golfCoursesExploreViewModel = function(serverModel, requestUrl) {
    var vm = this;

    vm.courseName = serverModel.CourseName;
    vm.courseId = serverModel.CourseId;
    vm.courseYelpReviews = ko.observableArray();
    vm.virtualTourUrl = requestUrl;

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
        var res = runningTotal / numberOfReviews;
        var resFixed = res.toFixed(2);
        return resFixed;
    });

    vm.takeVirtualTour = function() {
        var url = vm.virtualTourUrl + "?golfCourseId=" + vm.courseId;
        window.location.href = url;
    }
}