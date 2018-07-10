using AutoMapper.QueryableExtensions;
using L2L.Entities;
using L2L.WebApi.Controllers;
using L2L.WebApi.Helper;
using L2L.WebApi.Models;
using L2L.WebApi.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Services
{
    public class QuizzClassLessonMessageService : BaseService, IResource
    {
        public QuizzClassLessonMessageService(BaseApiController controller)
            : base(controller)
        {
        }

        public object GetManyAlt(int id, int id2, int id3, string str, string str2, string str3)
        {
            throw new NotImplementedException();
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

                var list = _uow.QuizzClassLessonMessages.GetAll()
                    .Where(qclm => qclm.QuizzClassLessonId == quizzClassLessonId && qclm.IsDeleted == false)
                    .OrderBy(qca => qca.PostedDate)
                    .Skip((pageNum - 1) * numPerPage)
                    .Take(numPerPage)
                    .ProjectTo<QuizzClassLessonMessageModel>(new { userId = _currentUser.Id })
                    .ToList();

                foreach (var item in list)
                    item.PostedDate = item.PostedDate.ToLocalTime();

                if (list.Count > 0)
                {
                    var quizzClassLessonIdx = list[0].QuizzClassLessonIdx;
                    _svcContainer.QuizzClassMemberUpdateSvc.RemoveClassLessonContent(quizzClass.Id, quizzClassLessonIdx, true);

                    var intArrayHelper = new IntArray(quizzClass.Member.NewLessonMessageCount);
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
                var model = JsonConvert.DeserializeObject<QuizzClassLessonMessageModel>(modelParam.ToString());
                if (model == null)
                    return null;

                QuizzClassLessonMessage entity;
                model.MapToNew(out entity);

                entity.PostedDate = DateTime.UtcNow;
                if (entity.AddContentType == AddContentTypeEnum.PictureOnly)
                {
                    string tmpString;
                    ImageUtil.SaveImage("QuizzClass_" + model.QuizzClassId.ToString(), model.NewImageFileName, "", model.ImageContent, out tmpString);
                    entity.ImageUrl = tmpString;
                }

                _uow.QuizzClassLessonMessages.Add(entity);
                _uow.SaveChanges();

                model = _uow.QuizzClassLessonMessages.GetAll()
                    .Where(qclc => qclc.Id == entity.Id)
                    .ProjectTo<QuizzClassLessonMessageModel>()
                    .FirstOrDefault();

                _svcContainer.QuizzClassMemberUpdateSvc.AddClassLessonContent(model.QuizzClassId, model.QuizzClassLessonIdx, true);

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
                var model = JsonConvert.DeserializeObject<QuizzClassLessonMessageModel>(modelParam.ToString());
                if (model == null)
                    return false;

                model.PostedDate = model.PostedDate.ToUniversalTime();

                QuizzClassLessonMessage entity;
                model.MapToNew(out entity);

                if (entity.AddContentType == AddContentTypeEnum.PictureOnly && model.IsImageChanged)
                {
                    string tmpString;
                    ImageUtil.SaveImage("QuizzClass_" + model.QuizzClassId.ToString(), model.NewImageFileName, model.ImageUrl, model.ImageContent, out tmpString);
                    entity.ImageUrl = tmpString;
                }

                _uow.QuizzClassLessonMessages.Update(entity);

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
                var entity = _uow.QuizzClassLessonMessages.GetAll()
                    .Where(qclm => qclm.Id == id)
                    .Include(qclm => qclm.QuizzClassLesson)
                    .FirstOrDefault();
                var quizzClassLesson = entity.QuizzClassLesson;

                entity.IsDeleted = true;
                _uow.QuizzClassLessonMessages.Update(entity);

                _svcContainer.QuizzClassMemberUpdateSvc.SubClassLessonContent(quizzClassLesson.QuizzClassId, quizzClassLesson.QuizzClassLesssonIdx, false);
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