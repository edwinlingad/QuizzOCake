interface INotificationModel {
    postedDate: Date;
    isNew: boolean;
    isNewOrigValue: boolean;
    count: number;
    newCount: number;
    message: string;
    notificationType: enums.NotificationTypeEnum;

    userId: number;
    userName: string;
    fullName: string;

    quizzId: number;
    quizzTitle: string;
    quizzAuthorUserName: string;
    quizzAuthorFullName: string;

    quizzCommentId: number;
    quizzComment: string;
    quizzCommentAuthorUserName: string;
    quizzCommentAuthorFullName: string;

    questionId: number;
    question: string;
} 

interface INotificationTypeModel {
    notificationType: enums.NotificationTypeEnum;
    faIcon: string;
    fgColor: string;
}