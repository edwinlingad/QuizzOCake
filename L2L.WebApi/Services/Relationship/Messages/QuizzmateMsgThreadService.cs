using L2L.WebApi.Controllers;
using L2L.WebApi.Models;
using L2L.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.Utilities;
using System.Data.Entity;
using AutoMapper.QueryableExtensions;

namespace L2L.WebApi.Services
{
    public class QuizzmateMsgThreadService : BaseService, IResource
    {
        public QuizzmateMsgThreadService(BaseApiController controller)
            : base(controller)
        {
        }

        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            throw new NotImplementedException();
        }

        public object GetManyAlt(int id, int id2, int id3, string str, string str2, string str3)
        {
            throw new NotImplementedException();
        }

        public object Get(int id)
        {
            try
            {
                var userId = id;

                var model = _uow.QuizzmateMsgThreads.GetAll()
                    .Where(mt =>
                        mt.MsgThreadMembers.Select(tm => tm.UserId).Contains(userId) &&
                        mt.MsgThreadMembers.Select(tm => tm.UserId).Contains(_currentUser.Id))
                    .ProjectTo<QuizzmateMsgThreadModel>()
                    .FirstOrDefault();

                if (model == null)
                {
                    var qtMember1 = new QuizzmateMsgThreadMember
                    {
                        HasNew = false,
                        NewCount = 0,
                        UserId = userId,
                        UpdatedDate = DateTime.UtcNow
                    };

                    var qtMember2 = new QuizzmateMsgThreadMember
                    {
                        HasNew = false,
                        NewCount = 0,
                        UserId = _currentUser.Id,
                        UpdatedDate = DateTime.UtcNow
                    };

                    var entity = new QuizzmateMsgThread
                    {
                        IsGroupMsg = false,
                        GroupMessageName = "",
                        SignalRGroupName = Guid.NewGuid().ToString(),
                        IsDeleted = false
                    };

                    entity.MsgThreadMembers.Add(qtMember1);
                    entity.MsgThreadMembers.Add(qtMember2);

                    _uow.QuizzmateMsgThreads.Add(entity);
                    _uow.SaveChanges();

                    entity.MapToNew(out model);

                    model.GroupMessageName = _uow.Users.GetAll()
                        .Where(u => u.Id == userId)
                        .Select(u => u.UserName)
                        .FirstOrDefault();
                }
                else
                {
                    if (IsMemberOrIsParent(model) == false)
                        return null;

                    var needCallSaveChanges = false;
                    if (model.IsDeleted == true)
                    {
                        var entity = _uow.QuizzmateMsgThreads.GetById(model.Id);
                        entity.IsDeleted = false;
                        _uow.QuizzmateMsgThreads.Update(entity);
                        needCallSaveChanges = true;
                    }

                    var messages = _uow.QuizzmateMsg1s.GetAll()
                        .Where(qm => qm.QuizzmateMsgThreadId == model.Id)
                        .OrderByDescending(qm => qm.PostedDate)
                        .Take(20)
                        .ProjectTo<QuizzmateMsgModel>()
                        .ToList();
                    messages.Reverse();
                    model.Messages = messages;

                    foreach (var item in model.MsgThreadMembers)
                    {
                        if (item.UserId == userId)
                            model.GroupMessageName = item.UserFullName;

                        if (item.UserId == _currentUser.Id)
                        {
                            QuizzmateMsgThreadMember msgThreadMemberEntity;
                            item.MapToNew(out msgThreadMemberEntity);
                            msgThreadMemberEntity.NewCount = 0;
                            msgThreadMemberEntity.HasNew = false;
                            _uow.QuizzmateMsgThreadMembers.Update(msgThreadMemberEntity);
                        }
                    }

                    if (needCallSaveChanges)
                        _uow.SaveChanges();
                }

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
                var threadId = Int32.Parse(str);

                var model = _uow.QuizzmateMsgThreads.GetAll()
                    .Where(mt => mt.Id == threadId)
                    .ProjectTo<QuizzmateMsgThreadModel>()
                    .FirstOrDefault();

                var messages = _uow.QuizzmateMsg1s.GetAll()
                    .Where(qm => qm.QuizzmateMsgThreadId == threadId)
                    .OrderByDescending(qm => qm.PostedDate)
                    .Take(20)
                    .ProjectTo<QuizzmateMsgModel>()
                    .ToList();
                messages.Reverse();
                model.Messages = messages;

                foreach (var item in model.MsgThreadMembers)
                {
                    if (item.UserId != _currentUser.Id)
                    {
                        if (model.IsGroupMsg == false)
                            model.GroupMessageName = item.UserName;
                    }

                    if (item.UserId == _currentUser.Id)
                    {
                        QuizzmateMsgThreadMember msgThreadMemberEntity;
                        item.MapToNew(out msgThreadMemberEntity);
                        msgThreadMemberEntity.NewCount = 0;
                        msgThreadMemberEntity.HasNew = false;
                        _uow.QuizzmateMsgThreadMembers.Update(msgThreadMemberEntity);
                        _uow.SaveChanges();
                    }
                }

                if (IsMemberOrIsParent(model) == false)
                    return null;
                UpdateModel(model);
                return model;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        // Create new thread 
        public object Post(object modelParam)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<QuizzmateMsgThreadModel>(modelParam.ToString());
                if (model == null)
                    return null;

                QuizzmateMsgThread entity = null;
                model.MapToNew(out entity);
                var quizzmateThreadMember = new QuizzmateMsgThreadMember
                {
                    UserId = _currentUser.Id
                };
                entity.MsgThreadMembers.Add(quizzmateThreadMember);
                entity.SignalRGroupName = Guid.NewGuid().ToString();

                _uow.QuizzmateMsgThreads.Add(entity);
                _uow.SaveChanges();

                model = MappingUtil.Map<QuizzmateMsgThread, QuizzmateMsgThreadModel>(entity);

                if (IsMemberOrIsParent(model) == false)
                    return null;
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
                var model = JsonConvert.DeserializeObject<QuizzmateMsgThreadModel>(modelParam.ToString());
                if (model == null)
                    return false;

                QuizzmateMsgThread entity = null;
                model.MapToNew(out entity);

                _uow.QuizzmateMsgThreads.Update(entity);
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

        public void UpdateModelList(IEnumerable<QuizzmateMsgThreadModel> list)
        {
            foreach (var item in list)
            {
                UpdateModel(item);
            }
        }

        public void UpdateModel(QuizzmateMsgThreadModel model)
        {
            UpdateIsViewerOnly(model);
            _svcContainer.QuizzmateMsgThreadMemberSvc.UpdateModelList(model.MsgThreadMembers);
        }

        private void UpdateIsViewerOnly(QuizzmateMsgThreadModel model)
        {

            var member = model.MsgThreadMembers
                .Where(mtm => mtm.UserId == _currentUser.Id)
                .FirstOrDefault();
            if (member != null)
            {
                model.IsViewerOnly = false;
                return;
            }

            // no need to check if parent since IsMemberOrIsParent should already been called before calling this

            model.IsViewerOnly = true;
        }

        private bool IsMemberOrIsParent(QuizzmateMsgThreadModel model)
        {
            foreach (var item in model.MsgThreadMembers)
            {
                if (item.UserId == _currentUser.Id)
                    return true;
                if (_svcContainer.UserSvc.IsDependent(item.UserId))
                    return true;
            }

            return false;
        }
    }
}