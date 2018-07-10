using AutoMapper.QueryableExtensions;
using L2L.Entities;
using L2L.WebApi.Controllers;
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
    public class QuizzClassInviteService : BaseService, IResource
    {
        private const int maxPerCategory = 10;

        public QuizzClassInviteService(BaseApiController controller)
            : base(controller)
        {
        }

        // get quizzers not yet enrolled and no request sent yet
        public object GetManyAlt(int id, int id2, int id3, string str, string str2, string str3)
        {
            try
            {
                var qcId = id;
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

                var userList = _svcContainer.SearchSvc.SearchUsers(search1, search2, search3);

                var membersIdList = _uow.QuizzClassMembers.GetAll()
                    .Where(qcm => qcm.QuizzClassId == qcId)
                    .Select(qcm => qcm.StudentId)
                    .ToList();

                var inviteRequestIdList = _uow.QuizzClassInviteRequests.GetAll()
                    .Where(qci => qci.QuizzClassId == qcId && qci.IsDeleted == false)
                    .Select(qci => qci.UserId)
                    .ToList();

                var joinRequestIdList = _uow.QuizzClassJoinRequests.GetAll()
                    .Where(qci => qci.QuizzClassId == qcId && qci.IsDeleted == false)
                    .Select(qci => qci.UserId)
                    .ToList();

                for (int i = userList.Count - 1; i >= 0; i--)
                {
                    if (membersIdList.Contains((userList[i].UserId)))
                    {
                        userList.RemoveAt(i);
                        continue;
                    }

                    if (inviteRequestIdList.Contains((userList[i].UserId)))
                    {
                        userList.RemoveAt(i);
                        continue;
                    }

                    if (joinRequestIdList.Contains((userList[i].UserId)))
                    {
                        userList.RemoveAt(i);
                        continue;
                    }

                    var item = userList[i];
                    item.UserDisplayName = item.IsQuizzmate ? item.UserFullName : item.UserName;

                    SetAge(item);
                }

                return userList;
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
                var type = id;  // 0 - get invites sent for the quizz
                                // 1 - get invites for current user
                var qcId = id2;
                List<QuizzClassInviteRequestModel> list = null;

                switch (type)
                {
                    case 0: // get invites sent for the quizz
                        list = _uow.QuizzClassInviteRequests.GetAll()
                                .Where(qci => qci.QuizzClassId == qcId && qci.IsDeleted == false && qci.QuizzClass.TeacherId == _currentUser.Id)
                                .ProjectTo<QuizzClassInviteRequestModel>(new { useId = _currentUser.Id })
                                .ToList();
                        break;
                    case 1:
                        list = _uow.QuizzClassInviteRequests.GetAll()
                                .Where(qci => qci.UserId == _currentUser.Id && qci.IsDeleted == false)
                                .OrderBy(qci => qci.IsNew == true)
                                .ThenByDescending(qci => qci.PostedDate)
                                .ProjectTo<QuizzClassInviteRequestModel>(new { useId = _currentUser.Id })
                                .ToList();
                        break;
                }

                foreach (var item in list)
                {
                    if (item.UserId == _currentUser.Id)
                        item.IsQuizzmate = true;

                    if (item.TeacherBirthdate != null)
                        item.TeacherAge = DateTimeUtil.GetAge(item.TeacherBirthdate);

                    if (item.UserBirthdate != null)
                        item.UserAge = DateTimeUtil.GetAge(item.UserBirthdate);

                    item.UserName = item.IsQuizzmate ? item.UserFullName : item.UserUserName;
                    item.UserUserName = "";
                    item.UserFullName = "";

                    item.TeacherName = item.IsQuizzmate ? item.TeacherFullName : item.TeacherUserName;
                    item.TeacherUserName = "";
                    item.TeacherFullName = "";
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

        // send invite
        public object Post(object modelParam)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<QuizzClassInviteRequestModel>(modelParam.ToString());
                if (model == null)
                    return null;

                QuizzClassInviteRequest entity;
                model.MapToNew(out entity);

                entity.PostedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                entity.IsAccepted = false;
                entity.IsNew = true;

                _uow.QuizzClassInviteRequests.Add(entity);
                _uow.SaveChanges();

                model = _uow.QuizzClassInviteRequests.GetAll()
                    .Where(qci => qci.Id == entity.Id)
                    .ProjectTo<QuizzClassInviteRequestModel>(new { userId = _currentUser.Id })
                    .FirstOrDefault();

                model.UserName = model.IsQuizzmate ? model.UserFullName : model.UserUserName;
                model.UserUserName = "";
                model.UserFullName = "";

                if (model.TeacherBirthdate != null)
                    model.TeacherAge = DateTimeUtil.GetAge(model.TeacherBirthdate);

                if (model.UserBirthdate != null)
                    model.UserAge = DateTimeUtil.GetAge(model.UserBirthdate);

                return model;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        // this will be the delete
        public bool Patch(object modelParam)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<QuizzClassInviteRequestModel>(modelParam.ToString());
                if (model == null)
                    return false;

                QuizzClassInviteRequest entity;
                model.MapToNew(out entity);

                entity.IsDeleted = true;
                _uow.QuizzClassInviteRequests.Update(entity);

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
                var list = _uow.QuizzClassInviteRequests.GetAll()
                    .Where(qci => qci.QuizzClassId == id && qci.UserId == id && qci.IsDeleted == false)
                    .ToList();

                foreach (var item in list)
                {
                    item.IsDeleted = true;
                    _uow.QuizzClassInviteRequests.Update(item);
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

        public bool HasInviteRequest(int quizzClassId)
        {
            try
            {
                var count = _uow.QuizzClassInviteRequests.GetAll()
                    .Where(qcj => qcj.QuizzClassId == quizzClassId && qcj.UserId == _currentUser.Id && qcj.IsDeleted == false)
                    .Count();

                return count != 0;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        public void DeleteInviteRequest(int quizzClassId, bool callSaveChanges = true)
        {
            try
            {
                var list = _uow.QuizzClassInviteRequests.GetAll()
                    .Where(qcj => qcj.QuizzClassId == quizzClassId && qcj.UserId == _currentUser.Id && qcj.IsDeleted == false)
                    .ToList();
                foreach (var item in list)
                {
                    _uow.QuizzClassInviteRequests.Delete(item.Id);
                }

                if (callSaveChanges)
                    _uow.SaveChanges();

            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
            }
        }
    }
}