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
    public class QuizzmateRequestService : BaseService, IResource, IRelationshipResponse
    {
        public QuizzmateRequestService(BaseApiController controller)
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

        public object Post(object modelParam)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<FriendRequestModel>(modelParam.ToString());
                if (model == null)
                    return null;

                FriendRequest entity;
                model.MapToNew<FriendRequestModel, FriendRequest>(out entity);
                entity.RequestFromId = _currentUser.Id;
                entity.IsNew = true;
                entity.IsAccepted = null;
                entity.PostedDate = DateTime.UtcNow;

                _uow.FriendRequests.Add(entity);
                _uow.SaveChanges();

                MappingUtil.Map(entity, model);

                _svcContainer.NotificationSvc.DepQuizzmateNotificationSvc.AddDepQuizzmateReceiveRequestNotification(entity, false);
                _svcContainer.NotificationSvc.DepQuizzmateNotificationSvc.AddDepQuizzmateSendRequestNotification(entity, false);
                _svcContainer.RelationshipNotificationSvc.QuizzmateNotificationSvc.AddQuizzmateRequestRNotification(entity, true);
                _svcContainer.EmailSvc.SendQuizzmateRequest(entity);

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
            bool ret = true;
            switch (model.Response)
            {
                case RelationshipNotificationResponseEnum.Accept:
                    ProcesssAccepted(model);
                    break;
                case RelationshipNotificationResponseEnum.Reject:
                    ProccessRejected(model);
                    break;
                case RelationshipNotificationResponseEnum.Resend:
                    ProcessResend(model);
                    ret = false;
                    break;
                case RelationshipNotificationResponseEnum.Cancel:
                    ProcessCancel(model);
                    break;
                default:
                    break;
            }

            return ret;
        }

        private void ProcesssAccepted(RelationshipNotificationModel model)
        {
            var friendRequest = _uow.FriendRequests.GetById((int)model.FriendRequestId);
            int requestFromId = friendRequest.RequestFromId;
            int requestToId = friendRequest.RequestToId;

            friendRequest.IsAccepted = true;
            friendRequest.IsDeleted = true;
            _uow.FriendRequests.Update(friendRequest);

            var friendRelationship = new FriendRelationship
            {
                User1Id = friendRequest.RequestFromId,
                User2Id = friendRequest.RequestToId
            };

            _uow.FriendRelationships.Add(friendRelationship);
            _svcContainer.NotificationSvc.DepQuizzmateNotificationSvc.AddDepQuizzmateRequestAcceptNotification(friendRequest, false);
            _svcContainer.NotificationSvc.DepQuizzmateNotificationSvc.AddDepQuizzmateSendRequestAcceptNotification(friendRequest, false);
            _svcContainer.NotificationSvc.QuizzmateNotificationSvc.AddQuizzmateRequestAcceptNofication(friendRequest, false);
        }

        private void ProccessRejected(RelationshipNotificationModel model)
        {
            var friendRequest = _uow.FriendRequests.GetById((int)model.FriendRequestId);
          
            friendRequest.IsAccepted = false;
            friendRequest.IsDeleted = true;

            _svcContainer.NotificationSvc.DepQuizzmateNotificationSvc.AddDepQuizzmateRequesRejectNotification(friendRequest, false);
            _uow.FriendRequests.Update(friendRequest);
        }

        private void ProcessResend(RelationshipNotificationModel model)
        {
            RelationshipNotification entity;
            model.MapToNew(out entity);
            entity.IsNew = true;
            entity.PostedDate = DateTime.UtcNow;

            _uow.RelationshipNotifications.Update(entity);
            _uow.SaveChanges();

            var friendRequest = _uow.FriendRequests.GetById((int)model.FriendRequestId);
            _svcContainer.EmailSvc.ReSendQuizzmateRequest(friendRequest);
        }

        private void ProcessCancel(RelationshipNotificationModel model)
        {
            var friendRequest = _uow.FriendRequests.GetById((int)model.FriendRequestId);

            friendRequest.IsAccepted = false;
            friendRequest.IsDeleted = true;

            _svcContainer.NotificationSvc.DepQuizzmateNotificationSvc.AddDepQuizzmateSendRequestCancelNotification(friendRequest, false);
            _uow.FriendRequests.Update(friendRequest);
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