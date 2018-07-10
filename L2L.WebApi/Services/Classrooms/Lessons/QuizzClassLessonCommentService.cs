using L2L.Entities;
using L2L.WebApi.Controllers;
using L2L.WebApi.Models;
using L2L.WebApi.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper.QueryableExtensions;
using L2L.WebApi.Helper;
using System.Data.Entity;

namespace L2L.WebApi.Services
{
    public class QuizzClassLessonCommentService : BaseService, IResource
    {
        public QuizzClassLessonCommentService(BaseApiController controller)
            : base(controller)
        {
        }

        public object GetManyAlt(int id, int id2, int id3, string str, string str2, string str3)
        {
            try
            {
                // 0 - get latest
                // 1 - get older with date
                // 2 - get newer with date
                var type = id;
                var quizzClassLessonId = id2;
                var numItemsToGet = id3;

                var quizzClassLesson = _uow.QuizzClassLessons.GetById(quizzClassLessonId);

                List<QuizzClassLessonCommentModel> list = null;

                switch (type)
                {
                    case 0:
                        list = _uow.QuizzClassLessonComments.GetAll()
                            .Where(qc => qc.QuizzClassLessonId == quizzClassLessonId && qc.IsDeleted == false)
                            .OrderByDescending(qc => qc.PostedDate)
                            .Take(numItemsToGet)
                            .ProjectTo<QuizzClassLessonCommentModel>(new { userId = _currentUser.Id })
                            .ToList();
                        list.Reverse();
                        break;
                    case 1:
                        var date = DateTimeUtil.GetTimeFromClientStr(str);
                        list = list = _uow.QuizzClassLessonComments.GetAll()
                            .Where(qc => qc.QuizzClassLessonId == quizzClassLessonId && qc.IsDeleted == false && qc.PostedDate < date)
                            .OrderByDescending(qc => qc.PostedDate)
                            .Take(numItemsToGet)
                            .ProjectTo<QuizzClassLessonCommentModel>(new { userId = _currentUser.Id })
                            .ToList();
                        break;
                    case 2:
                        break;
                    default:
                        break;
                }

                foreach (var item in list)
                {
                    if (item.IsAuthor)
                        item.IsQuizzmate = true;
                    item.AuthorName = item.IsQuizzmate ? item.AuthorFullName : item.AuthorUserName;
                    item.PostedDate = item.PostedDate.ToLocalTime();

                    SetAge(item);
                }

                var quizzClassLessonIdx = _uow.QuizzClassLessons.GetAll()
                    .Where(qcl => qcl.Id == quizzClassLessonId)
                    .Select(qcl => qcl.QuizzClassLesssonIdx)
                    .FirstOrDefault();

                var numCount = _svcContainer.QuizzClassMemberUpdateSvc.RemoveClassLessonDiscussion(quizzClassLesson.QuizzClassId, quizzClassLessonIdx, numItemsToGet, true);

                UpdateModelList(list, numCount);

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
                var quizzClassLessonId = id;
                var pageNum = id2;
                var numPerPage = id3;

                var quizzClass = _uow.QuizzClassLessons.GetAll()
                    .Where(qcl => qcl.Id == quizzClassLessonId)
                    .Select(qcl => qcl.QuizzClass)
                    .ProjectTo<QuizzClassModel>(new { userId = _currentUser.Id })
                    .FirstOrDefault();

                quizzClass.Member = _uow.QuizzClassMembers.GetAll()
                   .Where(qcm => qcm.Id == quizzClass.QuizzClassMemberId)
                   .ProjectTo<QuizzClassMemberModel>()
                   .FirstOrDefault();

                var list = _uow.QuizzClassLessonComments.GetAll()
                    .Where(qclm => qclm.QuizzClassLessonId == quizzClassLessonId && qclm.IsDeleted == false)
                    .OrderByDescending(qca => qca.PostedDate)
                    .Skip((pageNum - 1) * numPerPage)
                    .Take(numPerPage)
                    .ProjectTo<QuizzClassLessonCommentModel>(new { userId = _currentUser.Id })
                    .ToList();

                foreach (var item in list)
                {
                    if (item.IsAuthor)
                        item.IsQuizzmate = true;
                    item.AuthorName = item.IsQuizzmate ? item.AuthorFullName : item.AuthorUserName;
                    item.PostedDate = item.PostedDate.ToLocalTime();

                    SetAge(item);
                }

                var quizzClassLessonIdx = _uow.QuizzClassLessons.GetAll()
                    .Where(qcl => qcl.Id == quizzClassLessonId)
                    .Select(qcl => qcl.QuizzClassLesssonIdx)
                    .FirstOrDefault();


                var intArrayHelper = new IntArray(quizzClass.Member.NewLessonCommentCount);
                var count = intArrayHelper.GetAtIndex(quizzClassLessonIdx);
                if (count > 0)
                    _svcContainer.QuizzClassMemberUpdateSvc.RemoveClassLessonDiscussion(quizzClass.Id, quizzClassLessonIdx, count, true);
                foreach (var item in list)
                {
                    item.IsNew = count-- > 0;
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
                var model = JsonConvert.DeserializeObject<QuizzClassLessonCommentModel>(modelParam.ToString());
                if (model == null)
                    return null;

                QuizzClassLessonComment entity;
                model.MapToNew(out entity);

                entity.PostedDate = DateTime.UtcNow;
                entity.AuthorId = _currentUser.Id;

                _uow.QuizzClassLessonComments.Add(entity);
                _uow.SaveChanges();

                model = _uow.QuizzClassLessonComments.GetAll()
                    .Where(qclc => qclc.Id == entity.Id)
                    .ProjectTo<QuizzClassLessonCommentModel>()
                    .FirstOrDefault();

                _svcContainer.QuizzClassMemberUpdateSvc.AddClassLessonDiscussion(model.QuizzClassId, model.QuizzClassLessonIdx, true);

                model.IsAuthor = true;
                UpdateModel(model);

                return model;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        public bool Patch(object modelParam)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<QuizzClassLessonCommentModel>(modelParam.ToString());
                if (model == null)
                    return false;

                model.PostedDate = model.PostedDate.ToUniversalTime();

                QuizzClassLessonComment entity;
                model.MapToNew(out entity);

                _uow.QuizzClassLessonComments.Update(entity);
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
            try
            {
                var entity = _uow.QuizzClassLessonComments.GetAll()
                    .Where(qclc => qclc.Id == id)
                    .Include(qclc => qclc.QuizzClassLesson)
                    .FirstOrDefault();
                var quizzClassLesson = entity.QuizzClassLesson;

                entity.IsDeleted = true;
                _uow.QuizzClassLessonComments.Update(entity);

                _svcContainer.QuizzClassMemberUpdateSvc.SubClassLessonDiscussion(quizzClassLesson.QuizzClassId, quizzClassLesson.QuizzClassLesssonIdx, false);
                _uow.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        public void UpdateModelList(IEnumerable<QuizzClassLessonCommentModel> list, int numNewCount)
        {
            foreach (var item in list)
            {
                UpdateModel(item);
                item.IsNew = numNewCount-- > 0;
            }
        }

        public void UpdateModel(QuizzClassLessonCommentModel model)
        {
            SetAge(model);

            if (model.IsAuthor)
            {
                model.IsAuthor = true;
                model.IsQuizzmate = true;
            }
            model.AuthorName = model.IsQuizzmate ? model.AuthorFullName : model.AuthorUserName;
            model.PostedDate = model.PostedDate.ToLocalTime();
        }

    }
}