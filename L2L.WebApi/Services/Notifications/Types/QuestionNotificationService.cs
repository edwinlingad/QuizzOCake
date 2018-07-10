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
    public class QuestionNotificationService : BaseService
    {
        public QuestionNotificationService(BaseApiController controller)
            : base(controller)
        {
        }

        public bool AddQuestionFlagNotification(int questionId, bool callSaveChanges = true)
        {
            return AddQuestionNotification(NotificationTypeEnum.QuestionFlag, questionId, callSaveChanges);
        }

        private bool AddQuestionNotification(NotificationTypeEnum type, int questionId, bool callSaveChanges = true)
        {
            try
            {
                var question = _uow.Questions.GetById(questionId);

                if (question.AuthorId == _currentUser.Id)
                    return false;

                var entity = _uow.NewNotifications.GetAll()
                    .Where(n => n.ToUserId == n.Question.AuthorId
                        && n.NotificationType == type
                        && n.QuestionId == questionId)
                    .FirstOrDefault();

                if (entity == null)
                    CreateNewQuestionNotification(type, question);
                else
                    UpdateQuestionNotification(entity);

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

        private void CreateNewQuestionNotification(NotificationTypeEnum type, Question question)
        {
            var editor = new NewNotificationEditor(type, _currentUser.Id, question.AuthorId);
            editor.AddQuestion(question.Id);
            var entity = editor.GetEntity();
            _uow.NewNotifications.Add(entity);
        }

        private void UpdateQuestionNotification(NewNotification entity)
        {
            var editor = new NewNotificationEditor(entity);
            editor.AddNewFrom(_currentUser.Id);

            _uow.NewNotifications.Update(entity);
        }
    }
}