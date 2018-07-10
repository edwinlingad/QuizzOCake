declare module server {
	interface Notification {
		id: number;
		postedDate: Date;
		notificationType: any;
		isNew: boolean;
		count: number;
		/** Foreign Keys */
		userId: number;
		quizzNotificationId: number;
		questionNotificationId: number;
		/** Navigation Properties */
		user: {
			id: number;
			userType: any;
			localAuthUserId: string;
			userName: string;
			email: string;
		/** Foreign KeysNavigation Properties */
			dependentPermission: {
				id: number;
				canAcceptQuizzmateRequests: boolean;
				canUseMessaging: boolean;
		/** Foreign KeysNavigation Properties */
				user: any;
			};
			profile: {
				id: number;
				firstName: string;
				lastName: string;
				birthDate: Date;
				profileImageUrl: string;
		/** Foreign KeysNavigation Properties */
				user: any;
			};
			quizzes: any[];
			quizLogs: any[];
			userRatings: any[];
			quizUpvotes: any[];
		/** Unfortunately the relationships are like these because of how the tables */
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
		quizzNotification: server.QuizzNotification;
		quizzCommentNotification: server.QuizzCommentNotification;
		questionNotification: server.QuestionNotification;
	}
	interface QuizzNotification {
		id: number;
		/** Foreign Keys */
		userId: number;
		quizzId: number;
		notificationId: number;
		/** Navigation Properties */
		user: {
			id: number;
			userType: any;
			localAuthUserId: string;
			userName: string;
			email: string;
		/** Foreign KeysNavigation Properties */
			dependentPermission: {
				id: number;
				canAcceptQuizzmateRequests: boolean;
				canUseMessaging: boolean;
		/** Foreign KeysNavigation Properties */
				user: any;
			};
			profile: {
				id: number;
				firstName: string;
				lastName: string;
				birthDate: Date;
				profileImageUrl: string;
		/** Foreign KeysNavigation Properties */
				user: any;
			};
			quizzes: any[];
			quizLogs: any[];
			userRatings: any[];
			quizUpvotes: any[];
		/** Unfortunately the relationships are like these because of how the tables */
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
		quizz: {
			id: number;
			title: string;
			description: string;
			isLive: boolean;
			isBuiltIn: boolean;
			isDeleted: boolean;
			visibility: any;
			modifyPermission: number;
			category: any;
			difficulty: any;
			gradeLevelMin: any;
			gradeLevelMax: any;
		/** Foreign Keys */
			ownerId: number;
		/** Navigation Properties */
			owner: {
				id: number;
				userType: any;
				localAuthUserId: string;
				userName: string;
				email: string;
		/** Foreign KeysNavigation Properties */
				dependentPermission: {
					id: number;
					canAcceptQuizzmateRequests: boolean;
					canUseMessaging: boolean;
		/** Foreign KeysNavigation Properties */
					user: any;
				};
				profile: {
					id: number;
					firstName: string;
					lastName: string;
					birthDate: Date;
					profileImageUrl: string;
		/** Foreign KeysNavigation Properties */
					user: any;
				};
				quizzes: any[];
				quizLogs: any[];
				userRatings: any[];
				quizUpvotes: any[];
		/** Unfortunately the relationships are like these because of how the tables */
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
			quizRating: {
				id: number;
		/** Foreign Keys */
				quizzId: number;
		/** Navigation Properties */
				quizz: any;
				quizUserRatings: any[];
				quizUpvotes: any[];
			};
			tests: any[];
			reviewers: any[];
			tags: any[];
			comments: any[];
		};
		notification: server.Notification;
		notificationSources: server.QuizzNotificationSource[];
	}
	interface QuizzNotificationSource {
		id: number;
		/** Foreign Keys */
		sourceId: number;
		quizzNotificationId: number;
		/** Navigation Properties */
		source: {
			id: number;
			userType: any;
			localAuthUserId: string;
			userName: string;
			email: string;
		/** Foreign KeysNavigation Properties */
			dependentPermission: {
				id: number;
				canAcceptQuizzmateRequests: boolean;
				canUseMessaging: boolean;
		/** Foreign KeysNavigation Properties */
				user: any;
			};
			profile: {
				id: number;
				firstName: string;
				lastName: string;
				birthDate: Date;
				profileImageUrl: string;
		/** Foreign KeysNavigation Properties */
				user: any;
			};
			quizzes: any[];
			quizLogs: any[];
			userRatings: any[];
			quizUpvotes: any[];
		/** Unfortunately the relationships are like these because of how the tables */
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
		quizzNotification: server.QuizzNotification;
	}
	interface QuizzCommentNotification {
		id: number;
		/** Foreign Keys */
		userId: number;
		quizzCommentId: number;
		notificationId: number;
		/** Navigation Properties */
		user: {
			id: number;
			userType: any;
			localAuthUserId: string;
			userName: string;
			email: string;
		/** Foreign KeysNavigation Properties */
			dependentPermission: {
				id: number;
				canAcceptQuizzmateRequests: boolean;
				canUseMessaging: boolean;
		/** Foreign KeysNavigation Properties */
				user: any;
			};
			profile: {
				id: number;
				firstName: string;
				lastName: string;
				birthDate: Date;
				profileImageUrl: string;
		/** Foreign KeysNavigation Properties */
				user: any;
			};
			quizzes: any[];
			quizLogs: any[];
			userRatings: any[];
			quizUpvotes: any[];
		/** Unfortunately the relationships are like these because of how the tables */
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
		quizzComment: {
			id: number;
			comment: string;
			postedDate: Date;
		/** Foreign Keys */
			authorId: number;
			quizzId: number;
		/** Navigation Properties */
			author: {
				id: number;
				userType: any;
				localAuthUserId: string;
				userName: string;
				email: string;
		/** Foreign KeysNavigation Properties */
				dependentPermission: {
					id: number;
					canAcceptQuizzmateRequests: boolean;
					canUseMessaging: boolean;
		/** Foreign KeysNavigation Properties */
					user: any;
				};
				profile: {
					id: number;
					firstName: string;
					lastName: string;
					birthDate: Date;
					profileImageUrl: string;
		/** Foreign KeysNavigation Properties */
					user: any;
				};
				quizzes: any[];
				quizLogs: any[];
				userRatings: any[];
				quizUpvotes: any[];
		/** Unfortunately the relationships are like these because of how the tables */
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
			quizz: {
				id: number;
				title: string;
				description: string;
				isLive: boolean;
				isBuiltIn: boolean;
				isDeleted: boolean;
				visibility: any;
				modifyPermission: number;
				category: any;
				difficulty: any;
				gradeLevelMin: any;
				gradeLevelMax: any;
		/** Foreign Keys */
				ownerId: number;
		/** Navigation Properties */
				owner: {
					id: number;
					userType: any;
					localAuthUserId: string;
					userName: string;
					email: string;
		/** Foreign KeysNavigation Properties */
					dependentPermission: {
						id: number;
						canAcceptQuizzmateRequests: boolean;
						canUseMessaging: boolean;
		/** Foreign KeysNavigation Properties */
						user: any;
					};
					profile: {
						id: number;
						firstName: string;
						lastName: string;
						birthDate: Date;
						profileImageUrl: string;
		/** Foreign KeysNavigation Properties */
						user: any;
					};
					quizzes: any[];
					quizLogs: any[];
					userRatings: any[];
					quizUpvotes: any[];
		/** Unfortunately the relationships are like these because of how the tables */
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
				quizRating: {
					id: number;
		/** Foreign Keys */
					quizzId: number;
		/** Navigation Properties */
					quizz: any;
					quizUserRatings: any[];
					quizUpvotes: any[];
				};
				tests: any[];
				reviewers: any[];
				tags: any[];
				comments: any[];
			};
			likes: any[];
			flags: any[];
		};
		notification: server.Notification;
		notificationSources: server.QuizzCommentNotificationSource[];
	}
	interface QuizzCommentNotificationSource {
		id: number;
		/** Foreign Keys */
		sourceId: number;
		quizzCommentNotificationId: number;
		/** Navigation Properties */
		source: {
			id: number;
			userType: any;
			localAuthUserId: string;
			userName: string;
			email: string;
		/** Foreign KeysNavigation Properties */
			dependentPermission: {
				id: number;
				canAcceptQuizzmateRequests: boolean;
				canUseMessaging: boolean;
		/** Foreign KeysNavigation Properties */
				user: any;
			};
			profile: {
				id: number;
				firstName: string;
				lastName: string;
				birthDate: Date;
				profileImageUrl: string;
		/** Foreign KeysNavigation Properties */
				user: any;
			};
			quizzes: any[];
			quizLogs: any[];
			userRatings: any[];
			quizUpvotes: any[];
		/** Unfortunately the relationships are like these because of how the tables */
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
		quizzCommentNotification: server.QuizzCommentNotification;
	}
	interface QuestionNotification {
		id: number;
		/** Foreign Keys */
		userId: number;
		questionId: number;
		notificationId: number;
		/** Navigation Properties */
		user: {
			id: number;
			userType: any;
			localAuthUserId: string;
			userName: string;
			email: string;
		/** Foreign KeysNavigation Properties */
			dependentPermission: {
				id: number;
				canAcceptQuizzmateRequests: boolean;
				canUseMessaging: boolean;
		/** Foreign KeysNavigation Properties */
				user: any;
			};
			profile: {
				id: number;
				firstName: string;
				lastName: string;
				birthDate: Date;
				profileImageUrl: string;
		/** Foreign KeysNavigation Properties */
				user: any;
			};
			quizzes: any[];
			quizLogs: any[];
			userRatings: any[];
			quizUpvotes: any[];
		/** Unfortunately the relationships are like these because of how the tables */
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
		question: {
			id: number;
			questionId: number;
			questionType: any;
			order: number;
		/** Foreign Keys */
			testId: number;
		/** public int AuthorId { get; set; }Navigation Properties */
			test: {
				id: number;
		/** Foreign Keys */
				quizzId: number;
				defaultSettingId: number;
		/** Navigation Properties */
				quizz: {
					id: number;
					title: string;
					description: string;
					isLive: boolean;
					isBuiltIn: boolean;
					isDeleted: boolean;
					visibility: any;
					modifyPermission: number;
					category: any;
					difficulty: any;
					gradeLevelMin: any;
					gradeLevelMax: any;
		/** Foreign Keys */
					ownerId: number;
		/** Navigation Properties */
					owner: {
						id: number;
						userType: any;
						localAuthUserId: string;
						userName: string;
						email: string;
		/** Foreign KeysNavigation Properties */
						dependentPermission: {
							id: number;
							canAcceptQuizzmateRequests: boolean;
							canUseMessaging: boolean;
		/** Foreign KeysNavigation Properties */
							user: any;
						};
						profile: {
							id: number;
							firstName: string;
							lastName: string;
							birthDate: Date;
							profileImageUrl: string;
		/** Foreign KeysNavigation Properties */
							user: any;
						};
						quizzes: any[];
						quizLogs: any[];
						userRatings: any[];
						quizUpvotes: any[];
		/** Unfortunately the relationships are like these because of how the tables */
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
					quizRating: {
						id: number;
		/** Foreign Keys */
						quizzId: number;
		/** Navigation Properties */
						quizz: any;
						quizUserRatings: any[];
						quizUpvotes: any[];
					};
					tests: any[];
					reviewers: any[];
					tags: any[];
					comments: any[];
				};
				defaultSetting: {
					id: number;
					isOrdered: boolean;
					numberOfQuestions: number;
					timedTypeEnum: any;
					secondsPerQuestion: number;
					secondsForWholeQuiz: number;
					instantFeedback: boolean;
				};
				questions: any[];
				qandAQuestions: any[];
				multiChoiceQuestions: any[];
				multiChoiceSameQuestions: any[];
				multiChoiceSameChoiceGroups: any[];
			};
		/** public virtual User Author { get; set; } */
			quickNoteRef: {
				id: number;
				title: string;
				notes: string;
		/** Foreign Keys */
				revieweId: number;
		/** Navigation Properties */
				reviewer: {
					id: number;
		/** Foreign Keys */
					quizzId: number;
		/** Navigation Properties */
					quizz: {
						id: number;
						title: string;
						description: string;
						isLive: boolean;
						isBuiltIn: boolean;
						isDeleted: boolean;
						visibility: any;
						modifyPermission: number;
						category: any;
						difficulty: any;
						gradeLevelMin: any;
						gradeLevelMax: any;
		/** Foreign Keys */
						ownerId: number;
		/** Navigation Properties */
						owner: {
							id: number;
							userType: any;
							localAuthUserId: string;
							userName: string;
							email: string;
		/** Foreign KeysNavigation Properties */
							dependentPermission: {
								id: number;
								canAcceptQuizzmateRequests: boolean;
								canUseMessaging: boolean;
		/** Foreign KeysNavigation Properties */
								user: any;
							};
							profile: {
								id: number;
								firstName: string;
								lastName: string;
								birthDate: Date;
								profileImageUrl: string;
		/** Foreign KeysNavigation Properties */
								user: any;
							};
							quizzes: any[];
							quizLogs: any[];
							userRatings: any[];
							quizUpvotes: any[];
		/** Unfortunately the relationships are like these because of how the tables */
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
						quizRating: {
							id: number;
		/** Foreign Keys */
							quizzId: number;
		/** Navigation Properties */
							quizz: any;
							quizUserRatings: any[];
							quizUpvotes: any[];
						};
						tests: any[];
						reviewers: any[];
						tags: any[];
						comments: any[];
					};
					reviewerItems: any[];
					quickNotes: any[];
					textFlashCards: any[];
				};
				relatedQuestions: any[];
			};
		};
		notification: server.Notification;
		notificationSources: server.QuestionNotificationSource[];
	}
	interface QuestionNotificationSource {
		id: number;
		/** Foreign Keys */
		sourceId: number;
		questionNotificationId: number;
		/** Navigation Properties */
		source: {
			id: number;
			userType: any;
			localAuthUserId: string;
			userName: string;
			email: string;
		/** Foreign KeysNavigation Properties */
			dependentPermission: {
				id: number;
				canAcceptQuizzmateRequests: boolean;
				canUseMessaging: boolean;
		/** Foreign KeysNavigation Properties */
				user: any;
			};
			profile: {
				id: number;
				firstName: string;
				lastName: string;
				birthDate: Date;
				profileImageUrl: string;
		/** Foreign KeysNavigation Properties */
				user: any;
			};
			quizzes: any[];
			quizLogs: any[];
			userRatings: any[];
			quizUpvotes: any[];
		/** Unfortunately the relationships are like these because of how the tables */
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
		questionNotification: server.QuestionNotification;
	}
}
