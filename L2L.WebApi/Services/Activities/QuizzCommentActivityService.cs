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
    public class QuizzCommentActivityService : BaseService
    {
        public QuizzCommentActivityService(BaseApiController controller)
            : base(controller)
        {
        }

        public bool AddQuizzCommentCreateActivity(int quizzId, int quizzCommentId, bool callSaveChanges = true)
        {
            return AddQuizzCommentActivity(ActivityEnum.QuizzCommentCreate, quizzId, quizzCommentId, callSaveChanges);
        }

        public bool AddQuizzCommentModifyActivity(int quizzId, int quizzCommentId, bool callSaveChanges = true)
        {
            return AddQuizzCommentActivity(ActivityEnum.QuizzCommentModify, quizzId, quizzCommentId, callSaveChanges);
        }

        public bool AddQuizzCommentLikeActivity(int quizzId, int quizzCommentId, bool callSaveChanges = true)
        {
            return AddQuizzCommentActivity(ActivityEnum.QuizzCommentLike, quizzId, quizzCommentId, callSaveChanges);
        }

        public bool AddQuizzCommentFlagActivity(int quizzId, int quizzCommentId, bool callSaveChanges = true)
        {
            return AddQuizzCommentActivity(ActivityEnum.QuizzCommentFlag, quizzId, quizzCommentId, callSaveChanges);
        }

        public bool AddQuizzCommentActivity(ActivityEnum type, int quizzId, int quizzCommentId, bool callSaveChanges = true)
        {
            try
            {
                var activity = new Activity
                {
                    PostedDate = DateTime.UtcNow,
                    ActivityType = type,
                    OwnerId = _currentUser.Id,
                    QuizzId = quizzId,
                    QuizzCommentId = quizzCommentId,
                    TestLogId = 1,
                    QuestionId = 1,
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

        public bool AddQuizzReceiveCommentActivity(int quizzId, int quizzCommentId, bool callSaveChanges = true)
        {
            try
            {
                var quizz = _uow.Quizzes.GetAll()
                    .Where(q => q.Id == quizzId)
                    .FirstOrDefault();
                
                if (quizz == null)
                    return false;

                if (quizz.OwnerId == _currentUser.Id)
                    return false;

                var activity = new Activity
                {
                    PostedDate = DateTime.UtcNow,
                    ActivityType = ActivityEnum.QuizzRecivedComment,
                    OwnerId = quizz.OwnerId,
                    QuizzId = quizzId,
                    QuizzCommentId = quizzCommentId,
                    TestLogId = 1,
                    QuestionId = 1,
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

        public bool AddQuizzCommentFlaggedActivity(int quizzId, int quizzCommentId, bool callSaveChanges = true)
        {
            try
            {
                var quizzComment = _uow.QuizzComments.GetAll()
                    .Where(q => q.Id == quizzCommentId)
                    .FirstOrDefault();
                if (quizzComment == null)
                    return false;

                var activity = new Activity
                {
                    PostedDate = DateTime.UtcNow,
                    ActivityType = ActivityEnum.QuizzRecivedComment,
                    OwnerId = quizzComment.AuthorId,
                    QuizzId = quizzId,
                    QuizzCommentId = quizzCommentId,
                    TestLogId = 1,
                    QuestionId = 1,
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