using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.Controllers;
using System.Data.Entity;
using L2L.WebApi.Models;
using L2L.Entities;
using L2L.WebApi.Utilities;
using L2L.Entities.Enums;
namespace L2L.WebApi.Services
{
    public class NotificationTypeService : BaseService
    {
        public NotificationTypeService(BaseApiController controller)
            : base(controller)
        {
        }

        public IList<NotificationTypeModel> GetNotificationTypes()
        {
            return notificationTypeList;
        }

        // DEP-REQUEST
        private List<NotificationTypeModel> notificationTypeList = new List<NotificationTypeModel>() {
            new NotificationTypeModel { // 0
                NotificationType = NotificationTypeEnum.QuizzLike,
                FAIcon = "fa-thumbs-up",
                FgColor = "",
            },
            new NotificationTypeModel { // 1
                NotificationType = NotificationTypeEnum.QuizzComment,
                FAIcon = "fa-comment",
                FgColor = "",
            },
            new NotificationTypeModel { // 2
                NotificationType = NotificationTypeEnum.QuizzTake,
                FAIcon = "fa-list",
                FgColor = "",
            },
            new NotificationTypeModel { // 3
                NotificationType = NotificationTypeEnum.QuestionFlag,
                FAIcon = "fa-comment",
                FgColor = "",
            },
            new NotificationTypeModel { // 4
                NotificationType = NotificationTypeEnum.QuizzCommentLike,
                FAIcon = "fa-thumbs-up",
                FgColor = "",
            },
            new NotificationTypeModel { // 5
                NotificationType = NotificationTypeEnum.QuizzCommentFlag,
                FAIcon = "fa-flag",
                FgColor = "",
            },
            new NotificationTypeModel { // 6
                NotificationType = NotificationTypeEnum.DepQuizzSubmit,
                FAIcon = "fa-plus",
                FgColor = "",
            },
            new NotificationTypeModel { // 7
                NotificationType = NotificationTypeEnum.DepQuizzLive,
                FAIcon = "fa-fire",
                FgColor = "",
            },
            new NotificationTypeModel { // 8
                NotificationType = NotificationTypeEnum.DepQuizzReceiveComment,
                FAIcon = "fa-comment",
                FgColor = "",
            },
            new NotificationTypeModel { // 9
                NotificationType = NotificationTypeEnum.DepPostComment,
                FAIcon = "fa-comment",
                FgColor = "",
            },
            new NotificationTypeModel { // 10
                NotificationType = NotificationTypeEnum.DepPostedCommentModified,
                FAIcon = "fa-comment",
                FgColor = "",
            },
            new NotificationTypeModel { // 11
                NotificationType = NotificationTypeEnum.DepPostedCommentFlagged,
                FAIcon = "fa-exclamation-circle",
                FgColor = "",
            },
            new NotificationTypeModel { // 12
                NotificationType = NotificationTypeEnum.DepQuestionFlagged,
                FAIcon = "fa-exclamation-circle",
                FgColor = "",
            },
            new NotificationTypeModel { // 13
                NotificationType = NotificationTypeEnum.DepMessageSent,
                FAIcon = "fa-plus",
                FgColor = "",
            },
            new NotificationTypeModel { // 14
                NotificationType = NotificationTypeEnum.DepMessageReceived,
                FAIcon = "fa-bolt",
                FgColor = "",
            },
            new NotificationTypeModel { // 15
                NotificationType = NotificationTypeEnum.DepQuizzmateReceiveRequest,
                FAIcon = "fa-users",
                FgColor = "",
            },
            new NotificationTypeModel {  // 16
                NotificationType = NotificationTypeEnum.DepQuizzmateReceiveRequestAccept,
                FAIcon = "fa-user-plus",
                FgColor = "",
            },
            new NotificationTypeModel { // 17
                NotificationType = NotificationTypeEnum.AssignmentAssigned,
                FAIcon = "fa-list",
                FgColor = "",
            },
            new NotificationTypeModel { // 18
                NotificationType = NotificationTypeEnum.AssignmentFinished,
                FAIcon = "fa-check-square-o",
                FgColor = "",
            },
            new NotificationTypeModel { // 19
                NotificationType = NotificationTypeEnum.QuizzmateAccept,
                FAIcon = "fa-user-plus",
                FgColor = "",
            },
            new NotificationTypeModel { // 20
                NotificationType = NotificationTypeEnum.QuizzlingAccept,
                FAIcon = "fa-child",
                FgColor = "",
            },
            new NotificationTypeModel { // 21
                NotificationType = NotificationTypeEnum.DepQuizzmateReceiveRequestReject,
                FAIcon = "fa-user-times",
                FgColor = "",
            },
            new NotificationTypeModel { // 22
                NotificationType = NotificationTypeEnum.DepUnQuizzmate,
                FAIcon = "fa-user-times",
                FgColor = "",
            },
            new NotificationTypeModel { // 23
                NotificationType = NotificationTypeEnum.DepQuizzmateSendRequest,
                FAIcon = "fa-users",
                FgColor = "",
            },
            new NotificationTypeModel { // 24
                NotificationType = NotificationTypeEnum.DepQuizzmateSendRequestAccept,
                FAIcon = "fa-user-plus",
                FgColor = "",
            },
            new NotificationTypeModel { // 25
                NotificationType = NotificationTypeEnum.DepQuizzmateSendRequestCancel,
                FAIcon = "fa-user-times",
                FgColor = "",
            },
        };
    }
}