using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.Entities;
using L2L.Entities.Enums;

namespace L2L.WebApi.Services
{
    public class NewNotificationEditor
    {
        private NewNotification _entity;
        public NewNotificationEditor(NewNotification entity)
        {
            _entity = entity;
        }

        public NewNotificationEditor(NotificationTypeEnum type, int FromId, int ToId)
        {
            _entity = new NewNotification
            {
                PostedDate = DateTime.UtcNow,
                NotificationType = type,
                IsNew = true,
                ToUserId = ToId,
                FromUserId = FromId,
                OldFromUser = "",
                NewFromUser = FromId.ToString(),
                QuizzId = 1,
                QuizzCommentId = 1,
                QuestionId = 1,
                AssignmentGroupId = 1,
                AssignmentId = 1,
            };
        }

        public NewNotification GetEntity()
        {
            return _entity;
        }

        public void AddQuizz(int quizzId)
        {
            _entity.QuizzId = quizzId;
        }

        public void AddQuizzComment(int quizzCommentId)
        {
            _entity.QuizzCommentId = quizzCommentId;
        }

        public void AddQuestion(int questionId)
        {
            _entity.QuestionId = questionId;
        }

        public void AddAssignment(int assignmentId)
        {
            _entity.AssignmentId = assignmentId;
        }

        public void AddAssignmentGroup(int assignmentGroupId)
        {
            _entity.AssignmentGroupId = assignmentGroupId;
        }

        public void AddQuizzmateRequest(int friendRequestId)
        {
            _entity.FriendRequestId = friendRequestId;
        }

        public void AddToUnQuizzmateUser(int toUnQuizzmateId)
        {
            _entity.ToUnQuizzmateId = toUnQuizzmateId;
        }

        public void AddNewFrom(int fromId)
        {
            if (_entity.IsNew == false)
                _entity.IsNew = true;

            _entity.FromUserId = fromId;
            _entity.PostedDate = DateTime.UtcNow;
            var newUserList = _entity.NewFromUser.Split(',');
            if (newUserList.Contains(fromId.ToString()) == false)
                _entity.NewFromUser += "," + fromId.ToString();
        }
    }
}