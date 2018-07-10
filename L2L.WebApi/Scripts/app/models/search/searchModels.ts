interface ISearchModel {
    searchType?: number;

    userId?: number;
    userName?: string;
    userFullName?: string;
    isQuizzmate?: boolean;
    profileImageUrl?: string;

    quizzId?: number;
    isLive?: boolean;
    quizzCategory?: number;
    quizzName?: string;
    quizzDescription?: string;
    quizzAuthor?: string;

    // common from query
    birthDate?: Date;

    // from query
    quizzClassId?: number;
    teacherId?: number;
    isRequestSent?: boolean;
    isInviteSent?: boolean;
    isMember?: boolean;
    teacherUserName?: string;
    teacherFullName?: string;
    className?: string;
    description?: string;
    qcTags?: string;
    imageUrl?: string;

    // from code
    userDisplayName?: string;
    teacherName?: string;
    isTeacher?: boolean;
    age?: number;
}