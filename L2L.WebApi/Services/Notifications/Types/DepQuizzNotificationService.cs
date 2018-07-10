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
using AutoMapper.QueryableExtensions;

namespace L2L.WebApi.Services
{
    public class DepQuizzNotificationService : BaseService
    {
        public DepQuizzNotificationService(BaseApiController controller)
            : base(controller)
        {
        }

        public bool AddDepQuizzReceiveCommentNotification(int quizzId, bool callSaveChanges = true)
        {
            return AddQuizzNotification(NotificationTypeEnum.DepQuizzReceiveComment, quizzId, callSaveChanges);
        }

        public bool AddDepQuizzSubmitNotification(int quizzId, bool callSaveChanges = true)
        {
            return AddQuizzNotification(NotificationTypeEnum.DepQuizzSubmit, quizzId, callSaveChanges);
        }

        public bool AddDepQuizzLiveNotification(int quizzId, bool callSaveChanges = true)
        {
            return AddQuizzNotification(NotificationTypeEnum.DepQuizzLive, quizzId, callSaveChanges);
        }

        private bool AddQuizzNotification(NotificationTypeEnum type, int quizzId, bool callSaveChanges = true)
        {
            try
            {
                var dependent = _uow.Quizzes.GetAll()
                    .Where(q => q.Id == quizzId)
                    .Select(q => q.Owner)
                    .Include(u => u.DependentPermission)
                    .Include(u => u.AsChildDependsOn.Select(d => d.User))
                    .FirstOrDefault();

                if (dependent.UserType == UserTypeEnum.Standard)
                    return true;

                var quizz = _uow.Quizzes.GetById(quizzId);

                if (quizz == null)
                    return false;

                foreach (var depEntity in dependent.AsChildDependsOn)
                {
                    if (isParentCurrentUser(depEntity))
                        continue;

                    if (type == NotificationTypeEnum.DepQuizzReceiveComment && isCurrentUserQuizzOwner(quizz))
                        continue;

                    if (NotificationTypeUtil.WillNotify(depEntity, type) == false)
                        continue;

                    var entity = _uow.NewNotifications.GetAll()
                            .Where(n => n.NotificationType == type
                                && n.QuizzId == quizzId
                                && n.ToUserId == depEntity.User.Id)
                            .FirstOrDefault();

                    if (entity == null)
                        CreateNewQuizzNotification(type, quizzId, depEntity.UserId);
                    else
                        UpdateQuizzNotification(entity);
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

        private bool isCurrentUserQuizzOwner(Quizz quizz)
        {
            return quizz.OwnerId == _currentUser.Id;
        }

        private bool isParentCurrentUser(Dependent depEntity)
        {
            return _currentUser.Id == depEntity.User.Id;
        }

        private void CreateNewQuizzNotification(NotificationTypeEnum type, int quizzId, int parentId)
        {
            var editor = new NewNotificationEditor(type, _currentUser.Id, parentId);
            editor.AddQuizz(quizzId);
            var entity = editor.GetEntity();
            _uow.NewNotifications.Add(entity);
        }

        private void UpdateQuizzNotification(NewNotification entity)
        {
            var editor = new NewNotificationEditor(entity);
            editor.AddNewFrom(_currentUser.Id);

            _uow.NewNotifications.Update(entity);
        }

        private DependentService __dependentSvc;
        private DependentService _dependentSvc
        {
            get
            {
                if (__dependentSvc == null)
                    __dependentSvc = _svcContainer.DependentSvc;
                return __dependentSvc;
            }
        }
    }
}