using L2L.Entities;
using L2L.Entities.Enums;
using L2L.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Services
{
    public class QuizzmateNotificationService : BaseService
    {
        public QuizzmateNotificationService(BaseApiController controller)
            : base(controller)
        {
        }

        public bool AddQuizzmateRequestAcceptNofication(FriendRequest friendRequest, bool callSaveChanges = true)
        {
            try
            {
                var editor = new NewNotificationEditor(NotificationTypeEnum.QuizzmateAccept, friendRequest.RequestToId, friendRequest.RequestFromId);

                var entity = editor.GetEntity();
                editor.AddQuizzmateRequest(friendRequest.Id);
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