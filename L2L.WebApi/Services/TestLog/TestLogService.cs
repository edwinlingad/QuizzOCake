using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.Controllers;
using System.Data.Entity;
using L2L.WebApi.Models;
using L2L.Entities;
using L2L.WebApi.Utilities;
using AutoMapper.QueryableExtensions;

namespace L2L.WebApi.Services
{
    public class TestLogService : BaseService, IResource
    {
        public TestLogService(BaseApiController controller)
            : base(controller)
        {
        }

        public TestLogModel GetTestLogById(int id)
        {
            try
            {
                var model = _uow.QuizLogs.GetAll()
                    .Include(t => t.TestSetting)
                    .Where(t => t.Id == id)
                    .ProjectTo<TestLogModel>(new { userId = _currentUser.Id })
                    .FirstOrDefault();

                UpdateModel(model);

                return model;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        public IList<TestLogModel> GetTestLogByCurrentUser(int pageNum = 0, int numPerPage = 5)
        {
            return GetTestLogsByUserId(_currentUser.Id, pageNum, numPerPage);
        }

        public IList<TestLogModel> GetTestLogsByUserId(int id, int pageNum = 0, int numPerPage = 5)
        {
            var userId = id == 0 ? _currentUser.Id : id;

            var list = _uow.QuizLogs.GetAll()
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.DateTaken)
                .Take(numPerPage)
                .Skip(numPerPage * pageNum)
                .ProjectTo<TestLogModel>(new { userId = _currentUser.Id })
                .ToList();

            UpdateModelList(list);

            return list;
        }

        public IList<TestLogGroup> GetTestLogsGroupedOfUserId(int id)
        {
            var userId = id == 0 ? _currentUser.Id : id;

            var list = _uow.QuizLogs.GetAll()
                .Where(t => t.UserId == userId)
                .GroupBy(t => t.QuizzId)
                .Select(g => new TestLogGroup
                {
                    QuizzSummary = new QuizzSummary
                    {
                        Id = g.Key,
                        Title = g.FirstOrDefault().Quizz.Title,
                        Description = g.FirstOrDefault().Quizz.Description,
                        ScoreSum = g.Sum(h => h.Score),
                        TotalSum = g.Sum(h => h.Total),
                        Category = g.FirstOrDefault().Quizz.Category
                    },
                    TestLogs = g.Select(ts => new TestLogSummary
                    {
                        Id = ts.Id,
                        DateTaken = ts.DateTaken,
                        Score = ts.Score,
                        Total = ts.Total
                    }).OrderByDescending(ts => ts.DateTaken)
                }).ToList();

            var newList = new List<TestLogGroup>();
            while (list.Count() != 0)
            {
                TestLogGroup latest = list.First();
                foreach (var item in list)
                {
                    if (item.TestLogs.First().DateTaken > latest.TestLogs.First().DateTaken)
                        latest = item;
                    foreach (var item2 in item.TestLogs)
                    {
                        item2.DateTaken = item2.DateTaken.ToLocalTime();
                    }
                }
                newList.Add(latest);
                list.Remove(latest);
            }

            return newList;
        }

        public bool CreateTestLog(TestLogModel model)
        {
            try
            {
                _notificationSvc.QuizzNotificationSvc.AddQuizzTakeNotification(model.QuizzId, false);

                var entity = MappingUtil.Map<TestLogModel, TestLog>(model);
                entity.DateTaken = DateTime.UtcNow;
                _uow.QuizLogs.Add(entity);
                _uow.SaveChanges();
                MappingUtil.Map(entity, model);

                _svcContainer.DailyRewardSvc.CheckAddDailyRewardPoints(model, false);
                _activitySvc.QuizzActivitySvc.AddQuizzTakeActivity(model.QuizzId, entity.Id, false);

                _uow.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        public bool DeleteTestLog(int id)
        {
            try
            {
                _uow.QuizLogs.Delete(id);
                _uow.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public void UpdateModelList(IEnumerable<TestLogModel> list)
        {
            foreach (var item in list)
            {
                UpdateModel(item);
            }
        }

        public void UpdateModel(TestLogModel model)
        {
            if (model.IsAuthor)
            {
                model.IsAuthor = true;
                model.IsQuizzmate = true;
            }
            model.AuthorName = model.IsQuizzmate ? model.AuthorFullName : model.AuthorUserName;
            model.DateTaken = model.DateTaken.ToLocalTime();

            if(model.AssignmentId != 0)
            {
                model.IsPassed = _uow.Assignments.GetAll()
                    .Where(a => a.Id == model.AssignmentId)
                    .Select(a => a.IsCompleted)
                    .FirstOrDefault();
            }
        }

        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            throw new NotImplementedException();
        }

        public object GetManyAlt(int id, int id2, int id3, string str, string str2, string str3)
        {
            throw new NotImplementedException();
        }

        public object GetAlt(string str)
        {
            throw new NotImplementedException();
        }

        public object Get(int id)
        {
            try
            {
                var count = _uow.QuizLogs.GetAll()
                    .Select(q => q.Total)
                    .Sum();

                return new { count = count };
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        public object Post(object modelParam)
        {
            throw new NotImplementedException();
        }

        public bool Patch(object modelParam)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
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