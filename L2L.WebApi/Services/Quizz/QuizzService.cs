using L2L.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.Models;
using L2L.Entities;
using L2L.Entities.Enums;
using L2L.WebApi.Enums;
using L2L.Data;
using System.Data.Entity;
using L2L.WebApi.Utilities;
using System.Net;

namespace L2L.WebApi.Services
{
    public class QuizzService : BaseService
    {
        public QuizzService(BaseApiController controller)
            : base(controller)
        {
        }

        public IList<QuizzModel> GetQuizzesByCurrentUser()
        {
            var list = GetQuizzesByUserId(_currentUser.Id);
            return list;
        }

        public IList<QuizzModel> GetQuizzesByUserId(int id)
        {
            var list = _uow.Quizzes.GetAll()
                .Where(q => q.OwnerId == id && q.IsDeleted == false)
                .OrderByDescending(q => q.Modified)
                .ToList();
            List<QuizzModel> modelList = null;

            if (list != null && list.Count != 0)
            {
                modelList = new List<QuizzModel>();
                foreach (var item in list)
                {
                    var model = MappingUtil.Map<Quizz, QuizzModel>(item);
                    modelList.Add(model);
                }
            }

            return modelList;
        }

        public QuizzModel GetQuizzById(int id)
        {
            try
            {
                QuizzModel model = null;
                var entity = _uow.Quizzes.GetAll()
                    .Include(q => q.Tests)
                    .Include(q => q.Reviewers)
                    .Where(q => q.Id == id && q.IsDeleted == false)
                    .FirstOrDefault();

                if (entity != null)
                    model = MappingUtil.Map<Quizz, QuizzModel>(entity);
                model.ReviewerId = entity.Reviewers.FirstOrDefault().Id;
                model.TestId = entity.Reviewers.FirstOrDefault().Id;

                return model;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        public bool CreateQuizz(QuizzModel model)
        {
            var entity = MappingUtil.Map<QuizzModel, Quizz>(model);
            entity.OwnerId = _currentUser.Id;
            entity.QuizRating = new QuizzRating();
            entity.Tests.Add(new Test
            {
                DefaultSetting = new TestSetting()
            });
            entity.Reviewers.Add(new Reviewer());

            _uow.Quizzes.Add(entity);
            _uow.SaveChanges();

            MappingUtil.Map<Quizz, QuizzModel>(entity, model);

            _notificationSvc.DepQuizzNotificationSvc.AddDepQuizzSubmitNotification(model.Id);
            _activitySvc.QuizzActivitySvc.AddQuizzCreateActivity(model.Id);
            return true;
        }

        public bool UpdateQuizz(QuizzModel model)
        {
            
            try
            {
                var entity = MappingUtil.Map<QuizzModel, Quizz>(model);

                _uow.Quizzes.Update(entity);
                _uow.SaveChanges();
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }

            return true;
        }

        public bool DeleteQuizz(int id)
        {
            try
            {
                var entity = _uow.Quizzes.GetById(id);
                entity.IsDeleted = true;

                _uow.Quizzes.Update(entity);
                _uow.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool SetLive(ToggleModel model)
        {
            try
            {
                if (IsAuthor(model.Id) == false) // Cannot modify
                    return false;

                var entity = _uow.Quizzes.GetById(model.Id);
                if (entity != null)
                {
                    entity.IsLive = model.Value;
                    _uow.Quizzes.Update(entity);

                    if (entity.IsLive == true)
                    {
                        _notificationSvc.DepQuizzNotificationSvc.AddDepQuizzLiveNotification(model.Id, false);
                        _activitySvc.QuizzActivitySvc.AddQuizzLiveActivity(model.Id, false);
                    }

                    _uow.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        public bool IsAuthor(int quizzId)
        {
            var quizz = _uow.Quizzes.GetById(quizzId);

            return quizz.OwnerId == _currentUser.Id;
        }

        public bool IsAuthor(int quizzId, int userId)
        {
            var quizz = _uow.Quizzes.GetById(quizzId);

            return quizz.OwnerId == userId;
        }

        private NotificationService __notificationSvc;
        private NotificationService _notificationSvc
        {
            get
            {
                if (__notificationSvc == null)
                    __notificationSvc = _svcContainer.NotificationSvc;
                return __notificationSvc;
            }
        }

        private ActivityService __activitySvc;
        private ActivityService _activitySvc
        {
            get
            {
                if (__activitySvc == null)
                    __activitySvc = _svcContainer.ActivitySvc;
                return __activitySvc;
            }
        }
    }
}