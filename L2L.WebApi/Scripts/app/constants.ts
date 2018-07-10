interface IL2LConsts {
    testQuestionType: ITestQuestionType;
    numPerPage: number;
}

interface ITestQuestionType {
    qandA: string;
    multipleChoice: string;
}

var consts: IL2LConsts = {
    testQuestionType : {
        qandA: "QandA",
        multipleChoice: "MultipleChoice"
    },
    numPerPage: 25
} 

enum UserTypeEnum {
    Standard = 0,
    Dependent
}

enum QuestionTypeEnum {
    BuiltIn,
    QandA,
    MultipleChoice,
    MultiChoiceSame
}

module enums {
    // add-category-tag
    export enum QuizzCategoryEnum {
        Unassigned = -1,
        Math = 0,
        Science = 1,
        English = 2,
        Filipino = 3,
        AralingPanlipunan,
        Mandarin,
        ComputerEducation
    }

    export enum QuizzGradeLevelEnum {
        Unassigned = 0,
        PreK,
        K,
        Grade1,
        Grade2,
        Grade3,
        Grade4,
        Grade5,
        Grade6,
        Grade7,
        Grade8,
        Grade9,
        Grade10,
        Grade11,
        Grade12,
        College,
        Professional,
        // Add here
        MaxGradeLevel
    }

    export enum ReviewerTypeEnum {
        QuickNote,
        TextFlashCard
    }

    export enum SortByEnum {
        DateCreated = 0,
        DateModified,
        NumLikes,
        NumPeopleTakenTest,
        NumQuestions,
        Creator
    }

    export enum SortTypeEnum {
        Ascending = 0,
        Descending
    }

    export enum NotificationTypeEnum {
        QuizzLike,
        QuizzComment,
        QuizzTake,
        QuestionComment
    }

    // RESOURCE-FACTORY
    export enum ResourceTypeEnum {
        QuizzCurrentUserRating,
        QuizzUserRating,
        AssignmentGroup,
        Assignment,
        TestSnapshot,
        QuizzCategories,
        QuizzerInfo,
        BadgeList,
        QuizzmateRequest,
        QuizzlingRequest,
        RelationshipNotification,
        Quizzmates,
        QuizzmateMsgThread,
        QuizzmateMsgThreadMember,
        QuizzmateMsg,
        QuizzConnectMsgThread,
        QuizzClass,
        QuizzClassAnnouncement,
        QuizzClassComment,
        QuizzClassLesson,
        QuizzClassLessonMessage,
        QuizzClassLessonComment,
        QuizzClassJoinRequest,
        QuizzClassMember,
        QuizzClassInviteRequest,
        QuizzClassMemberInvite,
        QuizzClassLessonQuizz,
        QuizzClassQuizz,
        Layout,
        QuizzComment,
        QuizzQuickNote,
        FlashCards,
        ReviewerFromQuestions,
        TestLog
    }
}

module list {
    export var quizzGradeLevels: IQuizzGradeLevelModel[] = [
        {
            quizzGradeLevelType: enums.QuizzGradeLevelEnum.Unassigned,
            name: "Unassigned"
        },
        {
            quizzGradeLevelType: enums.QuizzGradeLevelEnum.PreK,
            name: "Pre K"
        },
        {
            quizzGradeLevelType: enums.QuizzGradeLevelEnum.K,
            name: "Kindergarden"
        },
        {
            quizzGradeLevelType: enums.QuizzGradeLevelEnum.Grade1,
            name: "Grade 1"
        },
        {
            quizzGradeLevelType: enums.QuizzGradeLevelEnum.Grade2,
            name: "Grade 2"
        },
        {
            quizzGradeLevelType: enums.QuizzGradeLevelEnum.Grade3,
            name: "Grade 3"
        },
        {
            quizzGradeLevelType: enums.QuizzGradeLevelEnum.Grade4,
            name: "Grade 4"
        },
        {
            quizzGradeLevelType: enums.QuizzGradeLevelEnum.Grade5,
            name: "Grade 5"
        },
        {
            quizzGradeLevelType: enums.QuizzGradeLevelEnum.Grade6,
            name: "Grade 6"
        },
        {
            quizzGradeLevelType: enums.QuizzGradeLevelEnum.Grade7,
            name: "Grade 7"
        },
        {
            quizzGradeLevelType: enums.QuizzGradeLevelEnum.Grade8,
            name: "Grade 8"
        },
        {
            quizzGradeLevelType: enums.QuizzGradeLevelEnum.Grade9,
            name: "Grade 9"
        },
        {
            quizzGradeLevelType: enums.QuizzGradeLevelEnum.Grade10,
            name: "Grade 10"
        },
        {
            quizzGradeLevelType: enums.QuizzGradeLevelEnum.Grade11,
            name: "Grade 11"
        },
        {
            quizzGradeLevelType: enums.QuizzGradeLevelEnum.Grade12,
            name: "Grade 12"
        },
        {
            quizzGradeLevelType: enums.QuizzGradeLevelEnum.College,
            name: "College"
        },
        {
            quizzGradeLevelType: enums.QuizzGradeLevelEnum.Professional,
            name: "Professionals"
        },
    ];
}

var str = {
    errorConnection: "You seem to be not connected to the internet",
    createSuccess: "Item successfully created",
    updateSuccess: "Update Successful",
    errorLoad: "Something went wrong",
    errorSave: "Something went wrong",
    
    deleteSuccess: "Delete successful",
    
    multiChoiceAtleastOneItemChoice: "There should at least be one chosen answer",
    multiChoiceAtleastTwoItemChoice: "There should at least be 2 choices",
    featureNotAvailableToGuests: "This feature is not available to guest user",

    registerQuizzlingSuccess: "Quizzling successfully registered",
    registerQuizzlingError: "Something went wrong",

    atleastOneDependentChosen: "There should at least be 1 dependent chosen",
    assignmentComplete: "Congratulations: Assignment Done",
    unansweredQuestions: "There are still unanswered questions. Continue?",


    emailConfirmSuccess: "Email confirmed successfully",
    emailConfirmFailed: "Email confirmation failed",

    

    confirmQuizzmateReject: "Quizzmate request will be rejected",
    quizzmateRequestSendSuccess: "Quizzmate request sent successfully",
    quizzmateRequestCancelSuccess: "Quizzmate request cancelled successfully",
    unQuizzmateConfirm: "Quizzmate relationship will be removed",
    unQuizzmateSuccess: "Quizzmate relationship successfully removed",

    relationshipRequestAcceptSuccess: "Request successfully Accepted",
    relationshipRequestRejectSuccess: "Request successfully Rejected",

    chatConnectError: "Failed to connect to chat server",
    errorCreateChat: "Something went wrong",
    errorAddMember: "Somehting went wrong",
    leaveChatGroup: "You are about to leave the chat group",

    successRequest: "Request successfully sent",
    successResendRequest: "Resend request successfully sent",
    successCancelRequest: "Request successfully cancelled",
    successDropStudent: "Successfully dropped student",

    successAcceptRequest: "Accept request successful",
    successRejectRequest: "Request rejected successfully",

    confirmRejectRequest: "Are you sure you want to reject this request?",
    confirmCancelRequest: "Are you sure you want to cancel this request?",
    confirmDropStudent: "Are you sure you want to drop this student?",
    confirmDropOut: "Are you sure you want to drop out?",

    alertAccountAssociated: "Email already associated with an account.",

    // Confirm
    confirmDeleteQuestion: "Are you sure you want to delete this question?",
    confirmChangeTestSnapshot: "Are you sure you want to overwrite your existing Test Progress?",
    confirmRemoveTestSnapshot: "Are you sure you want to delete your Test Progress?",
    confirmLogout: "Are you sure you want to log out of the system?",
    confirmDeleteComment: "Are you sure you want to delete this comment?",
    confirmDelete: "Are you sure you want to delete item?",
}

