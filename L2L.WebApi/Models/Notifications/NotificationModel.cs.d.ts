declare module server {
	interface NotificationModel {
		postedDate: Date;
		isNew: boolean;
		count: number;
		newCount: number;
		message: string;
		notificationType: any;
		dependentId: number;
		dependentName: string;
		userId: number;
		userName: string;
		fullName: string;
		quizzId: number;
		quizzTitle: string;
		quizzAuthorUserName: string;
		quizzAuthorFullName: string;
		quizzCommentId: number;
		quizzComment: string;
		quizzCommentAuthorUserName: string;
		quizzCommentAuthorFullName: string;
		questionId: number;
		question: string;
	}
	interface NotificationTypeModel {
		notificationType: any;
		fAIcon: string;
		fgColor: string;
	}
}
