using L2L.WebApi.Controllers;
using L2L.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper.QueryableExtensions;
using System.Data.Entity;

namespace L2L.WebApi.Services
{
    public class QuizzConnectMsgThreadService : BaseService, IResource
    {
        public QuizzConnectMsgThreadService(BaseApiController controller)
            : base(controller)
        {

        }

        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            try
            {
                var userId = id != 0 ? id : _currentUser.Id;

                if (userId != _currentUser.Id)
                {
                    if (_svcContainer.UserSvc.IsDependent(userId) == false)
                        return null;
                }

                var list = new List<QuizzConnectMsgThread>();
                GetQuizzmateMsgs(userId, list);

                UpdateModelList(list);
                return list;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        private void GetQuizzmateMsgs(int userId, List<QuizzConnectMsgThread> addList)
        {
            var list = _uow.QuizzmateMsgThreadMembers.GetAll()
                .Where(qm => qm.UserId == userId && qm.QuizzmateMsgThread.IsDeleted == false)
                .Include(qm => qm.QuizzmateMsgThread.MsgThreadMembers.Select(mtm => mtm.User.Profile))
                .OrderByDescending(qm => qm.UpdatedDate)
                .ToList();

            var tmpList = new List<QuizzConnectMsgThread>();

            foreach (var item in list)
            {
                QuizzConnectMsgThread lastMsg = null;
                try
                {
                    lastMsg = _uow.QuizzmateMsgThreads.GetAll()
                        .Where(qm => qm.Id == item.QuizzmateMsgThreadId)
                        .Select(qm => qm.Messages.OrderByDescending(m => m.PostedDate).FirstOrDefault())
                        .ProjectTo<QuizzConnectMsgThread>()
                        .FirstOrDefault();

                    lastMsg.QuizzMessageType = QuizzMessageTypeEnum.Quizzmate;
                    lastMsg.NewCount = item.NewCount;
                    lastMsg.QuizzmateMsgThreadId = item.QuizzmateMsgThreadId;

                }
                catch (Exception)
                {
                    var threadMsgGroup = _uow.QuizzmateMsgThreads.GetAll()
                        .Where(qm => qm.Id == item.QuizzmateMsgThreadId)
                        .FirstOrDefault();
                    lastMsg = new QuizzConnectMsgThread
                    {
                        IsEmpty = true,
                        QuizzmateMsgThreadId = threadMsgGroup.Id,
                    };
                }

                if (item.QuizzmateMsgThread.IsGroupMsg)
                    lastMsg.GroupName = item.QuizzmateMsgThread.GroupMessageName;
                else
                {

                    if (userId != _currentUser.Id)
                    {
                        var dependent = item.QuizzmateMsgThread.MsgThreadMembers
                            .Where(m => m.UserId == userId)
                            .FirstOrDefault();
                        var depName = dependent.User.Profile.FirstName;
                        var quizzmate = item.QuizzmateMsgThread.MsgThreadMembers
                            .Where(m => m.UserId != userId)
                            .FirstOrDefault();
                        var quizzmateName = quizzmate.User.Profile.FirstName + " " + quizzmate.User.Profile.LastName;
                        if (quizzmate.UserId == _currentUser.Id)
                            quizzmateName = "you";
                        else
                            lastMsg.NewCount = 0;

                        lastMsg.GroupName = "Between " + depName + " and " + quizzmateName;
                    }
                    else
                    {
                        var quizzmate = item.QuizzmateMsgThread.MsgThreadMembers
                            .Where(m => m.UserId != userId)
                            .FirstOrDefault();
                        var quizzmateName = quizzmate.User.Profile.FirstName + " " + quizzmate.User.Profile.LastName;
                        lastMsg.GroupName = "Between You and " + quizzmateName;
                    }
                }

                tmpList.Add(lastMsg);
            }

            tmpList = tmpList.OrderByDescending(tm => tm.LastMsgPostedDate).ToList();
            foreach (var item in tmpList)
            {
                addList.Add(item);
            }
        }

        public object GetManyAlt(int id, int id2, int id3, string str, string str2, string str3)
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
            throw new NotImplementedException();
        }

        public bool Patch(object modelParam)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void RemoveQuizzmateMsgThread(int quizzmateId, bool callSaveChanges = true)
        {
            try
            {
                var list = _uow.QuizzmateMsgThreadMembers.GetAll()
                            .Where(qm => qm.UserId == _currentUser.Id && qm.QuizzmateMsgThread.IsDeleted == false)
                            .Select(qm => qm.QuizzmateMsgThread)
                            .Include(qmt => qmt.MsgThreadMembers)
                            .ToList();

                foreach (var item in list)
                {
                    if (item.MsgThreadMembers.Count == 2 && 
                        item.MsgThreadMembers.Select(mtm => mtm.UserId).Contains(quizzmateId))
                    {
                        var entity = _uow.QuizzmateMsgThreads.GetById(item.Id);
                        entity.IsDeleted = true;
                        _uow.QuizzmateMsgThreads.Update(entity);
                    }
                }

                if (callSaveChanges)
                    _uow.SaveChanges();
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
            }
        }

        public void UpdateModelList(IEnumerable<QuizzConnectMsgThread> list)
        {
            foreach (var item in list)
            {
                UpdateModel(item);
            }
        }

        public void UpdateModel(QuizzConnectMsgThread model)
        {
            model.LastMsgPostedDate = model.LastMsgPostedDate.ToLocalTime();
        }
    }
}