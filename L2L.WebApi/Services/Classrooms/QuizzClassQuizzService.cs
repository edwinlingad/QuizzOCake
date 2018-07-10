using AutoMapper.QueryableExtensions;
using L2L.Entities;
using L2L.WebApi.Controllers;
using L2L.WebApi.Models;
using L2L.WebApi.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Services
{
    public class QuizzClassQuizzService : BaseService, IResource
    {
        public QuizzClassQuizzService(BaseApiController controller)
            : base(controller)
        {
        }

        public object GetManyAlt(int id, int id2, int id3, string str, string str2, string str3)
        {
            try
            {
                var quizzClassId = id;
                var search = str;
                if (string.IsNullOrEmpty(search))
                    return new List<SearchModel>();

                var searchStr = search.Split(' ');
                string search1 = searchStr[0];
                string search2 = "";
                string search3 = "";
                if (searchStr.Length == 3)
                {
                    search2 = searchStr[1];
                    search3 = searchStr[2];
                }
                else if (searchStr.Length == 2)
                {
                    search2 = searchStr[1];
                }

                var list = _svcContainer.SearchSvc.SearchQuizz(search1, search2, search3);

                var quizzClassQuizzIds = _uow.QuizzClassQuizzes.GetAll()
                    .Where(qcq => qcq.QuizzClassId == quizzClassId)
                    .Select(qcq => qcq.QuizzId)
                    .ToList();

                for (int i = list.Count - 1; i >= 0; i--)
                {
                    var item = list[i];
                    if (quizzClassQuizzIds.Contains(item.QuizzId))
                        list.RemoveAt(i);
                }

                return list;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            try
            {
                var qcId = id;

                var quizzClass = _uow.QuizzClasses.GetAll()
                    .Where(qc => qc.Id == qcId)
                    .ProjectTo<QuizzClassModel>(new { userId = _currentUser.Id })
                    .FirstOrDefault();

                quizzClass.Member = _uow.QuizzClassMembers.GetAll()
                    .Where(qcm => qcm.Id == quizzClass.QuizzClassMemberId)
                    .ProjectTo<QuizzClassMemberModel>()
                    .FirstOrDefault();

                var list = _uow.QuizzClassQuizzes.GetAll()
                    .Where(qcq => qcq.QuizzClassId == qcId)
                    .OrderBy(qcq => qcq.PostedDate)
                    .Select(qcq => qcq.Quizz)
                    .ProjectTo<QuizzOverviewModel>(new { userId = _currentUser.Id })
                    .ToList();

                foreach (var item in list)
                {
                    SetAge(item);

                    if (item.OwnerId == _currentUser.Id)
                        item.IsQuizzmate = true;

                    item.OwnerName = item.IsQuizzmate ? item.OwnerFullName : item.OwnerUserName;
                    item.OwnerFullName = "";
                    item.OwnerUserName = "";
                }

                if (list.Count > 0)
                {
                    var count = _svcContainer.QuizzClassMemberUpdateSvc.RemoveClassQuizz(quizzClass.Id);
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        var item = list[i];
                        item.IsNew = count-- > 0;
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        public object Get(int id)
        {
            throw new NotImplementedException();
        }

        public object GetAlt(string str)
        {
            throw new NotImplementedException();
        }

        public object Post(object modelParam)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<QuizzClassQuizzModel>(modelParam.ToString());
                if (model == null)
                    return null;

                QuizzClassQuizz entity;
                model.MapToNew(out entity);
                entity.PostedDate = DateTime.UtcNow;

                _uow.QuizzClassQuizzes.Add(entity);
                _svcContainer.QuizzClassMemberUpdateSvc.AddClassQuizz(model.QuizzClassId, false);

                _uow.SaveChanges();

                var newModel = _svcContainer.QuizzOverviewSvc.GetQuizzOverviewModel(model.QuizzId);

                return newModel;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        // this is the delete method
        public bool Patch(object modelParam)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<QuizzClassQuizzModel>(modelParam.ToString());
                if (model == null)
                    return false;

                var list = _uow.QuizzClassQuizzes.GetAll()
                    .Where(qcq => qcq.QuizzId == model.QuizzId && qcq.QuizzClassId == model.QuizzClassId)
                    .ToList();

                foreach (var item in list)
                {
                    _uow.QuizzClassQuizzes.Delete(item.Id);
                    _svcContainer.QuizzClassMemberUpdateSvc.SubClassQuizz(item.QuizzClassId, false);
                }

                _uow.SaveChanges();

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
    }
}