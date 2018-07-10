interface ITestLogModel {
    id?: number;
    score?: number;
    total?: number;
    duration?: number;
    dateTaken?: Date;
    comment?: string;
    resultBlob?: any;
    quizzName?: string;
    category?: number;

    // foreign keys
    userId?: number;
    testSettingId?: number;
    quizzId?: number;
    testSetting?: ITestSettingModel;
    assignmentId?: number;

    // from code
    isPassed?: boolean;

    // for view 
    average?: number;
}

interface ITestLogDataModel {
    quizzInfo: IQuizzInfo;
    testResult: IQuestionTakenModel[];
    testResultScore: ITestProgress;
}

interface ITestLogGroupedModel {
    quizzSummary: IQuizzSummaryModel;
    testLogs: ITestLogSummaryModel[];

    // for view
    isOpen: boolean;
    average?: number;
}

interface IQuizzSummaryModel {
    id: number;
    title: string;
    description: string;
    scoreSum: number;
    totalSum: number;
    category: number;
}

interface ITestLogSummaryModel {
    id: number;
    score: number;
    total: number;
    dateTaken: Date;

    // for view
    average?: number;
}