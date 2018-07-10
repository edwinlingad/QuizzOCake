interface IQuizzCurrentUserRatingModel {
    quizzRatingId: number;
    rating: number;
}

interface IQuizzUserRatingModel {
    quizzRatingId: number;
    ratingAvg: number;
    numRatings: number;
}

interface IQuizzUserRatingUpdateRateModel {
    quizzRatingId: number;
    rating: number;
}