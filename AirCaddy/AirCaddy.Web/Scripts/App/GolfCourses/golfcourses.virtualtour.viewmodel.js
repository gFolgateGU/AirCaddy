var virtualTourViewModel = function(serverModel, antiForgeryRequestToken) {

    var vm = this;

    //other constants
    var vimeoEmbeddedUrlBase = "https://player.vimeo.com/video/";
    var difficultyLevels = ["--", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
    vm.tokenValue = antiForgeryRequestToken;

    //constant course attributes
    vm.courseId = serverModel.GolfCourseId;
    vm.courseName = serverModel.GolfCourseName;
    vm.courseAddress = serverModel.GolfCourseAddress;
    vm.coursePhone = serverModel.GolfCoursePhone;
    vm.courseType = serverModel.GolfCourseType;
    vm.courseOwner = serverModel.GolfCourseOwnerId;
    /*vm.courseRatingsForHole = ko.observableArray();

    initCourseRatings(serverModel.GolfCourseHoleRatings);

    function initCourseRatings(courseRatingsData) {
        courseRatingsData.forEach(function (ratingData) {
            vm.courseRatings.push(new userReview(ratingData));
        });
    }*/

    //virtual tour viewmodel specific attributes
    vm.holeInFocus = ko.observable(1);
    vm.holeSpecificCourseRatingComment = ko.observable("");
    vm.difficultyRatingLevels = ko.observableArray(difficultyLevels);
    vm.selectedDifficultyRatingLevel = ko.observable();
    vm.numberOfReviews = ko.observable(0);  //default of 0 will change with computed.
    vm.percentFilledDifficultyBar = ko.observable("0%");  //default without computed
    vm.difficultyBarColor = ko.observable("w3-gray");  //default without computed
    vm.currentHoleVimeoApiSrc = ko.observable("");

    //visible trigger attributes
    vm.footageAvailableVisible = ko.observable(false);
    vm.footageUnavailableVisible = ko.observable(false);
    vm.ratedVisible = ko.observable(false);
    vm.notRatedVisible = ko.observable(false);

    //functionality
    vm.changeHoleInFocus = function(holeNumber)
    {
        vm.holeInFocus(holeNumber);
        vm.holeSpecificCourseRatingComment("");
        vm.selectedDifficultyRatingLevel(0);
        vm.hideSideBar();
    }

    vm.showRatingPopUp = function () {
        $("#submitRatingPopUp").modal("show");
    }

    vm.hideRatingPopUp = function () {
        $("#submitRatingPopUp").modal("hide");
    }

    vm.cancelRatingSubmit = function () {
        
    }

    vm.submitRating = function () {
  
        var difficultyRatingModel = {
            Id: "",
            HoleNumber: vm.holeInFocus(),
            GolfCourseId: vm.courseId,
            CourseName: vm.courseName,
            Difficulty: vm.selectedDifficultyRatingLevel(),
            Comment: vm.holeSpecificCourseRatingComment(),
            Username: ""
        };

        console.log(difficultyRatingModel);

        //vm.courseRatingsForHole().push(new userReview(difficultyRatingModel));

        $.ajax("/GolfCourses/PostDifficultyRatingForHole",
            {
                type: "post",
                data: {
                    __RequestVerificationToken: vm.requestToken,
                    difficultyRating: difficultyRatingModel
                },
                success: function (data) {
                    alert(data);
                    
                    vm.hideRatingPopUp();
                    //if (data === 1) {
                    //    //course is a duplicate entry
                    //    vm.duplicateEntryShow(true);
                    //    resetFields();
                    //}
                    //else if (data === 2) {
                    //    vm.badAddressPopUpMessage("The Address provided is not valid.  Please double check for any mistakes.");
                    //    vm.badAddressShow(true);
                    //} else {
                    //    resetFields();
                    //    vm.successShow(true);
                    //}
                    alert("yes!");
                },
                error: function () {
                    //vm.errorShow(true);
                    alert("no.");
                }
            });
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

    function highlightDifficultyRatingBar(averageRating) {
        if (averageRating <= 3.0) {
            vm.difficultyBarColor("w3-green");
        }
        else if (averageRating <= 7.0) {
            vm.difficultyBarColor("w3-yellow");
        }
        else {
            vm.difficultyBarColor("w3-red");
        }
    }


    //computed functions
    vm.uploadedVideo = ko.computed(function () {
        var holeInFocus = vm.holeInFocus();
        vm.footageAvailableVisible(false);
        vm.footageUnavailableVisible(false);

        serverModel.GolfCourseHoleVideos.forEach(function (courseVideo) {
            if (courseVideo.CourseHoleNumber === holeInFocus) {
                if (courseVideo.YouTubeVideoId === "") {
                    //vm.currentHoleInFocusYouTubeVideoId("There is currently no uploaded video.");
                    vm.footageUnavailableVisible(true);
                    //vm.manageAreaVisible(false);
                } else {
                    vm.currentHoleVimeoApiSrc(vimeoEmbeddedUrlBase + courseVideo.YouTubeVideoId);
                    //vm.uploadingAreaVisible(false);
                    vm.footageAvailableVisible(true);
                }
            }
        });
    });

    vm.courseRatingsForHole = ko.computed(function () {
        vm.ratedVisible(false);
        vm.notRatedVisible(false);

        var courseRatings = [];
        serverModel.GolfCourseHoleRatings.forEach(function (courseHoleRating) {
            if (courseHoleRating.HoleNumber === vm.holeInFocus()) {
                courseRatings.push(new userReview(courseHoleRating));
            }
        });

        if (courseRatings.length === 0) {
            vm.notRatedVisible(true);
        } else {
            vm.ratedVisible(true);
        }

        return courseRatings;
    });

    vm.averageRating = ko.computed(function () {
        vm.percentFilledDifficultyBar(0);
        vm.numberOfReviews(0);

        var runningRatingTotal = 0;
        var numberOfReviews = 0;
        serverModel.GolfCourseHoleRatings.forEach(function (courseHoleRating) {
            if (courseHoleRating.HoleNumber === vm.holeInFocus()) {
                runningRatingTotal += courseHoleRating.Difficulty;
                numberOfReviews += 1;
            }
        });

        if (numberOfReviews === 0) {
            //do something here and say we don't have any reviews
        }

        var averageRating = runningRatingTotal / numberOfReviews;
        var percentToFillNum = averageRating * 10;
        var percentToFillCssProp = percentToFillNum + "%";
        vm.percentFilledDifficultyBar(percentToFillCssProp);
        vm.numberOfReviews(numberOfReviews);
        highlightDifficultyRatingBar(averageRating);
        return averageRating;
    });
}