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
    public class DepQuizzCommentNotificationService : BaseService
    {
        public DepQuizzCommentNotificationService(BaseApiController controller)
            : base(controller)
        {
        }

        public bool AddDepPostCommentNotification(int quizzCommentId, bool callSaveChanges = true)
        {
            return AddQuizzCommentNotification(NotificationTypeEnum.DepPostComment, quizzCommentId, false, callSaveChanges);
        }

        public bool AddDepPostedCommentModifiedNotification(int quizzCommentId, bool callSaveChanges = true)
        {
            return AddQuizzCommentNotification(NotificationTypeEnum.DepPostedCommentModified, quizzCommentId, false, callSaveChanges);
        }

        public bool AddDepPostedCommentFlaggedNotification(int quizzCommentId, bool callSaveChanges = true)
        {
            return AddQuizzCommentNotification(NotificationTypeEnum.DepPostedCommentFlagged, quizzCommentId, false, callSaveChanges);
        }

        private bool AddQuizzCommentNotification(NotificationTypeEnum type, int quizzCommentId, bool depQuizzOwner = false, bool callSaveChanges = true)
        {
            try
            {
                User dependent;
                if (depQuizzOwner == true)
                {
                    dependent = _uow.QuizzComments.GetAll()
                        .Where(q => q.Id == quizzCommentId)
                        .Select(q => q.Quizz.Owner)
                        .Include(u => u.DependentPermission)
                        .Include(u => u.AsChildDependsOn.Select(d => d.User))
                        .FirstOrDefault();
                }
                else
                {
                    dependent = _uow.QuizzComments.GetAll()
                        .Where(q => q.Id == quizzCommentId)
                        .Select(q => q.Author)
                        .Include(u => u.DependentPermission)
                        .Include(u => u.AsChildDependsOn.Select(d => d.User))
                        .FirstOrDefault();
                }

                if (dependent.UserType == UserTypeEnum.Standard)
                    return true;

                var quizz = _uow.QuizzComments.GetAll()
                    .Where(q => q.Id == quizzCommentId)
                    .Select(q => q.Quizz)
                    .FirstOrDefault();

                foreach (var depEntity in dependent.AsChildDependsOn)
                {
                    if (_currentUser.Id == depEntity.User.Id)
                        continue;

                    if (NotificationTypeUtil.WillNotify(depEntity, type) == false)
                        continue;

                    var entity = _uow.NewNotifications.GetAll()
                            .Where(n => n.NotificationType == type
                                && n.QuizzCommentId == quizzCommentId
                                && n.ToUserId == depEntity.User.Id)
                            .FirstOrDefault();

                    if (entity == null)
                        CreateNewQuizzCommentNotification(type, quizz.Id, quizzCommentId, depEntity.UserId);
                    else
                        UpdateQuizzCommentNotification(entity);
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

        private void CreateNewQuizzCommentNotification(NotificationTypeEnum type, int quizzId, int quizzCommentId, int parentId)
        {
            var editor = new NewNotificationEditor(type, _currentUser.Id, parentId);
            editor.AddQuizz(quizzId);
            editor.AddQuizzComment(quizzCommentId);
            var entity = editor.GetEntity();
            _uow.NewNotifications.Add(entity);
        }

        private void UpdateQuizzCommentNotification(NewNotification entity)
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