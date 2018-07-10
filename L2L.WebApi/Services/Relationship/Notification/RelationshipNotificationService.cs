using L2L.WebApi.Controllers;
using L2L.WebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper.QueryableExtensions;
using L2L.Entities;
using L2L.WebApi.Utilities;

namespace L2L.WebApi.Services
{
    public class RelationshipNotificationService : BaseService, IResource
    {
        public RelationshipNotificationService(BaseApiController controller)
            : base(controller)
        {
        }

        // Get Notifications
        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            try
            {
                int pageNum = id;
                int numPerpage = id2;
                int lookForPending = id3;

                List<RelationshipNotificationModel> list;

                if (lookForPending == 0)
                    list = GetRelationshipNotificationRequests(pageNum, numPerpage);
                else
                    list = GetPendingRequests(pageNum, numPerpage);

                return list;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        private List<RelationshipNotificationModel> GetRelationshipNotificationRequests(int pageNum, int numPerpage)
        {
            List<RelationshipNotificationModel> list = _uow.RelationshipNotifications.GetAll()
                                    .OrderByDescending(r => r.PostedDate)
                                    .Where(r => r.ToUserId == _currentUser.Id)
                                    .Skip((pageNum - 1) * numPerpage)
                                    .Take(numPerpage)
                                    .ProjectTo<RelationshipNotificationModel>()
                                    .ToList();
            bool hasChanged = false;
            foreach (var item in list)
            {
                if (item.IsNew)
                {
                    RelationshipNotification entity;
                    item.MapToNew(out entity);
                    entity.IsNew = false;
                    _uow.RelationshipNotifications.Update(entity);
                    hasChanged = true;
                }
            }

            if (hasChanged)
                _uow.SaveChanges();
            return list;
        }

        private List<RelationshipNotificationModel> GetPendingRequests(int pageNum, int numPerpage)
        {
            return _uow.RelationshipNotifications.GetAll()
                                    .OrderByDescending(r => r.PostedDate)
                                    .Where(r => r.FromUserId == _currentUser.Id)
                                    .Skip((pageNum - 1) * numPerpage)
                                    .Take(numPerpage)
                                    .ProjectTo<RelationshipNotificationModel>()
                                    .ToList();
        }

        // Get Count
        public object Get(int id)
        {
            try
            {
                int count = 0;
                if (id == 0)
                {
                    count = _uow.RelationshipNotifications.GetAll()
                        .Where(r => r.ToUserId == _currentUser.Id)
                        .Count();
                } else
                {
                    count = _uow.RelationshipNotifications.GetAll()
                        .Where(r => r.FromUserId == _currentUser.Id)
                        .Count();
                }

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

        // response
        public bool Patch(object modelParam)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<RelationshipNotificationModel>(modelParam.ToString());
                if (model == null)
                    return false;

                var responseObject = _relationshipNotificationFactory.GetResponseObject(model.RNType);
                if( responseObject.ProccessResponse(model))
                {
                    _uow.RelationshipNotifications.Delete(model.Id);
                    _uow.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        public bool Delete(int id)
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

        private RelationshipNotificationResponseFactory __relationshipNotificationFactory;
        public RelationshipNotificationResponseFactory _relationshipNotificationFactory
        {
            get
            {
                if (__relationshipNotificationFactory == null)
                    __relationshipNotificationFactory = new RelationshipNotificationResponseFactory(_controller);
                return __relationshipNotificationFactory;
            }
        } 

        private QuizzmateRNotificationService _quizzmateNotificationSvc;
        public QuizzmateRNotificationService QuizzmateNotificationSvc
        {
            get
            {
                if (_quizzmateNotificationSvc == null)
                    _quizzmateNotificationSvc = new QuizzmateRNotificationService(_controller);
                return _quizzmateNotificationSvc;
            }
        }

        private QuizzlingRNotificationService _quizzlingNotificationSvc;
        public QuizzlingRNotificationService QuizzlingNotificationSvc
        {
            get
            {
                if (_quizzlingNotificationSvc == null)
                    _quizzlingNotificationSvc = new QuizzlingRNotificationService(_controller);
                return _quizzlingNotificationSvc;
            }
        } 
    }
}