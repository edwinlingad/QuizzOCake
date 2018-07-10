declare module server {
	interface AssignmentQuizzModel {
		id: number;
		title: string;
		description: string;
		isBuiltIn: boolean;
		ownerId: number;
		category: number;
		numQuestions: number;
		numQuickNotes: number;
		ownerName: string;
		reviewerId: number;
		testId: number;
	}
	interface AssignmentGroupModel {
		id: number;
		message: string;
		targetScore: number;
		isDefaultTestSetting: boolean;
		isDeleted: boolean;
		isCompleted: boolean;
		dateAssigned: Date;
		noDueDate: boolean;
		targetDate: Date;
		/** Extra values */
		assignedByFullName: string;
		/** Foreign Keys */
		testSettingId: number;
		quizzId: number;
		assignedById: number;
		/** Navigation Properties */
		testSetting: {
			id: number;
			isOrdered: boolean;
			numberOfQuestions: number;
			timedTypeEnum: any;
			secondsPerQuestion: number;
			secondsForWholeQuiz: number;
			instantFeedback: boolean;
		};
		assignments: server.AssignmentInfo[];
		quizz: server.AssignmentQuizzModel;
	}
	interface AssignmentInfo {
		id: number;
		dependentFullName: string;
		isCompleted: boolean;
		completedDate: Date;
		completedScore: number;
		/** Foreign Keys */
		dependentId: number;
		testResultId: number;
	}
	interface AssignmentModel {
		id: number;
		isCompleted: boolean;
		completedDate: Date;
		completedMessage: string;
		completedTestSetting: string;
		completedScore: number;
		/** Foreign Keys */
		dependentId: number;
		assignmentGroupId: number;
		testResultId: number;
		assignmentGroup: server.AssignmentGroupModel;
	}
}
