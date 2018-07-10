interface IQuizzmateMsgThreadModel {
    id?: number;
    isGroupMsg?: boolean;
    groupMessageName?: string;
    signalRGroupName?: string;

    // navigation properties
    msgThreadMembers?: IQuizzmateMsgThreadMemberModel[];
    messages?: IQuizzmateMsgModel[];

    // generated from code
    isViewerOnly?: boolean;
}

interface IQuizzmateMsgThreadMemberModel {
    id: number;
    hasNew?: boolean;
    newCount?: number;

    // foreign keys
    quizzmateMsgThreadId: number;
    userId: number;

    // generated from query
    userName?: string;
    userFullName?: string;
    profileImageUrl?: string;
}

interface IQuizzmateMsgModel {
    id: number;
    message: string;
    postedDate: Date;
    authorId: number;
    quizzmateMsgThreadId: number;

    // generated
    isNew: boolean;
    userName: string;
    profileImageUrl: string;
}

interface IQuizzConnectMsgThread {
    isEmpty: boolean;
    quizzMessageType: number;
    id: number;
    newCount: number;
    groupName: string;
    quizzmateMsgThreadId: number;
    lastSenderUserName: string;
    lastSenderProfileUrl: string;
    lastSenderMsg: string;
    lastMsgPostedDate: Date;
}

interface IQZConnectMessage {
    userName: string;
    profileImageUrl: string;
    message: string;
    dateTime: string;

    isSelf: boolean;
    isSameAsLastSender: boolean;
}