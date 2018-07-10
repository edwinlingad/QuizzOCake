using L2L.Data;
using L2L.WebApi.Controllers;
using L2L.WebApi.Models;
using L2L.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using L2L.WebApi.Utilities;
using L2L.Entities;
using AutoMapper.QueryableExtensions;

namespace L2L.WebApi.Creator
{
    public class LayoutModelCreator
    {
        private int _numItemsPerGroup = 3;
        private BaseApiController _controller;
        private UserModel _currentUser;
        private ApplicationUnit _uow;
        public LayoutModelCreator(BaseApiController controller)
        {
            _controller = controller;
            _currentUser = _controller.SvcContainer.UserSvc.GetCurrentUserModel();
            _uow = _controller.Uow;

        }

        private LayoutModel _layoutModel;
        public LayoutModel CreateLayoutModel()
        {
            _layoutModel = new LayoutModel();

            if (_currentUser.Id != 0)
            {
                GetTopPanel();
                GetLeftSideBar();
            }

            return _layoutModel;
        }

        private void GetTopPanel()
        {
            GetNewQUizzClassNotificationCount();
            GetNewFriendRequestCount();
            GetNewMessageCount();
            GetNewNotificationCount();
        }

        private void GetNewQUizzClassNotificationCount()
        {
            _layoutModel.TopPanel.NewQuizzClassNotificationCount = _svcContainer.QuizzClassMemberUpdateSvc.GetNewNotificationCount();
        }

        private void GetNewFriendRequestCount()
        {
            _layoutModel.TopPanel.NewFriendRequestCount = _svcContainer.NewRelationshipNotificationSvc.GetNewRNotificationCount();
        }

        private void GetNewMessageCount()
        {
            var list = _uow.QuizzmateMsgThreadMembers.GetAll()
               .Where(qm => qm.UserId == _currentUser.Id && qm.HasNew == true)
               .Select(qm => qm.NewCount);
            int count = 0;
            foreach (var item in list)
            {
                count += item;
            }
            _layoutModel.TopPanel.NewMessageCount = count;
        }

        private void GetNewNotificationCount()
        {
            _layoutModel.TopPanel.NewNotificationCount = _svcContainer.NotificationSvc.GetNewNotificationCount();
        }

        private void GetLeftSideBar()
        {
            GetTestSnapshot();
            GetDependents();
            GetAssignments();
            GetRecentQuizzes();
            GetMyQuizzes();
            GetBookmarks();
            GetSuggestedQuizzes();
            GetGroups();
        }

        private void GetTestSnapshot()
        {
            var list = _uow.TestSnapshots.GetAll()
                .Where(p => p.OwnerId == _currentUser.Id)
                .ProjectTo<LayoutTestSnapshot>()
                .ToList();

            foreach (var item in list)
                _layoutModel.LeftSideBar.TestSnapshots.Add(item);
        }

        private void GetDependents()
        {
            var user = _uow.Users.GetAll()
                .Include(u => u.AsUserDependents.Select(d => d.Child.Profile))
                .Where(u => u.Id == _currentUser.Id)
                .FirstOrDefault();

            if (user.AsUserDependents.Count != 0)
            {
                foreach (var item in user.AsUserDependents)
                {
                    _layoutModel.LeftSideBar.Dependents.Add(new LayoutDependentModel()
                    {
                        UserId = item.ChildId,
                        DependentName = item.Child.Profile.FirstName + " " + item.Child.Profile.LastName,
                    });
                }
            }
        }

        private void GetAssignments()
        {
            GetMyAssignments();
            GetAssignmentsGiven();
        }

        private void GetAssignmentsGiven()
        {
            var list = _uow.AssignmentGroups.GetAll()
                    .Where(a => a.AssignedById == _currentUser.Id && a.IsDeleted == false && a.IsCompleted == false)
                    .OrderBy(a => a.TargetDate)
                    .ThenByDescending(a => a.DateAssigned)
                    .Take(_numItemsPerGroup)
                    .ProjectTo<LayoutAssignmentGroupModel>()
                    .ToList();

            _layoutModel.LeftSideBar.AssignmentsGiven = list;

            var count = _uow.AssignmentGroups.GetAll()
                    .Where(a => a.AssignedById == _currentUser.Id && a.IsDeleted == false && a.IsCompleted == false)
                    .Count();

            _layoutModel.LeftSideBar.HasMoreAssignmentsGiven = count > _numItemsPerGroup;
        }

        private void GetMyAssignments()
        {
            var list = _uow.Assignments.GetAll()
                    .Where(a => a.DependentId == _currentUser.Id && a.IsCompleted == false)
                    .OrderBy(a => a.AssignmentGroup.TargetDate)
                    .ThenByDescending(a => a.AssignmentGroup.DateAssigned)
                    .Take(_numItemsPerGroup)
                    .ProjectTo<LayoutAssignmentModel>()
                    .ToList();

            _layoutModel.LeftSideBar.Assignments = list;

            var count = _uow.Assignments.GetAll()
                    .Where(a => a.DependentId == _currentUser.Id && a.IsCompleted == false)
                    .Count();
            _layoutModel.LeftSideBar.HasMoreAssignments = count > _numItemsPerGroup;
        }

        private void GetRecentQuizzes()
        {
            var list = _uow.QuizLogs.GetAll()
               .Where(t => t.UserId == _currentUser.Id)
               .OrderByDescending(t => t.DateTaken)
               .Take(_numItemsPerGroup)
               .ProjectTo<LayoutRecentQuizzModel>()
               .ToList();

            _layoutModel.LeftSideBar.RecentQuizzes = list;

            var count = _uow.QuizLogs.GetAll()
               .Where(t => t.UserId == _currentUser.Id)
               .Count();

            _layoutModel.LeftSideBar.HasMoreRecentQuizzes = count > _numItemsPerGroup;
        }

        private void GetMyQuizzes()
        {
            var list = _uow.Quizzes.GetAll()
                .Where(q => q.OwnerId == _currentUser.Id && q.IsDeleted == false)
                .OrderByDescending(q => q.Modified)
                .Take(_numItemsPerGroup)
                .ProjectTo<LayoutQuizzModel>()
                .ToList();

            _layoutModel.LeftSideBar.MyQuizzes = list;

            var count = _uow.Quizzes.GetAll()
                .Where(q => q.OwnerId == _currentUser.Id)
                .Count();

            _layoutModel.LeftSideBar.HasMoreMyQuizzes = count > _numItemsPerGroup;
        }

        private void GetBookmarks()
        {
            _layoutModel.LeftSideBar.Bookmarks.Add(new LayoutBookmarkModel()
            {
                QuizzName = "Bookmark 1"
            });
            _layoutModel.LeftSideBar.Bookmarks.Add(new LayoutBookmarkModel()
            {
                QuizzName = "Bookmark 2"
            });
        }

        private void GetSuggestedQuizzes()
        {
            _layoutModel.LeftSideBar.SuggestedQuizzes.Add(new LayoutSuggestedQuizzModel()
            {
                QuizzName = "Suggested Quizz 1"
            });
            _layoutModel.LeftSideBar.SuggestedQuizzes.Add(new LayoutSuggestedQuizzModel()
            {
                QuizzName = "Suggested Quizz 2s"
            });
        }

        private void GetGroups()
        {
            _layoutModel.LeftSideBar.Groups.Add(new LayoutGroupModel()
            {
                GroupName = "Group 1"
            });
            _layoutModel.LeftSideBar.Groups.Add(new LayoutGroupModel()
            {
                GroupName = "Group 2"
            });
        }

        private ServiceContainer __serviceContainer;
        public ServiceContainer _svcContainer
        {
            get
            {
                if (__serviceContainer == null)
                    __serviceContainer = _controller.SvcContainer;
                return __serviceContainer;
            }
        }
    }
}