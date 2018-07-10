/* 

$scope 
$state: ng.ui.IStateService  
$location: ng.ILocationService
$rootScope: 
  
globalDataSvc: IGlobalDataService
testProgressSvc: ITestProgressSvc
takeTestCtrlSvc: ITakeTestCtrlSvc
dialogSvc: IDialogSvc
cachedDataSvc: ICachedDataSvc
testLogSvc: ITestLogSvc
currentUser: ICurrentUser

notificationSvc: INotificationSvc
broadcastSvc: IBroadcastSvc
modalSvc: IModalSvc

reviewerSvc: IReviewerSvc
accountSvc: IAccount
 */

interface ITrackModify {
    isModified?: boolean;
}

interface IQuizzCategoryModel {
    id?: number;
    quizzCategoryType?: any;
    title?: string;
    description?: string;
    iconStrValue?: string;

    iconColor?: string;
    borderColor?: string;
    textColor?: string;
    isIncludedInDailyReward?: boolean;

    quizzCount?: number;
}

//interface IQuizzModel extends ITrackModify {
//    id?: number;
//    title?: string;
//    description?: string;
//    isLive?: boolean;
//    isBuiltIn?: boolean;
//    builtInType?: number;
//    mainType?: number;
//    subType?: number;
//    SubType2?: number;
//    subType3?: number;
//    subType4?: number;
//    subType5?: number;
//    visibility?: any;
//    modifyPermission?: number;
//    category?: number;
//    difficulty?: number;
//    gradeLevelMin?: number;
//    gradeLevelMax?: number;
//    ownerId?: number;
//    reviewerId?: number;
//    testId?: number;
//}

interface IQuizzModel extends ITrackModify {
    id?: number;
    title?: string;
    description?: string;
    isLive?: boolean;
    visibility?: any;
    modifyPermission?: number;
    isBuiltIn?: boolean;
    builtInType?: any;
    mainType?: number;
    subType?: number;
    subType2?: number;
    subType3?: number;
    subType4?: number;
    subType5?: number;
    quizzTags?: string;

    ownerId?: number;
    reviewerId?: number;
    testId?: number;
    defaultTestSetting?: ITestSettingModel;
    category?: number;
    difficulty?: any;
    gradeLevelMin?: any;
    gradeLevelMax?: any;
    dailyReward?: IDailyRewardModel;
}

interface IQuizzOverviewListResult {
    totalCount: number;
    pageNum: number;
    numPerPage: number;
    list: IQuizzOverviewModel[];
}

interface IQuizzOverviewModel {
    id?: number;
    title?: string;
    description?: string;
    isLive?: boolean;
    category?: number;
    difficulty?: number;
    gradeLevelMin?: number;
    gradeLevelMax?: number;

    isBuiltIn?: boolean;
    builtInType?: number;
    mainType?: number;
    subType?: number;
    subType2?: number;
    subType3?: number;
    subType4?: number;
    subType5?: number;
    quizzTags?: string;

    // foreign keys
    ownerId?: number;

    // generated from query
    numQuestions?: number;
    numQuickNotes?: number;
    numLikes?: number;
    numTaken?: number;

    isQuizzmate?: boolean;
    ownerUserName?: string;
    ownerFullName?: string;
    ownerPoints?: number;
    totalBadge?: number;
    reviewerId?: number;
    testId?: number;
    isBookmarked?: boolean;
    isLiked?: boolean;
    quizzRatingId?: number;
    numComments?: number;

    dailyReward?: IDailyRewardModel;

    // generated from code
    ownerName?: string;
    isNew?: boolean;
    canEdit?: boolean;
    isOwner?: boolean;

    // for view
    disabled?: boolean;

    // callbacks
    updateQuestions?: () => void;
}

interface IQuizzInfo {
    title?: string;
    description?: string;
    category?: number;
}

interface ITakeTestModel extends ITestModel {
    qandAQuestions: IQAQuestionModel[];
    multiChoiceQuestions: IMCQuestionModel[];
    multiChoiceSameQuestions: IMultiChoiceSameQuestionModel[];
}

interface ITestSettingModel extends ITrackModify {
    id?: number;
    isOrdered?: boolean;
    numberOfQuestions?: number;
    timedTypeEnum?: any;
    secondsPerQuestion?: number;
    secondsForWholeQuiz?: number;
    instantFeedback?: boolean;
}

interface IQuestionModel {
    id?: number;
    questionId?: number;
    questionType?: number;
    order?: number;
    isFlashCard?: boolean;
    testId?: number;
    question?: string;

    isReady?: boolean;

    questionViewType?: number;
}

interface ICreateQuestionModel {
    questionType: any;
    question: any;
}

interface IQuestionWithActualModel extends IQuestionModel {
    actualQuestion?: any;

    idx?: number;
    isSubmitted?: boolean;
    isMarked?: boolean;
}

interface IQuestionTakenModel {
    question?: IQuestionWithActualModel;
    answer?: any;
    isCorrect?: boolean;
    score?: number;
    totalAvailableScore?: number;
}

interface IQuizzGradeLevelModel {
    quizzGradeLevelType: enums.QuizzGradeLevelEnum;
    name: string;
}

/* 
 * Reviewers
 */

interface ICreateReviewerModel {
    reviewerType: any;
    reviewer: any;
}



interface IReviewerItemModel {
    id: number;
    reviewerString: string;
    reviewerType: any;
    actualReviewerId: number;
    order: number;
    reviewerId: number;
}

interface IReviewerItemWithActualModel extends IReviewerItemModel {
    actualReviewer: any;
}

interface IReviewerModel {
    id: number;
    quizzId: number;
    reviewerItems: any[];
}

interface IReviewerWithActualItemsModel extends IReviewerModel {
    textFlashCards: ITextFlashCardModel[];
    quickNotes: IQuickNoteModel[];
}

