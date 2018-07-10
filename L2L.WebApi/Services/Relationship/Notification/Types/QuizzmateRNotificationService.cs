using L2L.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.Entities;

namespace L2L.WebApi.Services
{
    public class QuizzmateRNotificationService : BaseService
    {
        public QuizzmateRNotificationService(BaseApiController controller)
            : base(controller)
        {
        }

        public bool AddQuizzmateRequestRNotification(FriendRequest friendRequest, bool callSaveChanges = true)
        {
            try
            {
                var editor = new RelationshipNotificationEditor(RelationshipNotificationTypeEnum.QuizzmateRequest, _currentUser.Id, friendRequest.RequestToId);
                editor.AddFriendRequest(friendRequest.Id);

                var entity = editor.GetEntity();

                _uow.RelationshipNotifications.Add(entity);

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