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
    public class QuizzClassJoinService : BaseService, IResource
    {
        public QuizzClassJoinService(BaseApiController controller)
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

                var list = _uow.QuizzClassJoinRequests.GetAll()
                    .Where(qcj => qcj.QuizzClassId == quizzClassId && qcj.IsDeleted == false)
                    .OrderBy(qcj => qcj.IsNew == true)
                    .ThenByDescending(qcj => qcj.PostedDate)
                    .ProjectTo<QuizzClassJoinRequestModel>(new { userId = _currentUser.Id })
                    .ToList();

                foreach (var item in list)
                {
                    if (item.UserId == _currentUser.Id)
                        item.IsQuizzmate = true;

                    SetAge(item);

                    item.UserName = item.IsQuizzmate ? item.UserFullName : item.UserUserName;
                    item.UserUserName = "";
                    item.UserFullName = "";
                }

                var entityList = _uow.QuizzClassJoinRequests.GetAll()
                    .Where(qcj => qcj.QuizzClassId == quizzClassId && qcj.IsDeleted == false && qcj.IsNew == true)
                    .ToList();

                if (entityList.Count > 0)
                {
                    foreach (var item in entityList)
                    {
                        item.IsNew = false;
                        _uow.QuizzClassJoinRequests.Update(item);
                    }
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
            throw new NotImplementedException();
        }

        public object GetAlt(string str)
        {
            throw new NotImplementedException();
        }

        // request - returns back QuizzClassModel
        public object Post(object modelParam)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<QuizzClassJoinRequestModel>(modelParam.ToString());
                if (model == null)
                    return null;

                QuizzClassJoinRequest entity;
                model.MapToNew(out entity);

                entity.PostedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                entity.IsAccepted = false;
                entity.UserId = _currentUser.Id;
                entity.IsNew = true;

                _uow.QuizzClassJoinRequests.Add(entity);
                _uow.SaveChanges();

                var newModel = _uow.QuizzClassJoinRequests.GetAll()
                    .Where(qcj => qcj.Id == entity.Id)
                    .Select(qcj => qcj.QuizzClass)
                    .ProjectTo<QuizzClassModel>(new { userId = _currentUser.Id })
                    .FirstOrDefault();

                newModel.TeacherName = newModel.IsTeacherQuizzmate ? newModel.TeacherFullName : newModel.TeacherUserName;

                SetAge(newModel);

                newModel.Member = _uow.QuizzClassMembers.GetAll()
                   .Where(qcm => qcm.Id == newModel.QuizzClassMemberId)
                   .ProjectTo<QuizzClassMemberModel>()
                   .FirstOrDefault();

                return newModel;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        // resend request
        public bool Patch(object modelParam)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<QuizzClassJoinRequestModel>(modelParam.ToString());
                if (model == null)
                    return false;


                var entity = _uow.QuizzClassJoinRequests.GetAll()
                    .Where(qcj => qcj.QuizzClassId == model.QuizzClassId && qcj.UserId == _currentUser.Id)
                    .FirstOrDefault();

                entity.PostedDate = DateTime.UtcNow;
                entity.IsNew = true;

                _uow.QuizzClassJoinRequests.Update(entity);
                _uow.SaveChanges();

                MappingUtil.Map(entity, model);

                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        // cancel request
        public bool Delete(int id)
        {
            try
            {
                var list = _uow.QuizzClassJoinRequests.GetAll()
                    .Where(qcj => qcj.QuizzClassId == id && qcj.UserId == _currentUser.Id && qcj.IsDeleted == false)
                    .ToList();

                foreach (var item in list)
                {
                    item.IsDeleted = true;
                    _uow.QuizzClassJoinRequests.Update(item);
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

        public bool HasJoinRequest(int quizzClassId)
        {
            try
            {
                var count = _uow.QuizzClassJoinRequests.GetAll()
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

        public void DeleteJoinRequest(int quizzClassId, bool callSaveChanges = true)
        {
            try
            {
                var list = _uow.QuizzClassJoinRequests.GetAll()
                    .Where(qcj => qcj.QuizzClassId == quizzClassId && qcj.UserId == _currentUser.Id && qcj.IsDeleted == false)
                    .ToList();
                foreach (var item in list)
                {
                    _uow.QuizzClassJoinRequests.Delete(item.Id);
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