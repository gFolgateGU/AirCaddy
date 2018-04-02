var userReview = function(reviewData) {
    var self = this;

    self.reviewVisibleFlag = ko.observable(false);

    self.id = reviewData.Id;
    self.holeNumber = reviewData.HoleNumber;
    self.golfCourseId = reviewData.GolfCourseId;
    self.difficulty = reviewData.Difficulty;
    self.comment = reviewData.Comment;
    self.username = reviewData.Username;

    self.toggleReview = function () {
        if (!self.reviewVisibleFlag()) {
            self.reviewVisibleFlag(true);
        }
        else {
            self.reviewVisibleFlag(false);
        }
    }
}