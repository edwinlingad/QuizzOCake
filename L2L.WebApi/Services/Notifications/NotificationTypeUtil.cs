using L2L.Data;
using L2L.Entities;
using L2L.Entities.Enums;
using L2L.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Services
{
    public class NotificationTypeUtil
    {
        ApplicationUnit _uow;
        public NotificationTypeUtil(ApplicationUnit uow)
        {
            _uow = uow;
        }

        public IList<NotificationModel> ConvertToNotificationModelList(IList<Notification> list)
        {
            var modelList = new List<NotificationModel>();
            foreach (var item in list)
            {
                var model = CreateNotificationModel(item);
                modelList.Add(model);
            }

            return modelList;
        }

        public NotificationModel CreateNotificationModel(Notification notification)
        {
            var model = new NotificationModel()
            {
                NotificationType = notification.NotificationType,
                IsNew = notification.IsNew,
                Count = notification.Count,
                PostedDate = notification.PostedDate.ToLocalTime()
            };

            switch (notification.NotificationType)
            {
                case NotificationTypeEnum.QuizzLike:
                case NotificationTypeEnum.QuizzComment:
                case NotificationTypeEnum.QuizzTake:

                case NotificationTypeEnum.DepQuizzSubmit:
                case NotificationTypeEnum.DepQuizzLive:
                case NotificationTypeEnum.DepQuizzReceiveComment:

                    CreateQuizzModel(notification, ref model);
                    break;

                case NotificationTypeEnum.QuizzCommentLike:
                case NotificationTypeEnum.QuizzCommentFlag:

                case NotificationTypeEnum.DepPostComment:
                case NotificationTypeEnum.DepPostedCommentModified:
                case NotificationTypeEnum.DepPostedCommentFlagged:
                case NotificationTypeEnum.DepQuestionFlagged:

                    CreateQuizzCommentModel(notification, ref model);
                    break;
                case NotificationTypeEnum.QuestionFlag:
                    CreateQuestionModel(notification, ref model);
                    break;
                default:
                    break;
            }

            return model;
        }

        private void CreateQuizzModel(Notification notification, ref NotificationModel model)
        {
            var actualNotification = notification.QuizzNotification;
            var sources = actualNotification.NotificationSources;

            model.UserId = actualNotification.UserId;
            model.QuizzId = actualNotification.QuizzId;
            model.UserName = actualNotification.User.UserName;
            model.QuizzTitle = actualNotification.Quizz.Title;
            model.QuizzAuthorUserName = actualNotification.Quizz.Owner.UserName;
            model.NewCount = sources.Count();

            switch (notification.NotificationType)
            {
                case NotificationTypeEnum.DepQuizzSubmit:
                case NotificationTypeEnum.DepQuizzLive:
                case NotificationTypeEnum.DepQuizzReceiveComment:
                    model.QuizzAuthorFullName = actualNotification.Quizz.Owner.Profile.FirstName + " " + actualNotification.Quizz.Owner.Profile.LastName;
                    model.FullName = actualNotification.User.Profile.FirstName + " " + notification.QuizzNotification.User.Profile.LastName;
                    break;
                default:
                    break;
            }

            UpdateCount(notification, ref model);

            for (int i = sources.Count() - 1; i >= 0; i--)
            {
                _uow.QuizzNotificationSources.Delete(sources[i].Id);
            }
        }

        private void UpdateCount(Notification notification, ref NotificationModel model)
        {
            var quizzNotification = notification.QuizzNotification;
            switch (notification.NotificationType)
            {
                case NotificationTypeEnum.QuizzLike:
                    break;
                case NotificationTypeEnum.DepQuizzReceiveComment:
                case NotificationTypeEnum.QuizzComment:
                    model.Count = _uow.Quizzes.GetAll()
                        .Where(q => q.Id == quizzNotification.QuizzId)
                        .Select(q => q.Comments.Select(c => c.AuthorId).Distinct().Count())
                        .FirstOrDefault();
                    break;
                case NotificationTypeEnum.QuizzTake:
                    model.Count = _uow.QuizLogs.GetAll()
                        .Where(l => l.QuizzId == quizzNotification.QuizzId)
                        .Select(l => l.UserId)
                        .Distinct()
                        .Count();
                    break;
                case NotificationTypeEnum.QuestionFlag:
                    break;
                case NotificationTypeEnum.QuizzCommentLike:
                    break;
                case NotificationTypeEnum.QuizzCommentFlag:
                    break;
                case NotificationTypeEnum.DepQuizzSubmit:
                    break;
                case NotificationTypeEnum.DepQuizzLive:
                    break;
                case NotificationTypeEnum.DepPostComment:
                    break;
                case NotificationTypeEnum.DepPostedCommentModified:
                    break;
                case NotificationTypeEnum.DepPostedCommentFlagged:
                    break;
                case NotificationTypeEnum.DepQuestionFlagged:
                    break;
                case NotificationTypeEnum.DepMessageSent:
                    break;
                case NotificationTypeEnum.DepMessageReceived:
                    break;
                case NotificationTypeEnum.DepQuizzmateReceiveRequest:
                    break;
                case NotificationTypeEnum.DepQuizzmateReceiveRequestAccept:
                    break;
                default:
                    break;
            }
        }

        private void CreateQuizzCommentModel(Notification notification, ref NotificationModel model)
        {
            var actualNotification = notification.QuizzCommentNotification;
            var sources = actualNotification.NotificationSources;

            model.UserId = actualNotification.UserId;
            model.UserName = actualNotification.User.UserName;

            model.QuizzId = actualNotification.QuizzComment.Quizz.Id;
            model.QuizzTitle = actualNotification.QuizzComment.Quizz.Title;
            model.QuizzCommentId = actualNotification.QuizzCommentId;
            model.QuizzComment = actualNotification.QuizzComment.Comment;
            model.QuizzCommentAuthorUserName = actualNotification.QuizzComment.Author.UserName;

            if (model.QuizzComment.Length > 64)
            {
                model.QuizzComment = model.QuizzComment.Substring(0, 64) + "...";
            }
            model.NewCount = sources.Count();

            switch (notification.NotificationType)
            {

                case NotificationTypeEnum.DepPostedCommentModified:
                case NotificationTypeEnum.DepPostComment:
                case NotificationTypeEnum.DepPostedCommentFlagged:
                case NotificationTypeEnum.DepQuestionFlagged:
                    model.FullName = notification.QuizzCommentNotification.User.Profile.FirstName + " " + notification.QuizzCommentNotification.User.Profile.LastName;
                    model.QuizzCommentAuthorFullName = actualNotification.QuizzComment.Author.Profile.FirstName + " " + actualNotification.QuizzComment.Author.Profile.LastName;
                    break;
                default:
                    break;
            }

            for (int i = sources.Count() - 1; i >= 0; i--)
            {
                _uow.QuizzCommentNotificationSources.Delete(sources[i].Id);
            }
        }

        private void CreateQuestionModel(Notification notification, ref NotificationModel model)
        {
            var actualNotification = notification.QuestionNotification;
            var sources = actualNotification.NotificationSources;

            model.UserId = actualNotification.UserId;
            model.QuestionId = actualNotification.QuestionId;
            model.UserName = actualNotification.User.UserName;
            // TODO: add get actual question

            model.NewCount = sources.Count();

            for (int i = sources.Count() - 1; i >= 0; i--)
            {
                _uow.QuestionNotificationSources.Delete(sources[i].Id);
            }
        }

        // DEP-REQUEST
        public static bool WillNotify(Dependent subscription, NotificationTypeEnum type)
        {
            bool ret = false;
            switch (type)
            {
                case NotificationTypeEnum.DepQuizzSubmit:
                    ret = subscription.NwQuizzSubmit;
                    break;
                case NotificationTypeEnum.DepQuizzLive:
                    ret = subscription.NwQuizzLive;
                    break;
                case NotificationTypeEnum.DepQuizzReceiveComment:
                    ret = subscription.NwQuizzReceiveComment;
                    break;
                case NotificationTypeEnum.DepPostComment:
                case NotificationTypeEnum.DepPostedCommentModified:
                    ret = subscription.NwPostComment;
                    break;
                case NotificationTypeEnum.DepPostedCommentFlagged:
                    ret = subscription.NwPostedCommentFlagged;
                    break;
                case NotificationTypeEnum.DepQuestionFlagged:
                    ret = subscription.NwQuestionFlagged;
                    break;
                case NotificationTypeEnum.DepMessageSent:
                    ret = subscription.NwMessageSent;
                    break;
                case NotificationTypeEnum.DepMessageReceived:
                    ret = subscription.NwMessageReceived;
                    break;
                case NotificationTypeEnum.DepQuizzmateReceiveRequest:
                case NotificationTypeEnum.DepQuizzmateSendRequest:
                    ret = subscription.NwQuizzmateRequest;
                    break;
                case NotificationTypeEnum.DepQuizzmateReceiveRequestAccept:
                case NotificationTypeEnum.DepQuizzmateReceiveRequestReject:
                case NotificationTypeEnum.DepQuizzmateSendRequestCancel:
                case NotificationTypeEnum.DepQuizzmateSendRequestAccept:
                case NotificationTypeEnum.DepUnQuizzmate:
                    ret = subscription.NwQuizzmateRequestAccept;
                    break;
                default:
                    break;
            }

            return ret;
        }

    }
}