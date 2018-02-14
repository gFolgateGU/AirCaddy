var yelpReview = function(reviewData) {
    var self = this;

    self.reviewVisibleFlag = ko.observable(false);

    self.rating = reviewData.Rating;
    self.user = reviewData.User;
    self.reviewText = reviewData.ReviewText;
    self.reviewDate = reviewData.ReviewDate;

    self.toggleReview = function () {
        if (!self.reviewVisibleFlag()) {
            self.reviewVisibleFlag(true);
        }
        else {
            self.reviewVisibleFlag(false);
        }
    }
}