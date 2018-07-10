declare module server {
	interface ActivityModel {
		id: number;
		postedDate: Date;
		activityType: any;
		count: number;
		ownerId: number;
		userName: string;
		fullName: string;
		quizzId: number;
		quizzTitle: string;
		quizzCommentId: number;
		quizzComment: string;
		testLogId: number;
		score: number;
		total: number;
		questionId: number;
		actualQuestion: string;
	}
}
