using AutoMapper.QueryableExtensions;
using L2L.Entities;
using L2L.WebApi.Controllers;
using L2L.WebApi.Helper;
using L2L.WebApi.Models;
using L2L.WebApi.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace L2L.WebApi.Services
{
    public class QuizzClassLessonQuizzService : BaseService, IResource
    {
        public QuizzClassLessonQuizzService(BaseApiController controller)
            : base(controller)
        {
        }

        public object GetManyAlt(int id, int id2, int id3, string str, string str2, string str3)
        {
            try
            {
                var quizzClassLessonId = id;
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

                var quizzClassQuizzQuizzIds = _uow.QuizzClassLessonQuizzes.GetAll()
                    .Where(qcq => qcq.QuizzClassLessonId == quizzClassLessonId)
                    .Select(qcq => qcq.QuizzId)
                    .ToList();

                for (int i = list.Count - 1; i >= 0; i--)
                {
                    var item = list[i];
                    if (quizzClassQuizzQuizzIds.Contains(item.QuizzId))
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
                var qcLessonId = id;

                var quizzClass = _uow.QuizzClassLessons.GetAll()
                    .Where(qcl => qcl.Id == qcLessonId)
                    .Select(qcl => qcl.QuizzClass)
                    .ProjectTo<QuizzClassModel>(new { userId = _currentUser.Id })
                    .FirstOrDefault();

                quizzClass.Member = _uow.QuizzClassMembers.GetAll()
                    .Where(qcm => qcm.Id == quizzClass.QuizzClassMemberId)
                    .ProjectTo<QuizzClassMemberModel>()
                    .FirstOrDefault();

                var list = _uow.QuizzClassLessonQuizzes.GetAll()
                    .Where(qcq => qcq.QuizzClassLessonId == qcLessonId)
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

                if(list.Count > 0)
                {
                    var quizzClassLessonIdx = _uow.QuizzClassLessons.GetAll()
                        .Where(qcl => qcl.Id == qcLessonId && qcl.IsDeleted == false)
                        .Select(qcl => qcl.QuizzClassLesssonIdx)
                        .FirstOrDefault();

                    _svcContainer.QuizzClassMemberUpdateSvc.RemoveClassLessonQuizz(quizzClass.Id, quizzClassLessonIdx);

                    var intArrayHelper = new IntArray(quizzClass.Member.NewLessonQuizzCount);
                    var count = intArrayHelper.GetAtIndex(quizzClassLessonIdx);
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
                var model = JsonConvert.DeserializeObject<QuizzClassLessonQuizzModel>(modelParam.ToString());
                if (model == null)
                    return null;

                var quizzClassLessonIdx = _uow.QuizzClassLessons.GetAll()
                    .Where(qcl => qcl.Id == model.QuizzClassLessonId && qcl.IsDeleted == false)
                    .Select(qcl => qcl.QuizzClassLesssonIdx)
                    .FirstOrDefault();

                QuizzClassLessonQuizz entity;
                model.MapToNew(out entity);
                entity.PostedDate = DateTime.UtcNow;

                _uow.QuizzClassLessonQuizzes.Add(entity);
                _svcContainer.QuizzClassMemberUpdateSvc.AddClassLessonQuizz(model.QuizzClassId, quizzClassLessonIdx, false);

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
                var model = JsonConvert.DeserializeObject<QuizzClassLessonQuizzModel>(modelParam.ToString());
                if (model == null)
                    return false;

                var list = _uow.QuizzClassLessonQuizzes.GetAll()
                    .Where(qcq => qcq.QuizzClassLessonId == model.QuizzClassLessonId && qcq.QuizzId == model.QuizzId)
                    .Include(qcq => qcq.QuizzClassLesson)
                    .ToList();

                foreach (var item in list)
                {
                    var quizzClassLesson = item.QuizzClassLesson;
                    _svcContainer.QuizzClassMemberUpdateSvc.SubClassLessonQuizz(quizzClassLesson.QuizzClassId, quizzClassLesson.QuizzClassLesssonIdx, false);
                    _uow.QuizzClassLessonQuizzes.Delete(item.Id);
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