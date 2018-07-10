using AutoMapper.QueryableExtensions;
using L2L.Entities;
using L2L.WebApi.Controllers;
using L2L.WebApi.Helper;
using L2L.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace L2L.WebApi.Services
{
    public enum QuizzClassMemberUpdateTypeEnum
    {
        ClassAnnouncement,
        ClassDiscussion,
        ClassLesson,
        LessonDiscussion,
        LessonTopic
    }

    public class QuizzClassMemberUpdateModel
    {
        public QuizzClassMemberUpdateTypeEnum QcType { get; set; }
        public int QuizzClassId { get; set; }
        public int QuizzClassLessonId { get; set; }
        public int QuizzClassLessonMemberIdx { get; set; }
    }

    [Authorize]
    public class QuizzClassMemberUpdateService : BaseService, IResource
    {
        public QuizzClassMemberUpdateService(BaseApiController controller)
            : base(controller)
        {
        }

        public int GetNewNotificationCount()
        {
            var count = GetMyClassesNewNotificationCount();
            count += GetEnrolledClassesNewNotificationCount();
            count += GetPendingInvitesCount();
            count += GetNewInvitesAcceptedCount();

            return count;
        }

        private int GetPendingInvitesCount()
        {
            var count = _uow.QuizzClassInviteRequests.GetAll()
                .Where(qci => qci.UserId == _currentUser.Id && qci.IsDeleted == false)
                .Count();
            return count;
        }

        private int GetNewInvitesAcceptedCount()
        {
            var count = _uow.QuizzClassMembers.GetAll()
                .Where(qcm => qcm.QuizzClass.TeacherId == _currentUser.Id && qcm.IsNewInviteAccepted == true)
                .Count();

            return count;
        }

        private int GetMyClassesNewNotificationCount()
        {
            var list = _uow.QuizzClasses.GetAll()
                .Where(qc => qc.TeacherId == _currentUser.Id && qc.IsDeleted == false)
                .ProjectTo<QuizzClassModel>(new { userId = _currentUser.Id })
                .ToList();
            var count = 0;
            foreach (var item in list)
            {
                item.Member = _uow.QuizzClassMembers.GetAll()
                    .Where(qcm => qcm.Id == item.QuizzClassMemberId)
                    .ProjectTo<QuizzClassMemberModel>()
                    .FirstOrDefault();

                count += item.NumStudentJoinRequests;
                if (item.Member != null)
                {
                    count += item.Member.NewClassCommentCount;
                    count += item.Member.NewAnnouncementCount;
                    count += item.Member.NewClassQuizzCount;

                    var intArrayHelper = new IntArray(item.Member.NewLessonMessageCount);
                    count += intArrayHelper.GetTotal();

                    intArrayHelper = new IntArray(item.Member.NewLessonCommentCount);
                    count += intArrayHelper.GetTotal();

                    intArrayHelper = new IntArray(item.Member.NewLessonQuizzCount);
                    count += intArrayHelper.GetTotal();
                }
            }

            return count;
        }

        private int GetEnrolledClassesNewNotificationCount()
        {
            var list = _uow.QuizzClassMembers.GetAll()
                .Where(qcm => qcm.StudentId == _currentUser.Id &&
                        qcm.QuizzClass.TeacherId != _currentUser.Id && 
                        qcm.IsParent == false &&
                        qcm.QuizzClass.IsDeleted == false)
                .Select(qcm => qcm.QuizzClass)
                .ProjectTo<QuizzClassModel>(new { userId = _currentUser.Id })
                .ToList();

            var count = 0;
            foreach (var item in list)
            {
                item.Member = _uow.QuizzClassMembers.GetAll()
                    .Where(qcm => qcm.Id == item.QuizzClassMemberId)
                    .ProjectTo<QuizzClassMemberModel>()
                    .FirstOrDefault();

                if (item.Member != null)
                {
                    count += item.Member.IsNew ? 1 : 0;
                    count += item.Member.NewClassLesson;
                    count += item.Member.NewClassCommentCount;
                    count += item.Member.NewAnnouncementCount;
                    count += item.Member.NewClassQuizzCount;

                    var intArrayHelper = new IntArray(item.Member.NewLessonMessageCount);
                    count += intArrayHelper.GetTotal();

                    intArrayHelper = new IntArray(item.Member.NewLessonCommentCount);
                    count += intArrayHelper.GetTotal();

                    intArrayHelper = new IntArray(item.Member.NewLessonQuizzCount);
                    count += intArrayHelper.GetTotal();
                }
            }

            return count;
        }

        #region Announcement
        public void AddAnnouncement(int qcId, bool callSaveChanges = true)
        {
            List<QuizzClassMember> list = GetMembersFromQCId(qcId);

            foreach (var item in list)
            {
                if (item.IsParent)
                    continue;
                item.NewAnnouncementCount++;
                _uow.QuizzClassMembers.Update(item);
            }

            if (callSaveChanges)
                _uow.SaveChanges();
        }

        public void SubAnnouncement(int qcId, bool callSaveChanges = true)
        {
            List<QuizzClassMember> list = GetMembersFromQCId(qcId);

            foreach (var item in list)
            {
                if (item.IsParent)
                    continue;
                item.NewAnnouncementCount--;
                if (item.NewAnnouncementCount < 0)
                    item.NewAnnouncementCount = 0;
                _uow.QuizzClassMembers.Update(item);
            }

            if (callSaveChanges)
                _uow.SaveChanges();
        }

        public int RemoveAnnouncement(int qcId, bool callSaveChanges = true)
        {
            var ret = 0;
            var entity = GetCurrentMemberFromQCId(qcId);
            if (entity != null)
            {
                ret = entity.NewAnnouncementCount;
                entity.NewAnnouncementCount = 0;
                _uow.QuizzClassMembers.Update(entity);
            }

            if (callSaveChanges)
                _uow.SaveChanges();

            return ret;
        }
        #endregion

        #region Class Discussion
        public void AddClassDiscussion(int qcId, bool callSaveChanges = true)
        {
            List<QuizzClassMember> list = GetMembersFromQCId(qcId);

            foreach (var item in list)
            {
                if (item.IsParent)
                    continue;
                item.NewClassCommentCount++;
                _uow.QuizzClassMembers.Update(item);
            }

            if (callSaveChanges)
                _uow.SaveChanges();
        }

        public void SubClassDiscussion(int qcId, bool callSaveChanges = true)
        {
            List<QuizzClassMember> list = GetMembersFromQCId(qcId);

            foreach (var item in list)
            {
                if (item.IsParent)
                    continue;

                item.NewClassCommentCount--;
                if (item.NewClassCommentCount < 0)
                    item.NewClassCommentCount = 0;
                
                _uow.QuizzClassMembers.Update(item);
            }

            if (callSaveChanges)
                _uow.SaveChanges();
        }

        public int RemoveClassDiscussion(int qcId, int countToRemove, int? depId, bool callSaveChanges = true)
        {
            var ret = 0;
            var entity = GetCurrentMemberFromQCId(qcId, depId);
            if (entity != null)
            {
                var newItemsLeft = entity.NewClassCommentCount - countToRemove;
                ret = entity.NewClassCommentCount < countToRemove ? entity.NewClassCommentCount : countToRemove;
                entity.NewClassCommentCount = newItemsLeft > 0 ? newItemsLeft : 0;
                _uow.QuizzClassMembers.Update(entity);
            }

            if (callSaveChanges)
                _uow.SaveChanges();

            return ret;
        }
        #endregion

        #region ClassLesson
        public void AddClassLesson(int qcId, bool callSaveChanges = true)
        {
            List<QuizzClassMember> list = GetMembersFromQCId(qcId);

            foreach (var item in list)
            {
                if (item.IsParent)
                    continue;

                item.NewClassLesson++;
                if (item.NewClassLesson < 0)
                    item.NewClassLesson = 0;
                _uow.QuizzClassMembers.Update(item);
            }

            if (callSaveChanges)
                _uow.SaveChanges();
        }

        public void SubClassLesson(int qcId, bool callSaveChanges = true)
        {
            List<QuizzClassMember> list = GetMembersFromQCId(qcId);

            foreach (var item in list)
            {
                if (item.IsParent)
                    continue;

                item.NewClassLesson--;
                _uow.QuizzClassMembers.Update(item);
            }

            if (callSaveChanges)
                _uow.SaveChanges();
        }

        public void RemoveClassLesson(int qcId, bool callSaveChanges = true)
        {
            var entity = GetCurrentMemberFromQCId(qcId);
            if (entity != null)
            {
                entity.NewClassLesson = 0;
                _uow.QuizzClassMembers.Update(entity);
            }

            if (callSaveChanges)
                _uow.SaveChanges();
        }
        #endregion

        #region ClassQuizz
        public void AddClassQuizz(int qcId, bool callSaveChanges = true)
        {
            List<QuizzClassMember> list = GetMembersFromQCId(qcId);

            foreach (var item in list)
            {
                if (item.IsParent)
                    continue;

                item.NewClassQuizzCount++;
                _uow.QuizzClassMembers.Update(item);
            }

            if (callSaveChanges)
                _uow.SaveChanges();
        }

        public void SubClassQuizz(int qcId, bool callSaveChanges = true)
        {
            List<QuizzClassMember> list = GetMembersFromQCId(qcId);

            foreach (var item in list)
            {
                if (item.IsParent)
                    continue;

                item.NewClassQuizzCount--;
                if (item.NewClassQuizzCount < 0)
                    item.NewClassQuizzCount = 0;
                _uow.QuizzClassMembers.Update(item);
            }

            if (callSaveChanges)
                _uow.SaveChanges();
        }

        public int RemoveClassQuizz(int qcId, bool callSaveChanges = true)
        {
            var ret = 0;
            var entity = GetCurrentMemberFromQCId(qcId);
            if (entity != null)
            {
                ret = entity.NewClassQuizzCount;
                entity.NewClassQuizzCount = 0;
                _uow.QuizzClassMembers.Update(entity);
            }

            if (callSaveChanges)
                _uow.SaveChanges();

            return ret;
        }
        #endregion

        #region Class Lesson Content
        public void AddClassLessonContent(int qcId, int qcLessonIndex, bool callSaveChanges = true)
        {
            List<QuizzClassMember> list = GetMembersFromQCId(qcId);

            foreach (var item in list)
            {
                if (item.IsParent)
                    continue;

                var intArrHelper = new IntArray(item.NewLessonMessageCount);
                intArrHelper.IncAtIndex(qcLessonIndex);
                item.NewLessonMessageCount = intArrHelper.GetStringArray();
                _uow.QuizzClassMembers.Update(item);
            }

            if (callSaveChanges)
                _uow.SaveChanges();
        }

        public void SubClassLessonContent(int qcId, int qcLessonIndex, bool callSaveChanges = true)
        {
            List<QuizzClassMember> list = GetMembersFromQCId(qcId);

            foreach (var item in list)
            {
                if (item.IsParent)
                    continue;

                var intArrHelper = new IntArray(item.NewLessonMessageCount);
                intArrHelper.DecAtIndex(qcLessonIndex);
                item.NewLessonMessageCount = intArrHelper.GetStringArray();
                _uow.QuizzClassMembers.Update(item);
            }

            if (callSaveChanges)
                _uow.SaveChanges();
        }

        public void RemoveClassLessonContent(int qcId, int qcLessonIndex, bool callSaveChanges = true)
        {
            var entity = GetCurrentMemberFromQCId(qcId);
            if (entity != null)
            {
                var intArrHelper = new IntArray(entity.NewLessonMessageCount);
                intArrHelper.ResetAtIndex(qcLessonIndex);
                entity.NewLessonMessageCount = intArrHelper.GetStringArray();
                _uow.QuizzClassMembers.Update(entity);
            }

            if (callSaveChanges)
                _uow.SaveChanges();
        }
        #endregion

        #region Class Lesson Discussion
        public void AddClassLessonDiscussion(int qcId, int qcLessonIndex, bool callSaveChanges = true)
        {
            List<QuizzClassMember> list = GetMembersFromQCId(qcId);

            foreach (var item in list)
            {
                if (item.IsParent)
                    continue;

                var intArrHelper = new IntArray(item.NewLessonCommentCount);
                intArrHelper.IncAtIndex(qcLessonIndex);
                item.NewLessonCommentCount = intArrHelper.GetStringArray();
                _uow.QuizzClassMembers.Update(item);
            }

            if (callSaveChanges)
                _uow.SaveChanges();
        }

        public void SubClassLessonDiscussion(int qcId, int qcLessonIndex, bool callSaveChanges = true)
        {
            List<QuizzClassMember> list = GetMembersFromQCId(qcId);

            foreach (var item in list)
            {
                if (item.IsParent)
                    continue;

                var intArrHelper = new IntArray(item.NewLessonCommentCount);
                intArrHelper.DecAtIndex(qcLessonIndex);
                item.NewLessonCommentCount = intArrHelper.GetStringArray();
                _uow.QuizzClassMembers.Update(item);
            }

            if (callSaveChanges)
                _uow.SaveChanges();
        }

        public int RemoveClassLessonDiscussion(int qcId, int qcLessonIndex, int countToRemove, bool callSaveChanges = true)
        {
            var ret = 0;
            var entity = GetCurrentMemberFromQCId(qcId);
            if (entity != null)
            {
                var intArrHelper = new IntArray(entity.NewLessonCommentCount);
                ret = intArrHelper.SubAtIndex(qcLessonIndex, countToRemove);
                entity.NewLessonCommentCount = intArrHelper.GetStringArray();
                _uow.QuizzClassMembers.Update(entity);
            }

            if (callSaveChanges)
                _uow.SaveChanges();

            return ret;
        }
        #endregion

        #region Class Lesson Quizz
        public void AddClassLessonQuizz(int qcId, int qcLessonIndex, bool callSaveChanges = true)
        {
            List<QuizzClassMember> list = GetMembersFromQCId(qcId);

            foreach (var item in list)
            {
                if (item.IsParent)
                    continue;

                var intArrHelper = new IntArray(item.NewLessonQuizzCount);
                intArrHelper.IncAtIndex(qcLessonIndex);
                item.NewLessonQuizzCount = intArrHelper.GetStringArray();
                _uow.QuizzClassMembers.Update(item);
            }

            if (callSaveChanges)
                _uow.SaveChanges();
        }

        public void SubClassLessonQuizz(int qcId, int qcLessonIndex, bool callSaveChanges = true)
        {
            List<QuizzClassMember> list = GetMembersFromQCId(qcId);

            foreach (var item in list)
            {
                if (item.IsParent)
                    continue;

                var intArrHelper = new IntArray(item.NewLessonQuizzCount);
                intArrHelper.DecAtIndex(qcLessonIndex);
                item.NewLessonQuizzCount = intArrHelper.GetStringArray();
                _uow.QuizzClassMembers.Update(item);
            }

            if (callSaveChanges)
                _uow.SaveChanges();
        }

        public void RemoveClassLessonQuizz(int qcId, int qcLessonIndex, bool callSaveChanges = true)
        {
            var entity = GetCurrentMemberFromQCId(qcId);
            if (entity != null)
            {
                var intArrHelper = new IntArray(entity.NewLessonQuizzCount);
                intArrHelper.ResetAtIndex(qcLessonIndex);
                entity.NewLessonQuizzCount = intArrHelper.GetStringArray();
                _uow.QuizzClassMembers.Update(entity);
            }

            if (callSaveChanges)
                _uow.SaveChanges();
        }
        #endregion

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
            throw new NotImplementedException();
        }

        // reset value
        public bool Patch(object modelParam)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        private QuizzClassMember GetCurrentMemberFromQCId(int qcId, int? depId = null)
        {
            return _uow.QuizzClassMembers.GetAll()
                .Where(qcm => qcm.QuizzClassId == qcId && 
                        qcm.StudentId == _currentUser.Id && 
                        qcm.DependentId == depId)
                .FirstOrDefault();
        }

        private List<QuizzClassMember> GetMembersFromQCId(int qcId)
        {
            return _uow.QuizzClassMembers.GetAll()
                    .Where(qcm => qcm.QuizzClassId == qcId && qcm.StudentId != _currentUser.Id)
                    .ToList();
        }
    }
}