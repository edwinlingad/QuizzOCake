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
    public class QuizzNotificationService : BaseService
    {
        public QuizzNotificationService(BaseApiController controller)
            : base(controller)
        {
        }

        public bool AddQuizzLikeNotification(int quizzId, bool callSaveChanges = true)
        {
            return AddQuizzNotification(NotificationTypeEnum.QuizzLike, quizzId, callSaveChanges);
        }

        public bool AddQuizzCommentNotification(int quizzId, bool callSaveChanges = true)
        {
            return AddQuizzNotification(NotificationTypeEnum.QuizzComment, quizzId, callSaveChanges);
        }

        public bool AddQuizzTakeNotification(int quizzId, bool callSaveChanges = true)
        {
            return AddQuizzNotification(NotificationTypeEnum.QuizzTake, quizzId, callSaveChanges);
        }

        private bool AddQuizzNotification(NotificationTypeEnum type, int quizzId, bool callSaveChanges = true)
        {
            try
            {
                var quizz = _uow.Quizzes.GetById(quizzId);

                if (quizz.OwnerId == _currentUser.Id)
                    return false;

                var entity = _uow.NewNotifications.GetAll()
                    .Where(n => n.ToUserId == n.Quizz.OwnerId
                        && n.NotificationType == type
                        && n.QuizzId == quizzId)
                    .FirstOrDefault();

                if (entity == null)
                    CreateNewQuizzNotification(type, quizz);
                else
                    UpdateQuizzNotification(entity);

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

        private void CreateNewQuizzNotification(NotificationTypeEnum type, Quizz quizz)
        {
            var editor = new NewNotificationEditor(type, _currentUser.Id, quizz.OwnerId);
            editor.AddQuizz(quizz.Id);
            var entity = editor.GetEntity();
            _uow.NewNotifications.Add(entity);
        }

        private void UpdateQuizzNotification(NewNotification entity)
        {
            var editor = new NewNotificationEditor(entity);
            editor.AddNewFrom(_currentUser.Id);
            
            _uow.NewNotifications.Update(entity);
        }
    }
}