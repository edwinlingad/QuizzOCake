using L2L.Entities;
using L2L.WebApi.Controllers;
using L2L.WebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.Utilities;

namespace L2L.WebApi.Services
{
    public class QuizzlingRequestService : BaseService, IResource, IRelationshipResponse
    {
        public QuizzlingRequestService(BaseApiController controller)
            : base(controller)
        {
        }

        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            throw new NotImplementedException();
        }

        public object Get(int id)
        {
            throw new NotImplementedException();
        }
        
        // request quizzling        
        public object Post(object modelParam)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<DependentRequestFromUserModel>(modelParam.ToString());
                if (model == null)
                    return null;

                DependentRequestFromUser entity;
                model.MapToNew(out entity);
                entity.FromUserId = _currentUser.Id;

                _uow.DependentRequestsFromUser.Add(entity);
                _uow.SaveChanges();

                MappingUtil.Map(entity, model);

                _svcContainer.RelationshipNotificationSvc.QuizzlingNotificationSvc.AddQuizzlingRequestRNotification(entity, true);

                return model;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        public bool Patch(object modelParam)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool ProccessResponse(RelationshipNotificationModel model)
        {
            if (model.IsAccepted == true)
                ProcesssAccepted(model);
            else
                ProccessRejected(model);

            return true;
        }

        private void ProcesssAccepted(RelationshipNotificationModel model)
        {
            var depRequest = _uow.DependentRequestsFromUser.GetById((int)model.DependentRequestFromUserId);
            int childId = depRequest.ToChildId;
            int userId = depRequest.FromUserId;
            _uow.DependentRequestsFromUser.Delete((int)model.DependentRequestFromUserId);

            var dependentEntity = new Dependent();
            dependentEntity.ChildId = childId;
            dependentEntity.UserId = userId;
            dependentEntity.IsPrimary = false;

            _uow.Dependents.Add(dependentEntity);
            _svcContainer.NotificationSvc.QuizzlingNotificationSvc.AddQuizzlingRequestAcceptNotification(depRequest);
        }

        private void ProccessRejected(RelationshipNotificationModel model)
        {
            _uow.DependentRequestsFromUser.Delete((int)model.DependentRequestFromUserId);           
        }

        public object GetManyAlt(int id, int id2, int id3, string str, string str2, string str3)
        {
            throw new NotImplementedException();
        }

        public object GetAlt(string str)
        {
            throw new NotImplementedException();
        }
    }
}