interface ITestModel {
    id?: number;
    quizzId?: number;
    defaultSettingId?: number;
    quizz?: IQuizzModel;
    questions?: IQuestionModel[];
    multiChoiceSameChoiceGroups?: IMultiChoiceSameChoiceGroupModel[];
    defaultSetting?: ITestSettingModel;
}

interface ITestSnapshot {
    id: number;
    quizzId: number;
    ownerId: number;
    takeTestModelSnapshot: any;
    testProgressSnapshot: any;
    takeTestCtrlSnapshot: any;
}

interface ITakeTestCtrlSnapshot {
    curIdx: number;
    logResult: boolean;
    totalNumQuestions: number;
    setting: ITestSettingModel;
    questionList: IQuestionWithActualModel[];
    markedQuestions: IQuestionWithActualModel[];
    assignment: IAssignmentModel;
}

interface ITestProgressSnapshot {
    testProgress: ITestProgress;
    questionsTaken: IQuestionTakenModel[];
}