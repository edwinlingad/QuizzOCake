declare module server {
	interface NewNotificationModel {
		id: number;
		postedDate: Date;
		notificationType: any;
		isNew: boolean;
		newFromUser: string;
		oldFromUser: string;
		toUserId: number;
		fromUserId: number;
		quizzId: number;
		quizzCommentId: number;
		questionId: number;
		assignmentId: number;
		assignmentGroupId: number;
		friendRequestId: number;
		toUnQuizzmateId: number;
		fromUserName: string;
		fromUserFullName: string;
		quizzTitle: string;
		quizzOwnerUserName: string;
		quizzOwnerFullName: string;
		quizzCommentValue: string;
		quizzCommentOwnerUserName: string;
		quizzCommentOwnerFullName: string;
		count: number;
		newCount: number;
		assignmentScore: number;
		toQuizzmateUserId: number;
		toQuizzmateUserName: string;
		toQuizzmateUserFullName: string;
		fromQuizzmateUserId: number;
		fromQuizzmateUserName: string;
		fromQuizzmateUserFullName: string;
		quizzmateRequestMessage: string;
		toUnQuizzmateUserName: string;
		toUnQUizzmateUserFullName: string;
	}
}
