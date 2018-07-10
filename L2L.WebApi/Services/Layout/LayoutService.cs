using AutoMapper.QueryableExtensions;
using L2L.WebApi.Controllers;
using L2L.WebApi.Creator;
using L2L.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Services
{
    public class LayoutService : BaseService, IResource
    {
        const int _numItemsPerGroup = 3;
        public LayoutService(BaseApiController controller)
            : base(controller)
        {
        }

        public LayoutModel GetLayoutModel()
        {
            var creator = new LayoutModelCreator(_controller);
            var model = creator.CreateLayoutModel();

            return model;
        }

        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            try
            {
                var type = id;

                switch (type)
                {
                    case 0: // My Quizzes
                        return GetMyQuizzes();
                    case 1: // Quizzlings
                        return GetDependents();
                    case 2: // Recent Test Resuts
                        return GetRecentTestResults();
                    case 3:
                        return GetGivenAssignments();
                    case 4:
                        return GetAssignments();
                    default:
                        break;
                }

                return null;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        private object GetAssignments()
        {
            var list = _uow.Assignments.GetAll()
                    .Where(a => a.DependentId == _currentUser.Id && a.IsCompleted == false)
                    .OrderBy(a => a.AssignmentGroup.TargetDate)
                    .ThenByDescending(a => a.AssignmentGroup.DateAssigned)
                    .Take(_numItemsPerGroup)
                    .ProjectTo<LayoutAssignmentModel>()
                    .ToList();

            return list;
        }

        private object GetGivenAssignments()
        {
            var list = _uow.AssignmentGroups.GetAll()
                    .Where(a => a.AssignedById == _currentUser.Id && a.IsDeleted == false && a.IsCompleted == false)
                    .OrderBy(a => a.TargetDate)
                    .ThenByDescending(a => a.DateAssigned)
                    .Take(_numItemsPerGroup)
                    .ProjectTo<LayoutAssignmentGroupModel>()
                    .ToList();

            return list;
        }

        private object GetMyQuizzes()
        {
            var list = _uow.Quizzes.GetAll()
                  .Where(q => q.OwnerId == _currentUser.Id && q.IsDeleted == false)
                  .OrderByDescending(q => q.Modified)
                  .Take(_numItemsPerGroup)
                  .ProjectTo<LayoutQuizzModel>()
                  .ToList();

            return list;
        }

        private object GetDependents()
        {
            var user = _uow.Users.GetAll()
                .Include(u => u.AsUserDependents.Select(d => d.Child.Profile))
                .Where(u => u.Id == _currentUser.Id)
                .FirstOrDefault();

            var list = new List<LayoutDependentModel>();

            if (user.AsUserDependents.Count != 0)
            {
                foreach (var item in user.AsUserDependents)
                {
                    list.Add(new LayoutDependentModel()
                    {
                        UserId = item.ChildId,
                        DependentName = item.Child.Profile.FirstName + " " + item.Child.Profile.LastName,
                    });
                }
            }

            return list;
        }

        private object GetRecentTestResults()
        {
            var list = _uow.QuizLogs.GetAll()
               .Where(t => t.UserId == _currentUser.Id)
               .OrderByDescending(t => t.DateTaken)
               .Take(_numItemsPerGroup)
               .ProjectTo<LayoutRecentQuizzModel>()
               .ToList();

            return list;
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
    }
}