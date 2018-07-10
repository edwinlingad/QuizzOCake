using L2L.Entities;
using L2L.Entities.Enums;
using L2L.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Services
{
    public class QuizzlingNotificationService : BaseService
    {
        public QuizzlingNotificationService(BaseApiController controller)
            : base(controller)
        {
        }

        public bool AddQuizzlingRequestAcceptNotification(DependentRequestFromUser depRequest, bool callSaveChanges = true)
        {
            try
            {
                var editor = new NewNotificationEditor(NotificationTypeEnum.QuizzlingAccept, depRequest.ToChildId, depRequest.FromUserId);

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
    }
}