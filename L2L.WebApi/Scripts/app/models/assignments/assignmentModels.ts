interface IAssignmentQuizzModel {
    id: number;
    title: string;
    description: string;
    isBuiltIn: boolean;
    ownerId: number;
    numQuestions: number;
    numQuickNotes: number;
    ownerName: string;
    reviewerId: number;
    testId: number;
}

interface IAssignmentGroupModel extends ITrackModify {
    id?: number;
    message?: string;
    targetScore?: number;
    isDefaultTestSetting?: boolean;
    isDeleted?: boolean;
    isCompleted?: boolean;
    dateAssigned?: Date;
    noDueDate?: boolean;
    targetDate?: any;
    /** Extra values */
    assignedByFullName?: string;
    /** Foreign Keys */
    testSettingId?: number;
    quizzId?: number;
    assignedById?: number;
    /** Navigation Properties */
    testSetting?: ITestSettingModel;
    assignments?: IAssignmentInfo[];
    quizz?: IAssignmentQuizzModel;

    numQuestions?: number;

    numDaysDue?: number;
}

interface IAssignmentInfo extends ITrackModify {
    id: number;
    dependentFullName: string;
    isCompleted?: boolean;
    completedDate?: Date;
    completedScore?: number;
    /** Foreign Keys */
    dependentId: number;
    testResultId?: number;

    isChosen: boolean;
}

interface IAssignmentModel {
    id: number;
    isCompleted: boolean;
    completedDate?: Date;
    completedMessage?: string;
    completedTestSetting?: string;
    completedScore?: number;
    /** Foreign Keys */
    dependentId: number;
    assignmentGroupId: number;
    testResultId?: number;

    assignmentGroup?: IAssignmentGroupModel;

    numDaysDue?: number;
}
 