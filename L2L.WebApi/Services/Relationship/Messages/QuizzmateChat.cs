using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using L2L.Data;
using L2L.Entities;
using System.Data.Entity;

namespace L2L.WebApi.Services
{
    public class QuizzmateChatHub : Hub
    {
        public void Send(int qmThreadId, string groupName, int userId, string message)
        {
            Clients.Group(groupName).broadcastMessage(userId, message);
            SaveMessage(qmThreadId, userId, message);
        }

        public void JoinGroup(string groupName)
        {
            Groups.Add(Context.ConnectionId, groupName);
        }

        public void LeaveGroup(string groupName)
        {
            Groups.Remove(Context.ConnectionId, groupName);
        }

        private void SaveMessage(int qmThreadId, int userId, string message)
        {
            try
            {
                var msgEntity = new QuizzmateMsg1
                {
                    Message = message,
                    PostedDate = DateTime.UtcNow,
                    QuizzmateMsgThreadId = qmThreadId,
                    AuthorId = userId
                };
                _uow.QuizzmateMsg1s.Add(msgEntity);

                var thread = _uow.QuizzmateMsgThreads.GetAll()
                    .Where(qm => qm.Id == qmThreadId)
                    .Include(qm => qm.MsgThreadMembers)
                    .FirstOrDefault();

                foreach (var item in thread.MsgThreadMembers)
                {
                    if (item.UserId != userId)
                    {
                        item.HasNew = true;
                        item.NewCount++;
                        item.UpdatedDate = DateTime.UtcNow;
                        _uow.QuizzmateMsgThreadMembers.Update(item);
                    }
                }

                _uow.SaveChanges();
            }
            catch (Exception ex)
            {
                Log(ex);
                return;
            }
        }

        private void Log(Exception ex)
        {
            return;
        }

        private ApplicationUnit __uow;
        private ApplicationUnit _uow
        {
            get
            {
                if (__uow == null)
                    __uow = new ApplicationUnit();
                return __uow;
            }
        }
    }
}