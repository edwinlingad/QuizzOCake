using L2L.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.Entities;

namespace L2L.WebApi.Services
{
    public class QuizzlingRNotificationService: BaseService
    {
        public QuizzlingRNotificationService(BaseApiController controller)
            : base(controller)
        {
        }

        public bool AddQuizzlingRequestRNotification(DependentRequestFromUser depRequest, bool callSaveChanges = true)
        {
            try
            {
                var primaryParent = _uow.Users.GetAll()
                    .Where(u => u.Id == depRequest.ToChildId)
                    .Select(u => u.AsChildDependsOn.Where(d => d.IsPrimary == true).FirstOrDefault().User)
                    .FirstOrDefault();

                var editor = new RelationshipNotificationEditor(RelationshipNotificationTypeEnum.QuizzlingRequest, depRequest.FromUserId, primaryParent.Id);
                editor.AddDependentRequest(depRequest.Id);

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