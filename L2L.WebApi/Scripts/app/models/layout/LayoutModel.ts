interface ILayoutTestSnapshot {
    id: number;
    origId: number;
}

interface ILayoutAssignmentModel {
    id: number;
    quizzName: string;
    category: any;
}

interface ILayoutAssignmentGroupModel {
    id: number;
    quizzName: string;
    category: any;
}

interface ILayoutBookmarkModel {
    id: number;
    quizzName: string;
}

interface ILayoutDependentModel {
    userId: number;
    dependentName: string;
}

interface ILayoutGroupModel {
    id: number;
    groupName: string;
}

interface ILayoutQuizzModel extends IQuizzModel {
}

interface ILayoutSuggestedQuizzModel {
    id: number;
    quizzName: string;
}

interface ILayoutRecentQuizzModel {
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
    category: any;
}

interface ILeftSideBarModel {
    testSnapshots: ILayoutTestSnapshot[];
    dependents: ILayoutDependentModel[];
    assignments: ILayoutAssignmentModel[];
    assignmentsGiven: ILayoutAssignmentGroupModel[];
    recentQuizzes: ILayoutRecentQuizzModel[];
    myQuizzes: ILayoutQuizzModel[];
    bookmarks: ILayoutBookmarkModel[];
    suggestedQuizzes: ILayoutSuggestedQuizzModel[];
    groups: ILayoutGroupModel[];

    hasMoreAssignments?: boolean;
    hasMoreAssignmentsGiven?: boolean;
    hasMoreRecentQuizzes?: boolean;
    hasMoreMyQuizzes?: boolean;
    hasMoreBookmarks?: boolean;
}

interface ITopPanelModel {
    newQuizzClassNotificationCount?: number;
    newFriendRequestCount?: number;
    newMessageCount?: number;
    newNotificationCount?: number;
}

interface ILayoutModel {
    topPanel: ITopPanelModel;
    leftSideBar: ILeftSideBarModel;
} 