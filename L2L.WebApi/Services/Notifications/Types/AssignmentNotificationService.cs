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
    public class AssignmentNotificationService : BaseService
    {
        public AssignmentNotificationService(BaseApiController controller)
            : base(controller)
        {
        }

        public bool AddAssignmentAssignedNotification(int quizzId, int assignmentGroupId, int assignmentId, int toId, bool callSaveChanges = true)
        {
            try
            {
                var editor = new NewNotificationEditor(NotificationTypeEnum.AssignmentAssigned, _currentUser.Id, toId);
                editor.AddQuizz(quizzId);
                editor.AddAssignmentGroup(assignmentGroupId);
                editor.AddAssignment(assignmentId);
                var entity = editor.GetEntity();
                _uow.NewNotifications.Add(entity);

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

        public bool AddAssignmentFinishedNotification(int quizzId, int assignmentGroupId, int toId, bool callSaveChanges = true)
        {
            try
            {
                var entity = _uow.NewNotifications.GetAll()
                    .Where(n => n.ToUserId == n.Quizz.OwnerId
                        && n.NotificationType == NotificationTypeEnum.AssignmentFinished
                        && n.AssignmentGroupId == assignmentGroupId)
                    .FirstOrDefault();

                if (entity == null)
                    CreateNewAssignmentFinishedNotification(quizzId, assignmentGroupId, toId);
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

        private void CreateNewAssignmentFinishedNotification(int quizzId, int assignmentGroupId, int toId)
        {
            var editor = new NewNotificationEditor(NotificationTypeEnum.AssignmentFinished, _currentUser.Id, toId);
            editor.AddQuizz(quizzId);
            editor.AddAssignmentGroup(assignmentGroupId);
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