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
    public class DeleteNotificationService : BaseService
    {
        public DeleteNotificationService(BaseApiController controller)
            : base(controller)
        {
        }

        public bool DeleteQuizzNotifications(int quizzId)
        {
            try
            {
                var list = _uow.NewNotifications.GetAll()
                    .Where(n => n.QuizzId == quizzId)
                    .ToList();

                foreach (var item in list)
                    _uow.NewNotifications.Delete(item.Id);

                return true;
            }
            catch (Exception ex)
            {
                _loggingSvc.Log(ex);
                return false;
            }
        }

        public bool DeleteQuizzCommentNotifications(int quizzCommentId)
        {
            try
            {
                var list = _uow.NewNotifications.GetAll()
                    .Where(n => n.QuizzCommentId == quizzCommentId)
                    .ToList();

                foreach (var item in list)
                    _uow.NewNotifications.Delete(item.Id);

                return true;
            }
            catch (Exception ex)
            {
                _loggingSvc.Log(ex);
                return false;
            }
        }

        #region Assignments
        public bool DeleteAssignmentNotification(int assGId)
        {
            try
            {
                var list = _uow.NewNotifications.GetAll()
                    .Where(n => n.AssignmentGroupId == assGId && n.FromUserId == _currentUser.Id)
                    .ToList();

                foreach (var item in list)
                    _uow.NewNotifications.Delete(item.Id);

                return true;
            }
            catch (Exception ex)
            {
                _loggingSvc.Log(ex);
                return false;
            }

        }
        #endregion

        private LoggingService __loggingSvc;
        private LoggingService _loggingSvc
        {
            get
            {
                if (__loggingSvc == null)
                    __loggingSvc = _svcContainer.LoggingSvc;
                return __loggingSvc;
            }
        }
    }
}