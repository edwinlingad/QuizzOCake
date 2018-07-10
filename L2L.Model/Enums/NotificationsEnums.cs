using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities.Enums
{
    // DEP-REQUEST
    // NOTIFICATION-ENUMS
    public enum NotificationTypeEnum
    {
        QuizzLike,                          // 0
        QuizzComment,                       // 1
        QuizzTake,                          // 2

        QuestionFlag,                       // 3
        
        QuizzCommentLike,                   // 4
        QuizzCommentFlag,                   // 5

        DepQuizzSubmit,                     // 6
        DepQuizzLive,                       // 7

        DepQuizzReceiveComment,             // 8

        DepPostComment,                     // 9
        DepPostedCommentModified,           // 10
        DepPostedCommentFlagged,            // 11

        DepQuestionFlagged,                 // 12

        DepMessageSent,                     // 13
        DepMessageReceived,                 // 14

        DepQuizzmateReceiveRequest,         // 15
        DepQuizzmateReceiveRequestAccept,   // 16

        AssignmentAssigned,                 // 17
        AssignmentFinished,                 // 18

        QuizzmateAccept,                    // 19
        QuizzlingAccept,                    // 20

        DepQuizzmateReceiveRequestReject,   // 21
        DepUnQuizzmate,                     // 22

        DepQuizzmateSendRequest,            // 23
        DepQuizzmateSendRequestAccept,      // 24
        DepQuizzmateSendRequestCancel,      // 25
    }

    public enum QuizzCommentSortTypeEnum
    {
        Popular = 0, 
        Latest
    }
}
