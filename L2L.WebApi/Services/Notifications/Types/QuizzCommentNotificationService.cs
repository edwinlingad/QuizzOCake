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
    public class QuizzCommentNotificationService : BaseService
    {
        public QuizzCommentNotificationService(BaseApiController controller)
            : base(controller)
        {
        }

        public bool AddQuizzCommentLikeNotification(int quizzCommentId, bool callSaveChanges = true)
        {
            return AddQuizzCommentNotification(NotificationTypeEnum.QuizzCommentLike, quizzCommentId, callSaveChanges);
        }

        public bool AddQuizzCommentFlagNotification(int quizzCommentId, bool callSaveChanges = true)
        {
            return AddQuizzCommentNotification(NotificationTypeEnum.QuizzCommentFlag, quizzCommentId, callSaveChanges);
        }

        private bool AddQuizzCommentNotification(NotificationTypeEnum type, int quizzCommentId, bool callSaveChanges = true)
        {
            try
            {
                var quizzComment = _uow.QuizzComments.GetById(quizzCommentId);

                if (quizzComment.AuthorId == _currentUser.Id)
                    return true;

                var entity = _uow.NewNotifications.GetAll()
                    .Where(n => n.NotificationType == type
                        && n.QuizzCommentId == quizzCommentId
                        && n.ToUserId == n.QuizzComment.AuthorId)
                    .FirstOrDefault();

                if (entity == null)
                    CreateNewQuizzCommentNotification(type, quizzComment);
                else
                    UpdateQuizzCommentNotification(entity);

                if (callSaveChanges)
                    _uow.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        private void CreateNewQuizzCommentNotification(NotificationTypeEnum type, QuizzComment quizzComment)
        {
            var editor = new NewNotificationEditor(type, _currentUser.Id, quizzComment.AuthorId);
            editor.AddQuizz(quizzComment.QuizzId);
            editor.AddQuizzComment(quizzComment.Id);
            var entity = editor.GetEntity();
            _uow.NewNotifications.Add(entity);
        }

        private void UpdateQuizzCommentNotification(NewNotification entity)
        {
            var editor = new NewNotificationEditor(entity);
            editor.AddNewFrom(_currentUser.Id);

            _uow.NewNotifications.Update(entity);
        }
    }
}