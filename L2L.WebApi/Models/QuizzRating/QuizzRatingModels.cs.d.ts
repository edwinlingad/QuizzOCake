declare module server {
	interface QuizzCurrentUserRatingModel {
		quizzRatingId: number;
		rating: number;
	}
	interface QuizzUserRatingModel {
		quizzRatingId: number;
		ratingAvg: number;
		numRatings: number;
	}
	interface QuizzUserRatingUpdateRateModel {
		quizzRatingId: number;
		rating: number;
	}
}
