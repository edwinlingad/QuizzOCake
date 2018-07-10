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
    public class QuizzClassMemberInviteService : BaseService, IResource
    {
        public QuizzClassMemberInviteService(BaseApiController controller)
            : base(controller)
        {
        }

        public object GetManyAlt(int id, int id2, int id3, string str, string str2, string str3)
        {
            throw new NotImplementedException();
        }

        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            throw new NotImplementedException();
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
                var model = JsonConvert.DeserializeObject<QuizzClassInviteRequestModel>(modelParam.ToString());
                if (model == null)
                    return null;

                QuizzClassInviteRequest inviteEntity;
                model.MapToNew(out inviteEntity);
                inviteEntity.IsDeleted = true;
                _uow.QuizzClassInviteRequests.Update(inviteEntity);

                _svcContainer.QuizzClassJoinSvc.DeleteJoinRequest(model.QuizzClassId, false);

                if (model.IsAccepted)
                {
                    QuizzClassMember entity = new QuizzClassMember
                    {
                        IsNew = false,
                        IsNewInviteAccepted = true,
                        QuizzClassId = model.QuizzClassId,
                        DependentId = 0,
                        StudentId = model.UserId
                    };
                    _uow.QuizzClassMembers.Add(entity);

                    var quizzClass = _uow.QuizzClasses.GetById(model.QuizzClassId);
                    var parents = _svcContainer.DependentSvc.GetParentsOfUserId(model.UserId);
                    foreach (var item in parents)
                    {
                        if (quizzClass.TeacherId == item)
                            continue;

                        QuizzClassMember parent = new QuizzClassMember
                        {
                            IsNew = true,
                            IsNewInviteAccepted = false,
                            QuizzClassId = model.QuizzClassId,
                            StudentId = item,
                            DependentId = model.UserId,
                            IsParent = true
                        };

                        _uow.QuizzClassMembers.Add(parent);
                    }
                }

                _uow.SaveChanges();

                var newModel = _uow.QuizzClasses.GetAll()
                    .Where(qc => qc.Id == model.QuizzClassId)
                    .ProjectTo<QuizzClassModel>(new { userId = _currentUser.Id })
                    .FirstOrDefault();

                SetAge(newModel);

                newModel.TeacherName = newModel.IsTeacherQuizzmate ? newModel.TeacherFullName : newModel.TeacherUserName;

                return newModel;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;    
            }
        }

        // accepted/rejected by user
        public bool Patch(object modelParam)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<QuizzClassInviteRequestModel>(modelParam.ToString());
                if (model == null)
                    return false;

                var inviteEntity = _uow.QuizzClassInviteRequests.GetAll()
                    .Where(qci => qci.QuizzClassId == model.QuizzClassId && qci.UserId == _currentUser.Id && qci.IsDeleted == false)
                    .FirstOrDefault();
                inviteEntity.IsDeleted = true;
                _uow.QuizzClassInviteRequests.Update(inviteEntity);

                _svcContainer.QuizzClassJoinSvc.DeleteJoinRequest(model.QuizzClassId, false);

                if (model.IsAccepted)
                {
                    QuizzClassMember entity = new QuizzClassMember
                    {
                        IsNew = false,
                        IsNewInviteAccepted = true,
                        QuizzClassId = model.QuizzClassId,
                        StudentId = _currentUser.Id,
                        DependentId = 0,
                        IsParent = false
                    };
                    _uow.QuizzClassMembers.Add(entity);

                    var quizzClass = _uow.QuizzClasses.GetById(model.QuizzClassId);
                    var parents = _svcContainer.DependentSvc.GetParentsOfUserId(model.UserId);
                    foreach (var item in parents)
                    {
                        if (quizzClass.TeacherId == item)
                            continue;

                        QuizzClassMember parent = new QuizzClassMember
                        {
                            IsNew = true,
                            IsNewInviteAccepted = false,
                            QuizzClassId = model.QuizzClassId,
                            StudentId = item,
                            DependentId = model.UserId,
                            IsParent = true
                        };

                        _uow.QuizzClassMembers.Add(parent);
                    }
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