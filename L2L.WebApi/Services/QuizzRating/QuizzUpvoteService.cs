using L2L.Entities;
using L2L.Entities.Enums;
using L2L.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Services
{
    public class QuizzUpvoteService : BaseService
    {
        public QuizzUpvoteService(BaseApiController controller)
            : base(controller)
        {
        }

        public bool UpVote(int quizzRatingId)
        {
            try
            {
                ModifyQuizzUpvote(quizzRatingId, 1);
            }
            catch(Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
            return true;
            
        }

        public bool DownVote(int quizzRatingId)
        {
            try
            {
                ModifyQuizzUpvote(quizzRatingId, 0);
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
            return true;
        }

        private void ModifyQuizzUpvote(int quizzRatingId, int value)
        {
            var entity = GetEntity(quizzRatingId);
            if(entity == null)
            {
                entity = new QuizzUpvote()
                {
                    UpVote = value,
                    UserId = _currentUser.Id,
                    QuizRatingId = quizzRatingId
                };
                _uow.QuizzUpvotes.Add(entity);
                _notificationSvc.QuizzNotificationSvc.AddQuizzLikeNotification(quizzRatingId, false);
                _activitySvc.QuizzActivitySvc.AddQuizzLikeActivity(quizzRatingId, false);
            }
            else
            {
                entity.UpVote = value;
                _uow.QuizzUpvotes.Update(entity);
            }
            _uow.SaveChanges();
        }

        public bool IsUpvoted(int quizzRatingId)
        {
            var isUpvoted = false;
            var entity = GetEntity(quizzRatingId);

            if (entity != null)
                isUpvoted = entity.UpVote != 0;

            return isUpvoted;
        }

        private QuizzUpvote GetEntity(int quzzRatingId)
        {
            var entity = _uow.QuizzUpvotes.GetAll()
                .Where(u => u.UserId == _currentUser.Id && u.QuizRatingId == quzzRatingId)
                .FirstOrDefault();

            return entity;
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