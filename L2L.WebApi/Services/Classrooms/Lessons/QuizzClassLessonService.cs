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

namespace L2L.WebApi.Services
{
    public class QuizzClassLessonService : BaseService, IResource
    {
        public QuizzClassLessonService(BaseApiController controller)
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
                var quizzClassId = id;
                var pageNum = id2;
                var numPerPage = id3;
                int? depId = null;
                if (!(id4 == 0 || id4 == -1))
                    depId = id4;

                var quizzClass = _uow.QuizzClasses.GetAll()
                    .Where(qc => qc.Id == quizzClassId)
                    .ProjectTo<QuizzClassModel>(new { userId = _currentUser.Id, depId = depId })
                    .FirstOrDefault();

                quizzClass.Member = _uow.QuizzClassMembers.GetAll()
                   .Where(qcm => qcm.Id == quizzClass.QuizzClassMemberId)
                   .ProjectTo<QuizzClassMemberModel>()
                   .FirstOrDefault();

                var list = _uow.QuizzClassLessons.GetAll()
                    .Where(qcl => qcl.QuizzClassId == quizzClassId && qcl.IsDeleted == false)
                    .OrderBy(qca => qca.QuizzClassLesssonIdx)
                    .Skip((pageNum - 1) * numPerPage)
                    .Take(numPerPage)
                    .ProjectTo<QuizzClassLessonModel>(new { userId = _currentUser.Id })
                    .ToList();

                int count = 0;
                IntArray intArrayHelper = null;
                IntArray intArrayHelper2 = null;
                IntArray intArrayHelper3 = null;

                if (quizzClass.Member != null)
                {
                    count = pageNum == 1 ? quizzClass.Member.NewClassLesson : 0;
                    intArrayHelper = new IntArray(quizzClass.Member.NewLessonMessageCount);
                    intArrayHelper2 = new IntArray(quizzClass.Member.NewLessonCommentCount);
                    intArrayHelper3 = new IntArray(quizzClass.Member.NewLessonQuizzCount);
                }

                for (int i = list.Count -1 ; i >=0; i--)
                {
                    var item = list[i];
                    SetAge(item);
                    item.IsTeacher = item.TeacherId == _currentUser.Id;
                    item.TeacherName = item.IsTeacherQuizzmate ? item.TeacherFullName : item.TeacherUserName;
                    item.TeacherUserName = "";
                    item.TeacherFullName = "";
                    item.IsNew = count-- > 0;

                    if (quizzClass.Member != null)
                    {
                        item.NewMessageCount = intArrayHelper.GetAtIndex(item.QuizzClassLesssonIdx);
                        item.NewCommentCount = intArrayHelper2.GetAtIndex(item.QuizzClassLesssonIdx);
                        item.NewQuizzCount = intArrayHelper3.GetAtIndex(item.QuizzClassLesssonIdx);
                    }
                }

                _svcContainer.QuizzClassMemberUpdateSvc.RemoveClassLesson(quizzClassId, true);

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
            try
            {
                var model = _uow.QuizzClassLessons.GetAll()
                    .Where(qcl => qcl.Id == id && qcl.IsDeleted == false)
                    .ProjectTo<QuizzClassLessonModel>(new { userId = _currentUser.Id })
                    .FirstOrDefault();

                model.IsTeacher = model.TeacherId == _currentUser.Id;
                SetAge(model);
                if (model.IsTeacher)
                    model.IsTeacherQuizzmate = true;
                model.TeacherName = model.IsTeacherQuizzmate ? model.TeacherFullName : model.TeacherUserName;
                model.TeacherUserName = "";
                model.TeacherFullName = "";

                model.Member = _uow.QuizzClassMembers.GetAll()
                    .Where(qcm => qcm.QuizzClassId == model.QuizzClassId && qcm.StudentId == _currentUser.Id)
                    .ProjectTo<QuizzClassMemberModel>(new { userId = _currentUser.Id })
                    .FirstOrDefault();

                if(model.Member != null)
                {
                    var intArrayHelper = new IntArray(model.Member.NewLessonMessageCount);
                    model.NewMessageCount = intArrayHelper.GetAtIndex(model.QuizzClassLesssonIdx);

                    intArrayHelper = new IntArray(model.Member.NewLessonCommentCount);
                    model.NewCommentCount = intArrayHelper.GetAtIndex(model.QuizzClassLesssonIdx);

                    intArrayHelper = new IntArray(model.Member.NewLessonQuizzCount);
                    model.NewQuizzCount = intArrayHelper.GetAtIndex(model.QuizzClassLesssonIdx);
                }

                return model;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        public object GetAlt(string str)
        {
            throw new NotImplementedException();
        }

        public object Post(object modelParam)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<QuizzClassLessonModel>(modelParam.ToString());
                if (model == null)
                    return null;

                var parent = _uow.QuizzClasses.GetById(model.QuizzClassId);

                QuizzClassLesson entity;
                model.MapToNew(out entity);
                entity.QuizzClassLesssonIdx = parent.QuizzClassLessonIdx++;

                _uow.QuizzClasses.Update(parent);
                _uow.QuizzClassLessons.Add(entity);

                _svcContainer.QuizzClassMemberUpdateSvc.AddClassLesson(entity.QuizzClassId, false);
                _uow.SaveChanges();

                entity.Comments = new List<QuizzClassLessonComment>();
                MappingUtil.Map(entity, model);

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
                var model = JsonConvert.DeserializeObject<QuizzClassLessonModel>(modelParam.ToString());
                if (model == null)
                    return false;

                QuizzClassLesson entity;
                model.MapToNew(out entity);

                _uow.QuizzClassLessons.Update(entity);
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
                var entity = _uow.QuizzClassLessons.GetById(id);
                entity.IsDeleted = true;
                _uow.QuizzClassLessons.Update(entity);

                _svcContainer.QuizzClassMemberUpdateSvc.SubClassLesson(entity.QuizzClassId, false);
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