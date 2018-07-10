using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.Entities;
using L2L.Entities.Enums;
using L2L.WebApi.Controllers;
using L2L.WebApi.Models;
using L2L.WebApi.Utilities;
using System.Data.Entity;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;

namespace L2L.WebApi.Services
{
    public class QuizzCommentService : BaseService, IResource
    {
        const int _numCommentsPerGet = 2;
        public QuizzCommentService(BaseApiController controller)
            : base(controller)
        {
        }

        public IList<QuizzCommentModel> GetQuizzComments(int quizzId, int pageNum, int numPerPage, int skip, QuizzCommentSortTypeEnum sortType)
        {
            try
            {
                var queryable = _uow.QuizzComments.GetAll()
                    .Where(q => q.QuizzId == quizzId && q.IsDeleted == false)
                    .ProjectTo<QuizzCommentModel>(new { userId = _currentUser.Id });

                if (sortType == QuizzCommentSortTypeEnum.Popular)
                    queryable = queryable.OrderByDescending(qc => qc.NumLikes)
                        .ThenByDescending(qc => qc.PostedDate);
                else
                    queryable = queryable.OrderByDescending(qc => qc.PostedDate)
                        .ThenByDescending(qc => qc.NumLikes);

                var list = queryable.Skip(skip + (pageNum - 1) * numPerPage)
                                .Take(numPerPage)
                                .ToList();

                foreach (var item in list)
                    item.PostedDate = item.PostedDate.ToLocalTime();

                return list;
            }
            catch (Exception ex)
            {
                _loggingSvc.Log(ex);
                return null;
            }
        }

        public bool CreateComment(QuizzCommentModel model)
        {
            try
            {
                model.AuthorId = _currentUser.Id;
                model.PostedDate = DateTime.UtcNow;

                var entity = MappingUtil.Map<QuizzCommentModel, QuizzComment>(model);
                _uow.QuizzComments.Add(entity);
                _notificationSvc.QuizzNotificationSvc.AddQuizzCommentNotification(model.QuizzId, false);
                _uow.SaveChanges();

                model.Id = entity.Id;
                model.AuthorName = _currentUser.UserName;
                model.PostedDate = entity.PostedDate;

                _notificationSvc.DepQuizzCommentNotificationSvc.AddDepPostCommentNotification(model.Id, false);
                _notificationSvc.DepQuizzNotificationSvc.AddDepQuizzReceiveCommentNotification(model.QuizzId, false);
                _activitySvc.QuizzCommentActivitySvc.AddQuizzCommentCreateActivity(model.QuizzId, model.Id, false);
                _activitySvc.QuizzCommentActivitySvc.AddQuizzReceiveCommentActivity(model.QuizzId, model.Id, false);

                _uow.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _loggingSvc.Log(ex);
                return false;
            }
        }

        public bool UpdateComment(QuizzCommentModel model)
        {
            try
            {
                var entity = _uow.QuizzComments.GetById(model.Id);
                entity.Comment = model.Comment;
                _uow.QuizzComments.Update(entity);

                _notificationSvc.DepQuizzCommentNotificationSvc.AddDepPostedCommentModifiedNotification(model.Id, false);
                _activitySvc.QuizzCommentActivitySvc.AddQuizzCommentModifyActivity(model.QuizzId, model.Id, false);

                _uow.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _loggingSvc.Log(ex);
                return false;
            }
        }

        public bool DeleteComment(int id)
        {
            try
            {
                var entity = _uow.QuizzComments.GetAll()
                    .Where(c => c.Id == id)
                    //.Include(c => c.Likes)
                    //.Include(c => c.Flags)
                    .FirstOrDefault();

                entity.IsDeleted = true;
                _uow.QuizzComments.Update(entity);

                //for (var i = entity.Likes.Count - 1; i >= 0; i--)
                //    _uow.QuizzCommentLikes.Delete(entity.Likes[i].Id);

                //for (var i = entity.Flags.Count - 1; i >= 0; i--)
                //    _uow.QuizzCommentFlags.Delete(entity.Flags[i].Id);

                //_svcContainer.DeleteNotificationSvc.DeleteQuizzCommentNotifications(id);

                //_uow.QuizzComments.Delete(id);
                _uow.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _loggingSvc.Log(ex);
                return false;
            }
        }

        public bool UpVote(int quizzCommentId)
        {
            try
            {
                ModifyQuizzCommentUpVote(quizzCommentId, 1);
                return true;
            }
            catch (Exception ex)
            {
                _loggingSvc.Log(ex);
                return false;
            }
        }

        public bool DownVote(int quizzCommentId)
        {
            try
            {
                ModifyQuizzCommentUpVote(quizzCommentId, 0);
                return true;
            }
            catch (Exception ex)
            {
                _loggingSvc.Log(ex);
                return false;
            }
        }

        private void ModifyQuizzCommentUpVote(int quizzCommentId, int value)
        {
            var entity = _uow.QuizzCommentLikes.GetAll()
                .Where(q => q.QuizzCommentId == quizzCommentId && q.AuthorId == _currentUser.Id)
                .FirstOrDefault();

            if (entity == null)
            {
                entity = new QuizzCommentLike()
                {
                    AuthorId = _currentUser.Id,
                    QuizzCommentId = quizzCommentId,
                    UpVote = value
                };

                _uow.QuizzCommentLikes.Add(entity);
                _notificationSvc.QuizzCommentNotificationSvc.AddQuizzCommentLikeNotification(quizzCommentId, false);

                var quizzComment = _uow.QuizzComments.GetById(quizzCommentId);
                if (quizzComment != null)
                    _activitySvc.QuizzCommentActivitySvc.AddQuizzCommentLikeActivity(quizzComment.QuizzId, quizzCommentId, false);

                _uow.SaveChanges();
            }
            else
            {
                entity.UpVote = value;
                _uow.QuizzCommentLikes.Update(entity);
                _uow.SaveChanges();
            }
        }

        public bool FlagComment(int quizzCommentId)
        {
            try
            {
                ModifyQuizzCommentFlag(quizzCommentId, 1);
                _svcContainer.NotificationSvc.DepQuizzCommentNotificationSvc.AddDepPostedCommentFlaggedNotification(quizzCommentId);
                return true;
            }
            catch (Exception ex)
            {
                _loggingSvc.Log(ex);
                return false;
            }
        }

        public bool UnFlagComment(int quizzCommentId)
        {
            try
            {
                ModifyQuizzCommentFlag(quizzCommentId, 0);
                return true;
            }
            catch (Exception ex)
            {
                _loggingSvc.Log(ex);
                return false;
            }
        }

        private void ModifyQuizzCommentFlag(int quizzCommentId, int value)
        {
            var entity = _uow.QuizzCommentFlags.GetAll()
                .Where(q => q.QuizzCommentId == quizzCommentId && q.AuthorId == _currentUser.Id)
                .FirstOrDefault();

            if (entity == null)
            {
                entity = new QuizzCommentFlag()
                {
                    UpVote = value,
                    AuthorId = _currentUser.Id,
                    QuizzCommentId = quizzCommentId
                };

                _uow.QuizzCommentFlags.Add(entity);
                _notificationSvc.QuizzCommentNotificationSvc.AddQuizzCommentFlagNotification(quizzCommentId, false);

                var quizzComment = _uow.QuizzComments.GetById(quizzCommentId);
                if (quizzComment != null)
                {
                    _activitySvc.QuizzCommentActivitySvc.AddQuizzCommentFlagActivity(quizzComment.QuizzId, quizzCommentId, false);
                    _activitySvc.QuizzCommentActivitySvc.AddQuizzCommentFlaggedActivity(quizzComment.QuizzId, quizzCommentId, false);
                }

                _uow.SaveChanges();
            }
            else
            {
                entity.UpVote = value;
                _uow.QuizzCommentFlags.Update(entity);
                _uow.SaveChanges();
            }
        }

        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            throw new NotImplementedException();
        }

        public object GetManyAlt(int id, int id2, int id3, string str, string str2, string str3)
        {
            try
            {
                // 0 - get latest
                // 1 - get older with date
                // 2 - get newer with date
                var type = id;
                var quizzId = id2;
                var numItemsToGet = id3;

                List<QuizzCommentModel> list = null;

                switch (type)
                {
                    case 0:
                        list = _uow.QuizzComments.GetAll()
                            .Where(qc => qc.QuizzId == quizzId && qc.IsDeleted == false)
                            .OrderByDescending(qc => qc.PostedDate)
                            .Take(numItemsToGet)
                            .ProjectTo<QuizzCommentModel>(new { userId = _currentUser.Id })
                            .ToList();
                        list.Reverse();
                        break;
                    case 1:
                        var date = DateTimeUtil.GetTimeFromClientStr(str);
                        list = list = _uow.QuizzComments.GetAll()
                            .Where(qc => qc.QuizzId == quizzId && qc.IsDeleted == false && qc.PostedDate < date)
                            .OrderByDescending(qc => qc.PostedDate)
                            .Take(numItemsToGet)
                            .ProjectTo<QuizzCommentModel>(new { userId = _currentUser.Id })
                            .ToList();
                        break;
                    case 2:
                        break;
                    default:
                        break;
                }

                UpdateModelList(list);

                return list;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        public object GetAlt(string str)
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
                var model = JsonConvert.DeserializeObject<QuizzCommentModel>(modelParam.ToString());
                if (model == null)
                    return null;

                model.AuthorId = _currentUser.Id;
                model.PostedDate = DateTime.UtcNow;

                var entity = MappingUtil.Map<QuizzCommentModel, QuizzComment>(model);
                _uow.QuizzComments.Add(entity);
                _notificationSvc.QuizzNotificationSvc.AddQuizzCommentNotification(model.QuizzId, false);
                _uow.SaveChanges();

                model.Id = entity.Id;
                model.AuthorName = _currentUser.UserName;
                model.PostedDate = entity.PostedDate;

                _notificationSvc.DepQuizzCommentNotificationSvc.AddDepPostCommentNotification(model.Id, false);
                _notificationSvc.DepQuizzNotificationSvc.AddDepQuizzReceiveCommentNotification(model.QuizzId, false);
                _activitySvc.QuizzCommentActivitySvc.AddQuizzCommentCreateActivity(model.QuizzId, model.Id, false);
                _activitySvc.QuizzCommentActivitySvc.AddQuizzReceiveCommentActivity(model.QuizzId, model.Id, false);

                _uow.SaveChanges();

                model = _uow.QuizzComments.GetAll()
                    .Where(qc => qc.Id == entity.Id)
                    .ProjectTo<QuizzCommentModel>(new { userId = _currentUser.Id })
                    .FirstOrDefault();

                UpdateModel(model);

                return model;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                throw;
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


        // Update Model List
        public void UpdateModelList(IEnumerable<QuizzCommentModel> list)
        {
            foreach (var item in list)
            {
                UpdateModel(item);
            }
        }

        public void UpdateModel(QuizzCommentModel model)
        {
            SetAge(model);

            if (model.AuthorId == _currentUser.Id)
            {
                model.IsQuizzmate = true;
                model.IsAuthor = true;
                model.CanEdit = true;
            }
            else
            {
                var childIds = _currentUser.AsUserDependents.Select(ud => ud.ChildId)
                    .ToList();
                model.CanEdit = childIds.Contains(model.AuthorId);
            }

            model.AuthorName = model.IsQuizzmate ? model.AuthorFullName : model.AuthorUserName;
            model.AuthorUserName = "";
            model.AuthorFullName = "";

            model.PostedDate = model.PostedDate.ToLocalTime();    
        }

        private LoggingService __loggingSvc;
        private LoggingService _loggingSvc
        {
            get
            {
                if (__loggingSvc == null)
                    __loggingSvc = _svcContainer.LoggingSvc;
                return __loggingSvc;
            }
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