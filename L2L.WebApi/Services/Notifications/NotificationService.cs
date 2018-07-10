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
    public class NotificationService : BaseService
    {
        public NotificationService(BaseApiController controller)
            : base(controller)
        {
        }

        public int GetNewNotificationCount()
        {
            try
            {
                var count = _uow.NewNotifications.GetAll()
                    .Where(n => n.ToUserId == _currentUser.Id && n.IsNew == true)
                    .Count();
                return count;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return 0;
            }
        }

        public IList<NewNotificationModel> GetNewNotifications()
        {
            try
            {
                int newCount = GetNewNotificationCount();
                int total = newCount < 5 ? 5 : newCount;
                total = total > 10 ? 10 : total;

                var modelList = _uow.NewNotifications.GetAll()
                    .Where(n => n.ToUserId == _currentUser.Id)
                    .OrderByDescending(n => n.PostedDate)
                    .Take(total)
                    .ProjectTo<NewNotificationModel>()
                    .ToList();

                if (GuestUtil.GetGuestList().Contains(_currentUser.UserName) == false)
                {
                    foreach (var item in modelList)
                    {
                        FillOtherAttributes(item);
                        if (item.IsNew == true)
                        {
                            item.OldFromUser = item.OldFromUser + item.NewFromUser;
                            NewNotification entity;
                            item.MapToNew<NewNotificationModel, NewNotification>(out entity);
                            entity.IsNew = false;
                            _uow.NewNotifications.Update(entity);
                        }
                    }

                    _uow.SaveChanges();
                }

                return modelList;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        private void FillOtherAttributes(NewNotificationModel item)
        {
            var oldList = item.OldFromUser.Split(',');
            var newList = item.NewFromUser.Split(',');
            item.Count = oldList.Count();
            item.NewCount = newList.Count();
            if (item.QuizzCommentValue.Length > 32)
                item.QuizzCommentValue = item.QuizzCommentValue.Substring(0, 32) + "...";
        }

        public int GetAllNotificationsCount()
        {
            try
            {
                return _uow.NewNotifications.GetAll()
                    .Where(n => n.ToUserId == _currentUser.Id)
                    .Count();
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return 0;
            }
        }

        public IList<NewNotificationModel> GetNotifications(int pageNum, int numPerPage)
        {
            try
            {
                var modelList = _uow.NewNotifications.GetAll()
                    .Where(n => n.ToUserId == _currentUser.Id)
                    .OrderByDescending(n => n.PostedDate)
                    .Skip((pageNum - 1) * numPerPage)
                    .Take(numPerPage)
                    .ProjectTo<NewNotificationModel>()
                    .ToList();

                if (GuestUtil.GetGuestList().Contains(_currentUser.UserName) == false)
                {
                    foreach (var item in modelList)
                    {
                        FillOtherAttributes(item);
                        if (item.IsNew == true)
                        {
                            item.OldFromUser = item.OldFromUser + item.NewFromUser;
                            NewNotification entity;
                            item.MapToNew<NewNotificationModel, NewNotification>(out entity);
                            entity.IsNew = false;
                            _uow.NewNotifications.Update(entity);
                        }
                    }

                    _uow.SaveChanges();
                }

                return modelList;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        private NotificationTypeUtil __notificationTypeUtil;
        private NotificationTypeUtil _notificationTypeUtil
        {
            get
            {
                if (__notificationTypeUtil == null)
                    __notificationTypeUtil = new NotificationTypeUtil(_uow);
                return __notificationTypeUtil;
            }
        }

        private QuizzNotificationService _quizzNotificationSvc;
        public QuizzNotificationService QuizzNotificationSvc
        {
            get
            {
                if (_quizzNotificationSvc == null)
                    _quizzNotificationSvc = new QuizzNotificationService(_controller);
                return _quizzNotificationSvc;
            }
        }

        private QuizzCommentNotificationService _quizzCommentNotificationSvc;
        public QuizzCommentNotificationService QuizzCommentNotificationSvc
        {
            get
            {
                if (_quizzCommentNotificationSvc == null)
                    _quizzCommentNotificationSvc = new QuizzCommentNotificationService(_controller);
                return _quizzCommentNotificationSvc;
            }
        }

        private QuestionNotificationService _questionNotificationSvc;
        public QuestionNotificationService QuestionNotificationSvc
        {
            get
            {
                if (_questionNotificationSvc == null)
                    _questionNotificationSvc = new QuestionNotificationService(_controller);
                return _questionNotificationSvc;
            }
        }

        private AssignmentNotificationService _assignmentNotificationSvc;
        public AssignmentNotificationService AssignmentNotificationSvc
        {
            get
            {
                if (_assignmentNotificationSvc == null)
                    _assignmentNotificationSvc = new AssignmentNotificationService(_controller);
                return _assignmentNotificationSvc;
            }
        }

        private DepQuizzNotificationService _depQuizzNotificationSvc;
        public DepQuizzNotificationService DepQuizzNotificationSvc
        {
            get
            {
                if (_depQuizzNotificationSvc == null)
                    _depQuizzNotificationSvc = new DepQuizzNotificationService(_controller);
                return _depQuizzNotificationSvc;
            }
        }

        private DepQuizzCommentNotificationService _depQuizzCommentNotificationSvc;
        public DepQuizzCommentNotificationService DepQuizzCommentNotificationSvc
        {
            get
            {
                if (_depQuizzCommentNotificationSvc == null)
                    _depQuizzCommentNotificationSvc = new DepQuizzCommentNotificationService(_controller);
                return _depQuizzCommentNotificationSvc;
            }
        }

        private DepQuizzmateNotificationService _depQuizzmateNotificationSvc;
        public DepQuizzmateNotificationService DepQuizzmateNotificationSvc
        {
            get
            {
                if (_depQuizzmateNotificationSvc == null)
                    _depQuizzmateNotificationSvc = new DepQuizzmateNotificationService(_controller);
                return _depQuizzmateNotificationSvc;
            }
        }

        private QuizzmateNotificationService _quizzmateNotificationSvc;
        public QuizzmateNotificationService QuizzmateNotificationSvc
        {
            get
            {
                if (_quizzmateNotificationSvc == null)
                    _quizzmateNotificationSvc = new QuizzmateNotificationService(_controller);
                return _quizzmateNotificationSvc;
            }
        }

        private QuizzlingNotificationService _quizzlingNotificationSvc;
        public QuizzlingNotificationService QuizzlingNotificationSvc
        {
            get
            {
                if (_quizzlingNotificationSvc == null)
                    _quizzlingNotificationSvc = new QuizzlingNotificationService(_controller);
                return _quizzlingNotificationSvc;
            }
        } 
    }
}