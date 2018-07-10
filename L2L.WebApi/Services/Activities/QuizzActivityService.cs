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

namespace L2L.WebApi.Services
{
    public class QuizzActivityService : BaseService
    {
        public QuizzActivityService(BaseApiController controller)
            : base(controller)
        {
        }

        public bool AddQuizzLikeActivity(int quizzId, bool callSaveChanges = true)
        {
            return AddQuizzActivity(ActivityEnum.QuizzLike, quizzId, callSaveChanges);
        }

        public bool AddQuizzCreateActivity(int quizzId, bool callSaveChanges = true)
        {
            return AddQuizzActivity(ActivityEnum.QuizzCreate, quizzId, callSaveChanges);
        }

        public bool AddQuizzLiveActivity(int quizzId, bool callSaveChanges = true)
        {
            return AddQuizzActivity(ActivityEnum.QuizzLive, quizzId, callSaveChanges);
        }

        public bool AddQuizzTakeActivity(int quizzId, int testLogId, bool callSaveChanges = true)
        {
            try
            {
                var activity = new Activity
                {
                    PostedDate = DateTime.UtcNow,
                    ActivityType = ActivityEnum.QuizzTake,
                    OwnerId = _currentUser.Id,
                    QuizzId = quizzId,
                    QuizzCommentId = 1,
                    TestLogId = testLogId,
                    QuestionId = 1
                    
                };

                _uow.Activities.Add(activity);
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

        private bool AddQuizzActivity(ActivityEnum type, int quizzId, bool callSaveChanges = true)
        {
            try
            {
                var activity = new Activity
                {
                    PostedDate = DateTime.UtcNow,
                    ActivityType = type,
                    OwnerId = _currentUser.Id,
                    QuizzId = quizzId,
                    QuizzCommentId = 1,
                    TestLogId = 1,
                    QuestionId =1,
                };

                _uow.Activities.Add(activity);
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