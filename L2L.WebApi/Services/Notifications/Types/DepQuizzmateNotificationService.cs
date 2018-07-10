using L2L.Entities;
using L2L.Entities.Enums;
using L2L.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace L2L.WebApi.Services
{
    public class DepQuizzmateNotificationService : BaseService
    {
        public DepQuizzmateNotificationService(BaseApiController controller)
            : base(controller)
        {
        }

        public bool AddDepQuizzmateReceiveRequestNotification(FriendRequest friendRequest, bool callSaveChanges = true)
        {
            return AddDepQuizzmateNotification(NotificationTypeEnum.DepQuizzmateReceiveRequest, friendRequest, friendRequest.RequestToId, callSaveChanges);
        }

        public bool AddDepQuizzmateSendRequestNotification(FriendRequest friendRequest, bool callSaveChanges = true)
        {
            return AddDepQuizzmateNotification(NotificationTypeEnum.DepQuizzmateSendRequest, friendRequest, friendRequest.RequestFromId, callSaveChanges);
        }

        public bool AddDepQuizzmateRequestAcceptNotification(FriendRequest friendRequest, bool callSaveChanges = true)
        {
            return AddDepQuizzmateNotification(NotificationTypeEnum.DepQuizzmateReceiveRequestAccept, friendRequest, friendRequest.RequestToId, callSaveChanges);
        }

        public bool AddDepQuizzmateSendRequestAcceptNotification(FriendRequest friendRequest, bool callSaveChanges = true)
        {
            return AddDepQuizzmateNotification(NotificationTypeEnum.DepQuizzmateSendRequestAccept, friendRequest, friendRequest.RequestFromId, callSaveChanges);
        }

        public bool AddDepQuizzmateRequesRejectNotification(FriendRequest friendRequest, bool callSaveChanges = true)
        {
            return AddDepQuizzmateNotification(NotificationTypeEnum.DepQuizzmateReceiveRequestReject, friendRequest, friendRequest.RequestToId, callSaveChanges);
        }

       

        public bool AddDepQuizzmateSendRequestCancelNotification(FriendRequest friendRequest, bool callSaveChanges = true)
        {
            return AddDepQuizzmateNotification(NotificationTypeEnum.DepQuizzmateSendRequestCancel, friendRequest, friendRequest.RequestFromId, callSaveChanges);
        }

        private bool AddDepQuizzmateNotification(NotificationTypeEnum type, FriendRequest friendRequest, int dependentId, bool callSaveChanges = true)
        {
            try
            {
                User dependent = _uow.Users.GetAll()
                            .Where(u => u.Id == dependentId)
                            .Include(u => u.DependentPermission)
                            .Include(u => u.AsChildDependsOn.Select(d => d.User))
                            .FirstOrDefault();

                AddDepQuizzmateNotification(type, dependent, friendRequest);

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

        private bool AddDepQuizzmateNotification(NotificationTypeEnum type, User dependent, FriendRequest friendRequest)
        {
            if (dependent.UserType == UserTypeEnum.Standard)
                return true;

            foreach (var depEntity in dependent.AsChildDependsOn)
            {
                if (depEntity.UserId == friendRequest.RequestFromId)
                    continue;
                if (NotificationTypeUtil.WillNotify(depEntity, type) == false)
                    continue;

                CreateNewQuizzmateNotification(type, friendRequest.Id, dependent.Id, depEntity.UserId);
            }

            return true;
        }

        private void CreateNewQuizzmateNotification(NotificationTypeEnum type, int friendRequestId, int quizzlingId, int parentId)
        {
            var editor = new NewNotificationEditor(type, quizzlingId, parentId);
            editor.AddQuizzmateRequest(friendRequestId);
            var entity = editor.GetEntity();
            _uow.NewNotifications.Add(entity);
        }

        public bool AddDepUnQuizzmateNotification(int toUnQuizzmateId, bool callSaveChanges = true)
        {
            try
            {
                var quizzlingId = _currentUser.Id;
                User dependent = _uow.Users.GetAll()
                            .Where(u => u.Id == quizzlingId)
                            .Include(u => u.DependentPermission)
                            .Include(u => u.AsChildDependsOn.Select(d => d.User))
                            .FirstOrDefault();

                if (dependent.UserType == UserTypeEnum.Standard)
                    return true;

                foreach (var depEntity in dependent.AsChildDependsOn)
                {
                    if (NotificationTypeUtil.WillNotify(depEntity, NotificationTypeEnum.DepUnQuizzmate) == false)
                        continue;

                    var editor = new NewNotificationEditor(NotificationTypeEnum.DepUnQuizzmate, quizzlingId, depEntity.UserId);
                    editor.AddToUnQuizzmateUser(toUnQuizzmateId);
                    var entity = editor.GetEntity();
                    _uow.NewNotifications.Add(entity);
                }

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