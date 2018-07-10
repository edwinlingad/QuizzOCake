/// <reference path="../Relationship/RelationshipNotificationModels.cs.d.ts" />

declare module server {
	interface UserPointsBase {
		dailyNormalPointsQuizzSelfStrList: string;
		dailyNormalPointsQuizzOthersStrList: string;
		dailySpecialPointsQuizzStrList: string;
		dailyPointsAllStrList: string;
		badgeStrList: string;
		totalDailyRewardItems: number;
		dailyNormalPointsQuizzSelfIntList: number[];
		dailyNormalPointsQuizzOthersIntList: number[];
		dailySpecialPointsQuizzIntList: number[];
		dailyPointsAllIntList: number[];
		badgeIntList: number[];
	}
	interface UserModel extends UserPointsBase {
		id: number;
		userType: any;
		localAuthUserId: string;
		userName: string;
		email: string;
		profileId: number;
		profile: {
			id: number;
			firstName: string;
			lastName: string;
			birthDate: Date;
			profileImageUrl: string;
			userId: number;
		};
		points: number;
		dailyPoints: number;
		dailyPointsDate: string;
		totalBadge: number;
		dependentPermission: {
			id: number;
			canAcceptQuizzmateRequests: boolean;
			canUseMessaging: boolean;
			user: {
				id: number;
				userType: any;
				localAuthUserId: string;
				userName: string;
				email: string;
				points: number;
				dailyPoints: number;
				dailyPointsDate: string;
				dailyNormalPointsQuizzSelfStrList: string;
				dailyNormalPointsQuizzOthersStrList: string;
				dailySpecialPointsQuizzStrList: string;
				dailyPointsAllStrList: string;
				totalBadge: number;
				badgeStrList: string;
				dependentPermission: any;
				profile: {
					id: number;
					firstName: string;
					lastName: string;
					birthDate: Date;
					profileImageUrl: string;
					user: any;
				};
				quizzes: any[];
				quizLogs: any[];
				questionsCreated: any[];
				userRatings: any[];
				quizUpvotes: any[];
				friendsAsUser1: any[];
				friendsAsUser2: any[];
				friendRequestsTo: any[];
				friendRequestsFrom: any[];
				userGroups: any[];
				publicGroupsCreated: any[];
				publicGroupsMemberOf: any[];
				publicGroupRequests: any[];
				asUserDependents: any[];
				asChildDependsOn: any[];
				asUserDependentRequestsTo: any[];
				asUserDependentRequestsFrom: any[];
				asChildDependentRequestsTo: any[];
				asChildDependentRequestsFrom: any[];
			};
		};
		isAdmin: boolean;
		isEmailConfirmed: boolean;
		asUserDependents: server.DependentModel[];
	}
	interface DependentModel {
		id: number;
		isPrimary: boolean;
		userId: number;
		childId: number;
		dependentFullName: string;
	}
	interface DependentUserModel {
		id: number;
		userName: string;
		profile: {
			id: number;
			firstName: string;
			lastName: string;
			birthDate: Date;
			profileImageUrl: string;
			userId: number;
		};
		userType: any;
		isCurrentUserPrimary: boolean;
		permissions: server.DependentPermissionModel;
		notificationsSubscription: server.DependentNotificationModel;
	}
	interface DependentPermissionModel {
		id: number;
		canAcceptQuizzmateRequests: boolean;
		canUseMessaging: boolean;
	}
	interface DependentNotificationModel {
		childId: number;
		nwQuizzSubmit: boolean;
		nwQuizzLive: boolean;
		nwQuizzReceiveComment: boolean;
		nwPostComment: boolean;
		nwPostedCommentFlagged: boolean;
		nwQuestionFlagged: boolean;
		nwMessageSent: boolean;
		nwMessageReceived: boolean;
		nwQuizzmateRequest: boolean;
		nwQuizzmateRequestAccept: boolean;
	}
	interface QuizzerModel extends UserPointsBase {
		id: number;
		userName: string;
		profile: {
			id: number;
			firstName: string;
			lastName: string;
			birthDate: Date;
			profileImageUrl: string;
			userId: number;
		};
		points: number;
		dailyPoints: number;
		dailyPointsDate: string;
		totalBadge: number;
		numQuizzes: number;
		userFullName: string;
		isQuizzmate: boolean;
		friendRequestSentId: number;
		isFriendRequestSent: boolean;
		friendRequestPendingId: number;
		isFriendRequestPending: boolean;
		friendRequestId: number;
		isQuizzlingRequestSent: boolean;
		isFollowing: boolean;
		isSelf: boolean;
		relationshipNotification: server.RelationshipNotificationModel;
		isProfilePixModified: boolean;
		profilePix: string;
		profilePixName: string;
		age: number;
		birthDate: Date;
	}
}
