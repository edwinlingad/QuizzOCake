interface IAssignmentGroupModel extends ITrackModify {
    id?: number;
    message?: string;
    targetScore?: number;
    isDefaultTestSetting?: boolean;
    isDeleted?: boolean;
    dateAssigned?: Date;
    /** Extra values */
    assignedByFullName?: string;
    /** Foreign Keys */
    testSettingId?: number;
    quizzId?: number;
    assignedById?: number;
    /** Navigation Properties */
    testSetting?: ITestSettingModel;
    assignments?: IAssignmentInfo[];
}

interface IAssignmentInfo extends ITrackModify {
    id: number;
    dependentFullName: string;
    isCompleted?: boolean;
    completedDate?: Date;
    /** Foreign Keys */
    dependentId: number;

    isChosen: boolean;
}

interface IAssignmentModel {
    id: number;
    isCompleted: boolean;
    completedDate?: Date;
    completedMessage?: string;
    completedTestSetting?: string;
    /** Foreign Keys */
    dependentId: number;
    assignmentGroupId: number;
    assignmentGroup?: IAssignmentGroupModel;
}
 