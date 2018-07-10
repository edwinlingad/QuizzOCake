declare module server {
	interface LeftSideBarModel {
		testSnapshots: server.LayoutTestSnapshot[];
		dependents: server.LayoutDependentModel[];
		hasMoreAssignments: boolean;
		assignments: server.LayoutAssignmentModel[];
		hasMoreAssignmentsGiven: boolean;
		assignmentsGiven: server.LayoutAssignmentGroupModel[];
		hasMoreRecentQuizzes: boolean;
		recentQuizzes: server.LayoutRecentQuizzModel[];
		hasMoreMyQuizzes: boolean;
		myQuizzes: server.LayoutQuizzModel[];
		hasMoreBookmarks: boolean;
		bookmarks: server.LayoutBookmarkModel[];
		suggestedQuizzes: server.LayoutSuggestedQuizzModel[];
		groups: server.LayoutGroupModel[];
	}
	interface LayoutTestSnapshot {
		id: number;
	}
	interface LayoutAssignmentModel {
		id: number;
		quizzName: string;
		category: number;
	}
	interface LayoutAssignmentGroupModel {
		id: number;
		quizzName: string;
		category: number;
	}
	interface LayoutBookmarkModel {
		id: number;
		quizzName: string;
	}
	interface LayoutDependentModel {
		userId: number;
		dependentName: string;
	}
	interface LayoutGroupModel {
		id: number;
		groupName: string;
	}
	interface LayoutQuizzModel extends QuizzModel {
	}
	interface LayoutRecentQuizzModel {
		id: number;
		score: number;
		total: number;
		duration: number;
		dateTaken: Date;
		comment: string;
		userId: number;
		testSettingId: number;
		quizzName: string;
		quizzId: number;
		category: number;
	}
	interface LayoutSuggestedQuizzModel {
		id: number;
		quizzName: string;
	}
}
