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

namespace L2L.WebApi.Services
{
    public class QuizzCommentService : BaseService
    {
        public QuizzCommentService(BaseApiController controller)
            : base(controller)
        {
        }

        public IList<QuizzCommentModel> GetQuizzComments(int quizzId, int pageNum, int numPerPage)
        {
            try
            {
                var list = _uow.QuizzComments.GetAll()
                    .Where(q => q.QuizzId == quizzId)
                    .OrderByDescending(q => q.PostedDate)
                    .Skip((pageNum - 1) * numPerPage)
                    .Take(numPerPage)
                    .ProjectTo<QuizzCommentModel>(new { userId = _currentUser.Id })
                    .ToList();

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
                var entity = MappingUtil.Map<QuizzCommentModel, QuizzComment>(model);
                _uow.QuizzComments.Add(entity);
                _notificationSvc.AddQuizzCommentNotification(model.QuizzId, false);
                _uow.SaveChanges();

                model.Id = entity.Id;

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
                var entity = MappingUtil.Map<QuizzCommentModel, QuizzComment>(model);
                _uow.QuizzComments.Update(entity);
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
                    .Include(c => c.Likes)
                    .Include(c => c.Flags)
                    .FirstOrDefault();

                foreach (var item in entity.Likes)
                    _uow.QuizzCommentLikes.Delete(item.Id);

                foreach (var item in entity.Flags)
                    _uow.QuizzCommentFlags.Delete(item.Id);

                _uow.QuizzComments.Delete(id);
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
                _notificationSvc.AddQuizzCommentLikeNotification(quizzCommentId, false);
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
                _notificationSvc.AddQuizzCommentFlagNotification(quizzCommentId, false);
                _uow.SaveChanges();
            }
            else
            {
                entity.UpVote = value;
                _uow.QuizzCommentFlags.Update(entity);
                _uow.SaveChanges();
            }
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
    }
}