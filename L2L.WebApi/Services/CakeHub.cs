using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using L2L.Entities;
using L2L.Data;
using System.Data.Entity;

namespace L2L.WebApi.Services
{
    public enum LayoutHubNotificationTypeEnum
    {
        QuizzmateMsg,
    }

    public class ActiveQuizzmateMsgThreadMember
    {
        public int QuizzmateMsgThreadId { get; set; }
        public int UserId { get; set; }
    }

    public class CakeHub : Hub
    {
        static List<ActiveQuizzmateMsgThreadMember> activeQmThreadMembers = new List<ActiveQuizzmateMsgThreadMember>();

        public void JoinLayoutGroup(string groupName)
        {
            Groups.Add(Context.ConnectionId, groupName);
        }

        public void JoinQuizzmatemsgGroup(string groupName, int qmThreadId, int userId)
        {
            Groups.Add(Context.ConnectionId, groupName);
            var aqmThreadMember = new ActiveQuizzmateMsgThreadMember
            {
                QuizzmateMsgThreadId = qmThreadId,
                UserId = userId
            };
            activeQmThreadMembers.Add(aqmThreadMember);
        }

        public void LeaveJoinQuizzmatemsgGroup(string groupName, int qmThreadId, int userId)
        {
            Groups.Remove(Context.ConnectionId, groupName);
            var aqmThreadMember = activeQmThreadMembers
                .Where(q => q.UserId == userId && q.QuizzmateMsgThreadId == qmThreadId)
                .FirstOrDefault();

            activeQmThreadMembers.Remove(aqmThreadMember);
        }

        #region Layout Messaging 
        internal static void SendLayoutMsg(int userId, LayoutHubNotificationTypeEnum type, int iParam = 0, string sParam = "")
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<CakeHub>();
            context.Clients.Group("QuizzOCake-" + userId.ToString()).layoutMsg(type, iParam, sParam);
        }

        #endregion

        #region Quizzmate Messaging
        public void SendQuizzmateMsg(int qmThreadId, string groupName, int userId, string message)
        {
            Clients.Group(groupName).broadcastQuizzmateMsg(userId, message);
            SaveQuizzmateMessage(qmThreadId, userId, message);
        }

        private void SaveQuizzmateMessage(int qmThreadId, int userId, string message)
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
                        SendLayoutMsg(item.UserId, LayoutHubNotificationTypeEnum.QuizzmateMsg, qmThreadId);

                        var aqmThreadMember = activeQmThreadMembers
                                .Where(q => q.UserId == item.UserId && q.QuizzmateMsgThreadId == qmThreadId)
                                .FirstOrDefault();
                        if (aqmThreadMember == null)
                        {
                            item.HasNew = true;
                            item.NewCount++;
                            item.UpdatedDate = DateTime.UtcNow;
                            _uow.QuizzmateMsgThreadMembers.Update(item);
                        }
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
        #endregion

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