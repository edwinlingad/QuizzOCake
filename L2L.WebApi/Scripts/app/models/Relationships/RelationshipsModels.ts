interface IDependentRequestFromUserModel {
    id: number;
    toChildId: number;
    fromUserId: number;
    isAccepted: boolean;
}

interface IFriendRequestModel {
    id?: number;
    message: string;
    isNew?: boolean;
    isAccepted?: boolean;
    requestToId: number;
    requestFromId?: number;
}

interface IFriendRelationshipModel {
    id: number;
    user1Id: number;
    user2Id: number;
}

interface IRelationshipNotificationModel {
    id?: number;
    rnType?: any;
    postedDate?: Date;
    isNew?: boolean;
    toUserId?: number;
    fromUserId?: number;
    friendRequestId?: number;
    dependentRequestFromUserId?: number;

    toUserName?: string;
    fromUserName?: string;
    fromUserFullName?: string;
    quizzmateRequestMessage?: string;

    isAccepted?: boolean;
    response?: number;

    disabled?: boolean;
    acceptProcessing?: IItemToWait;
    rejectProcessing?: IItemToWait;
    resendProcessing?: IItemToWait;
    cancelProcessing?: IItemToWait;
}