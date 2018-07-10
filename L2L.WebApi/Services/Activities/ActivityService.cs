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
    public class ActivityService : BaseService
    {
        public ActivityService(BaseApiController controller)
            : base(controller)
        {
        }

        public IEnumerable<ActivityModel> GetActivitiesOfCurrentUser(int pageNum, int numPerPage, int skip = 0)
        {
            return GetActivitiesOfUser(_currentUser.Id, pageNum, numPerPage, skip);
        }

        public IEnumerable<ActivityModel> GetActivitiesOfUser(int userId, int pageNum, int numPerPage, int skip = 0)
        {
            try
            {
                var dependentEntities = _uow.Dependents.GetAll()
                    .Where(d => d.ChildId == userId);

                bool isDependentOfCurrentUser = false;

                foreach (var item in dependentEntities)
                {
                    if (item.UserId == _currentUser.Id)
                    {
                        isDependentOfCurrentUser = true;
                        break;
                    }
                }

                var queryable = _uow.Activities.GetAll()
                    .Where(a => a.OwnerId == userId);

                if (isDependentOfCurrentUser == false)
                {
                    queryable = queryable.Where(a => a.ActivityType != ActivityEnum.QuizzCreate &&
                        a.ActivityType != ActivityEnum.QuizzCommentModify &&
                        a.ActivityType != ActivityEnum.QuizzCommentFlag &&
                        a.ActivityType != ActivityEnum.QuizzRecivedComment &&
                        a.ActivityType != ActivityEnum.QuizzCommentFlagged);
                }

                queryable = queryable.OrderByDescending(a => a.PostedDate)
                    .Skip(skip + ((pageNum - 1) * numPerPage))
                    .Take(numPerPage);

                var list = queryable.ProjectTo<ActivityModel>().ToList();

                foreach (var item in list)
                {
                    item.PostedDate = item.PostedDate.ToLocalTime();
                }

                return list;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        public IEnumerable<ActivityModel> GetActivitiesOfQuizzmates(int pageNum, int numPerPage, int skip = 0)
        {
            throw new NotImplementedException();
        }

        private QuizzActivityService _quizzActivitySvc;
        public QuizzActivityService QuizzActivitySvc
        {
            get
            {
                if (_quizzActivitySvc == null)
                    _quizzActivitySvc = new QuizzActivityService(_controller);
                return _quizzActivitySvc;
            }
        }

        private QuizzCommentActivityService _quizzCommentActivitySvc;
        public QuizzCommentActivityService QuizzCommentActivitySvc
        {
            get
            {
                if (_quizzCommentActivitySvc == null)
                    _quizzCommentActivitySvc = new QuizzCommentActivityService(_controller);
                return _quizzCommentActivitySvc;
            }
        }


    }
}