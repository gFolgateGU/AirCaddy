var userReview = function(reviewData, antiForgeryRequestToken) {
    var self = this;

    self.reviewVisibleFlag = ko.observable(false);
    self.parentContainerNonDeleteVisible = ko.observable(true);

    self.id = reviewData.Id;
    self.holeNumber = reviewData.HoleNumber;
    self.golfCourseId = reviewData.GolfCourseId;
    self.difficulty = reviewData.Difficulty;
    self.comment = reviewData.Comment;
    self.username = reviewData.Username;
    self.showDeleteRatingOption = reviewData.ShowDeleteRatingOption;
    self.requestToken = antiForgeryRequestToken

    self.toggleReview = function () {
        if (!self.reviewVisibleFlag()) {
            self.reviewVisibleFlag(true);
        }
        else {
            self.reviewVisibleFlag(false);
        }
    }

    self.deleteUserReview = function () {
        self.parentContainerNonDeleteVisible(false);
        $.ajax("/GolfCourses/DeleteDifficultyCommentRatingForHole",
            {
                type: "post",
                data: {
                    //__RequestVerificationToken: self.requestToken,
                    reviewId: self.id
                },
                success: function (data) {
                    if (data === true) {
                        self.parentContainerNonDeleteVisible(false);
                        alert("The review was succesfully deleted.")
                    }
                    else {
                        alert("There was an error deleting that particular review with ID: " + self.id);
                    }
                },
                error: function () {
                    alert("There was an error contacting the server");
                }
            });
    }
}