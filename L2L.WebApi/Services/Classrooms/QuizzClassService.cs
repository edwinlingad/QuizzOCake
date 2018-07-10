using L2L.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper.QueryableExtensions;
using L2L.WebApi.Models;
using Newtonsoft.Json;
using L2L.Entities;
using L2L.WebApi.Utilities;
using System.Web.Http;
using L2L.WebApi.Helper;

namespace L2L.WebApi.Services
{
    [Authorize]
    public class QuizzClassService : BaseService, IResource
    {
        public QuizzClassService(BaseApiController controller)
            : base(controller)
        {
        }

        public object GetManyAlt(int id, int id2, int id3, string str, string str2, string str3)
        {
            try
            {
                var searchType = id; // 0 - no filter
                                     // 1 - remove if isTeacher, isMember, isRequestSent, isInviteSent
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

                var list = _svcContainer.SearchSvc.SearchQuizzClass(search1, search2, search3);

                switch (searchType)
                {
                    case 0: // no filter
                        break;
                    case 1: // 1 - remove if isTeacher, isMember, isRequestSent, isInviteSent
                        for (int i = list.Count - 1; i >= 0; i--)
                        {
                            var model = list[i];
                            if (model.IsTeacher || model.IsMember || model.IsRequestSent || model.IsInviteSent)
                                list.RemoveAt(i);
                        }
                        break;
                    default:
                        break;
                }

                SetAge(list);

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
                var type = id;       // 0 - owned
                                     // 1 - classes enrolled in
                                     // 2 - classes available from userId ( id2 )
                                     // 3 - pending requests of current user
                                     // 4 - quizzrooms dependent is enrolled in
                var userId = id2 == 0 ? _currentUser.Id : id2;
                var isTeacher = type == 0;
                List<QuizzClassModel> list = null;
                switch (type)
                {
                    case 0: // owned
                        list = _uow.QuizzClasses.GetAll()
                            .Where(qc => qc.TeacherId == _currentUser.Id && qc.IsDeleted == false)
                            .ProjectTo<QuizzClassModel>(new { userId = _currentUser.Id })
                            .ToList();
                        break;
                    case 1: // 1 - classes enrolled in
                        list = _uow.QuizzClassMembers.GetAll()
                            .Where(qcm => qcm.StudentId == _currentUser.Id &&
                                    qcm.IsParent == false &&
                                    qcm.QuizzClass.TeacherId != _currentUser.Id &&
                                    qcm.QuizzClass.IsDeleted == false)
                            .Select(qcm => qcm.QuizzClass)
                            .ProjectTo<QuizzClassModel>(new { userId = _currentUser.Id })
                            .ToList();
                        break;
                    case 2: // 2 - classes available from userId ( id2 )
                        list = _uow.QuizzClasses.GetAll()
                            .Where(qc => qc.TeacherId == userId && qc.IsDeleted == false)
                            .ProjectTo<QuizzClassModel>(new { userId = _currentUser.Id, depId = userId })
                            .ToList();
                        break;
                    case 3: // 3 - pending requests of current user
                        var tmpList = _uow.QuizzClassJoinRequests.GetAll()
                            .Where(qcj => qcj.UserId == _currentUser.Id && qcj.QuizzClass.IsDeleted == false && qcj.IsDeleted == false)
                            .ToList();

                        list = _uow.QuizzClassJoinRequests.GetAll()
                            .Where(qcj => qcj.UserId == _currentUser.Id && qcj.QuizzClass.IsDeleted == false && qcj.IsDeleted == false)
                            .Select(qcj => qcj.QuizzClass)
                            .ProjectTo<QuizzClassModel>(new { userId = _currentUser.Id })
                            .ToList();
                        break;
                    case 4: // 4 - quizzrooms dependent is enrolled in
                        list = _uow.QuizzClassMembers.GetAll()
                            .Where(qcm => qcm.StudentId == _currentUser.Id &&
                                    qcm.IsParent == true &&
                                    qcm.QuizzClass.TeacherId != userId &&
                                    qcm.DependentId == userId &&
                                    qcm.QuizzClass.IsDeleted == false)
                            .Select(qcm => qcm.QuizzClass)
                            .ProjectTo<QuizzClassModel>(new { userId = _currentUser.Id, depId = userId })
                            .ToList();
                        break;
                }

                UpdateModelList(list);

                if (type == 1 || type == 4)
                {
                    bool hasChange = false;
                    foreach (var model in list)
                    {
                        if (model.Member != null && model.Member.IsNew)
                        {
                            QuizzClassMember entity;
                            model.Member.MapToNew(out entity);
                            entity.IsNew = false;
                            _uow.QuizzClassMembers.Update(entity);

                            hasChange = true;
                        }
                    }

                    if (hasChange)
                        _uow.SaveChanges();
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
            try
            {
                var qcId = id;
                var model = _uow.QuizzClasses.GetAll()
                    .Where(qc => qc.Id == qcId)
                    .ProjectTo<QuizzClassModel>(new { userId = _currentUser.Id })
                    .FirstOrDefault();

                UpdateModel(model);

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
            try
            {
                var ids = str.Split(',')
                                .Select(id => Int32.Parse(id))
                                .ToList();
                var qcId = ids[0];
                var depId = ids[1];
                var model = _uow.QuizzClasses.GetAll()
                    .Where(qc => qc.Id == qcId)
                    .ProjectTo<QuizzClassModel>(new { userId = _currentUser.Id, depId = depId })
                    .FirstOrDefault();

                UpdateModel(model);

                return model;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        public object Post(object modelParam)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<QuizzClassModel>(modelParam.ToString());
                if (model == null)
                    return null;

                QuizzClass entity;
                model.MapToNew(out entity);

                entity.IsDeleted = false;
                entity.QuizzClassLessonIdx = 0;
                entity.QuizzChatGuid = Guid.NewGuid().ToString();
                entity.TeacherId = _currentUser.Id;

                entity.Students.Add(new QuizzClassMember()
                {
                    StudentId = _currentUser.Id
                });

                if (model.IsImageChanged)
                {
                    string tmpString;
                    ImageUtil.SaveImage("QuizzClass/QuizzClass_" + model.Id.ToString(), model.NewImageFileName, "", model.ImageContent, out tmpString);
                    entity.ImageUrl = tmpString;
                }

                _uow.QuizzClasses.Add(entity);
                _uow.SaveChanges();

                model = _uow.QuizzClasses.GetAll()
                    .Where(qc => qc.Id == entity.Id)
                    .ProjectTo<QuizzClassModel>()
                    .FirstOrDefault();

                SetAge(model);

                model.Member = _uow.QuizzClassMembers.GetAll()
                   .Where(qcm => qcm.Id == model.QuizzClassMemberId)
                   .ProjectTo<QuizzClassMemberModel>()
                   .FirstOrDefault();

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
                var model = JsonConvert.DeserializeObject<QuizzClassModel>(modelParam.ToString());
                if (model == null)
                    return false;

                QuizzClass entity;
                model.MapToNew(out entity);

                if (model.IsImageChanged)
                {
                    string tmpString;
                    ImageUtil.SaveImage("QuizzClass/QuizzClass_" + model.Id.ToString(), model.NewImageFileName, "", model.ImageContent, out tmpString);
                    entity.ImageUrl = tmpString;
                }

                _uow.QuizzClasses.Update(entity);
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
                var entity = _uow.QuizzClasses.GetById(id);
                entity.IsDeleted = true;
                _uow.QuizzClasses.Update(entity);
                _uow.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        public void UpdateModelList(IEnumerable<QuizzClassModel> list)
        {
            foreach (var model in list)
            {
                UpdateModel(model);
            }
        }

        public void UpdateModel(QuizzClassModel model)
        {
            model.IsTeacher = model.TeacherId == _currentUser.Id;
            model.TeacherName = model.IsTeacherQuizzmate ? model.TeacherFullName : model.TeacherUserName;
            model.TeacherUserName = "";
            model.TeacherFullName = "";

            SetAge(model);

            if (model.LatestAnnouncementPosteDate != null)
                model.LatestAnnouncementPosteDate = ((DateTime)model.LatestAnnouncementPosteDate).ToLocalTime();

            var qcAnnouncement = _svcContainer.QuizzClassAnnouncementSvc.GetLatestAnnouncement(model.Id);
            if (qcAnnouncement != null)
            {
                model.LatestAnnouncement = qcAnnouncement.Announcement;
                model.LatestAnnouncementPosteDate = qcAnnouncement.PostedDate.ToLocalTime();
            }

            model.Member = _uow.QuizzClassMembers.GetAll()
                .Where(qcm => qcm.Id == model.QuizzClassMemberId)
                .ProjectTo<QuizzClassMemberModel>()
                .FirstOrDefault();

            if (model.Member != null)
            {
                var intArrayHelper = new IntArray(model.Member.NewLessonCommentCount);
                var totalNewLessonComment = intArrayHelper.GetTotal();

                intArrayHelper = new IntArray(model.Member.NewLessonMessageCount);
                var totalNewLessonContent = intArrayHelper.GetTotal();

                intArrayHelper = new IntArray(model.Member.NewLessonQuizzCount);
                var totalNewQuizzContent = intArrayHelper.GetTotal();

                model.NumNewLessonItemCount = model.Member.NewClassLesson + totalNewLessonComment + totalNewLessonContent + totalNewQuizzContent;
            }
        }
    }
}