var yelpReview = function(reviewData) {
    var self = this;

    self.rating = reviewData.Rating;
    self.user = reviewData.User;
    self.reviewText = reviewData.ReviewText;
    self.reviewDate = reviewData.ReviewDate;
}