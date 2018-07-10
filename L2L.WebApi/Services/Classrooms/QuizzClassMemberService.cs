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
    public class QuizzClassMemberService : BaseService, IResource
    {
        public QuizzClassMemberService(BaseApiController controller)
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
                var type = id2; // 0 - not from owner
                                // 1 - from owner
                var list = _uow.QuizzClassMembers.GetAll()
                    .Where(qcm => qcm.QuizzClassId == quizzClassId && 
                            qcm.StudentId != qcm.QuizzClass.TeacherId &&
                            qcm.IsParent == false)
                    .ProjectTo<QuizzClassMemberModel>(new { userId = _currentUser.Id })
                    .ToList();

                UpdateModelList(list);

                switch (type)
                {
                    case 0:
                        foreach (var item in list)
                        {
                            // need to set to false so that it will not hightlight
                            item.IsNewInviteAccepted = false;
                        }
                        break;
                    case 1:
                        // remove new marker
                        var hasChanges = false;
                        foreach (var item in list)
                        {
                            if(item.IsNewInviteAccepted)
                            {
                                QuizzClassMember entity;
                                item.MapToNew(out entity);
                                entity.IsNewInviteAccepted = false;
                                _uow.QuizzClassMembers.Update(entity);
                                hasChanges = true;
                            }
                        }
                        if (hasChanges)
                            _uow.SaveChanges();
                        break;
                    default:
                        break;
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

        // accept/reject request
        public object Post(object modelParam)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<QuizzClassJoinRequestModel>(modelParam.ToString());
                if (model == null)
                    return null;

                QuizzClassJoinRequest requestEntity;
                model.MapToNew(out requestEntity);
                requestEntity.IsDeleted = true;
                _uow.QuizzClassJoinRequests.Update(requestEntity);

                _svcContainer.QuizzClassInviteSvc.DeleteInviteRequest(model.QuizzClassId, false);

                if (model.IsAccepted)
                {
                    QuizzClassMember entity = new QuizzClassMember
                    {
                        IsNew = true,
                        IsNewInviteAccepted = false,
                        QuizzClassId = model.QuizzClassId,
                        StudentId = model.UserId,
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

                    _uow.SaveChanges();

                    var memberModel = _uow.QuizzClassMembers.GetAll()
                        .Where(qcm => qcm.Id == entity.Id)
                        .ProjectTo<QuizzClassMemberModel>()
                        .FirstOrDefault();

                    UpdateModel(memberModel);
                    return memberModel;
                }
                else
                {
                    _uow.SaveChanges();
                    return model; // return dummy
                }
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        // remove current user using quizzClass
        public bool Patch(object modelParam)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<QuizzClassModel>(modelParam.ToString());
                if (model == null)
                    return false;

                var entity = _uow.QuizzClassMembers.GetAll()
                    .Where(qcm => qcm.QuizzClassId == model.Id && qcm.StudentId == _currentUser.Id)
                    .FirstOrDefault();

                var list = _uow.QuizzClassMembers.GetAll()
                    .Where(qcm => qcm.QuizzClassId == model.Id && qcm.DependentId == _currentUser.Id)
                    .ToList();

                var quizzClass = _uow.QuizzClasses.GetById(model.Id);
                foreach (var item in list)
                {
                    if (quizzClass.TeacherId == item.StudentId)
                        continue;

                    _uow.QuizzClassMembers.Delete(item.Id);
                }

                _uow.QuizzClassMembers.Delete(entity.Id);
                _uow.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        // remove 
        public bool Delete(int id)
        {
            try
            {
                var entity = _uow.QuizzClassMembers.GetById(id);
                var list = _uow.QuizzClassMembers.GetAll()
                    .Where(qcm => qcm.QuizzClassId == entity.QuizzClassId && qcm.DependentId == entity.StudentId)
                    .ToList();

                var quizzClass = _uow.QuizzClasses.GetById(entity.QuizzClassId);
                foreach (var item in list)
                {
                    if (quizzClass.TeacherId == item.StudentId)
                        continue;
                    _uow.QuizzClassMembers.Delete(item.Id);
                }

                _uow.QuizzClassMembers.Delete(id);
                _uow.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        public void UpdateModelList(IEnumerable<QuizzClassMemberModel> list)
        {
            foreach (var item in list)
            {
                UpdateModel(item);
            }
        }

        public void UpdateModel(QuizzClassMemberModel model)
        {
            SetAge(model);
            model.StudentName = model.IsQuizzmate ? model.StudentFullName : model.StudentUserName;
            model.StudentFullName = "";
            model.StudentUserName = "";
        }
    }
}