declare module server {
	const enum RelationshipNotificationResponseEnum {
		Accept = 0,
		Reject,
		Resend,
		Cancel,
	}
	interface RelationshipNotificationModel {
		id: number;
		rNType: any;
		postedDate: Date;
		isNew: boolean;
		isAccepted: boolean;
		toUserId: number;
		fromUserId: number;
		friendRequestId: number;
		dependentRequestFromUserId: number;
		toUserName: string;
		toUserFullName: string;
		fromUserName: string;
		fromUserFullName: string;
		quizzmateRequestMessage: string;
		response: server.RelationshipNotificationResponseEnum;
	}
}
