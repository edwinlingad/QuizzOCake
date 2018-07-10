using L2L.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper.QueryableExtensions;
using L2L.WebApi.Models;
using System.Data.Entity;

namespace L2L.WebApi.Services
{
    public class QuizzmatesService : BaseService, IResource
    {
        public QuizzmatesService(BaseApiController controller)
            : base(controller)
        {
        }

        // Quizzmate list
        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            try
            {
                int quizzerId = id;
                if (quizzerId == 0)
                    quizzerId = _currentUser.Id;

                var list1 = _uow.FriendRelationships.GetAll()
                    .Where(u => u.User1Id == quizzerId)
                    .Select(u => u.User2)
                    .ProjectTo<QuizzerModel>(new { userId = _currentUser.Id })
                    .ToList();

                var list2 = _uow.FriendRelationships.GetAll()
                    .Where(u => u.User2Id == quizzerId)
                    .Select(u => u.User1)
                    .ProjectTo<QuizzerModel>(new { userId = _currentUser.Id })
                    .ToList();

                var list = list1.Concat(list2).OrderBy(q => q.Profile.FirstName)
                    .ToList();

                UpdateModelList(list, quizzerId);

                return list;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        // Get Quizzmate count
        public object Get(int id)
        {
            try
            {
                var count1 = _uow.FriendRelationships.GetAll()
                    .Where(fr => fr.User1Id == _currentUser.Id)
                    .Count();
                var count2 = _uow.FriendRelationships.GetAll()
                    .Where(fr => fr.User2Id == _currentUser.Id)
                    .Count();

                return new { count = count1 + count2 };
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

        // remove quizzmate
        public bool Delete(int id)
        {
            try
            {
                var entity = _uow.FriendRelationships.GetAll()
                    .Where(fr => fr.User1Id == _currentUser.Id && fr.User2Id == id)
                    .FirstOrDefault();

                if (entity == null)
                {
                    entity = _uow.FriendRelationships.GetAll()
                    .Where(fr => fr.User2Id == _currentUser.Id && fr.User1Id == id)
                    .FirstOrDefault();
                }

                _uow.FriendRelationships.Delete(entity.Id);

                _svcContainer.QuizzConnectMsgThreadSvc.RemoveQuizzmateMsgThread(id, false);

                _svcContainer.NotificationSvc.DepQuizzmateNotificationSvc.AddDepUnQuizzmateNotification(id, false);
                _uow.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        public object GetManyAlt(int id, int id2, int id3, string str, string str2, string str3)
        {
            throw new NotImplementedException();
        }

        public object GetAlt(string str)
        {
            throw new NotImplementedException();
        }

        public void UpdateModelList(IEnumerable<QuizzerModel> list, int quizzerId)
        {
            foreach (var item in list)
            {
                UpdateModel(item, quizzerId);
            }
        }

        public void UpdateModel(QuizzerModel model, int quizzerId)
        {
            SetAge(model);

            if (model.Id == _currentUser.Id)
                model.IsQuizzmate = true;

            if (_svcContainer.UserSvc.IsDependent(model.Id))
                model.IsQuizzmate = true;

            if (model.IsQuizzmate == false)
            {
                model.UserFullName = "";
            }
        }
    }
}