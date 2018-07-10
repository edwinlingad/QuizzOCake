using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.Entities;
using L2L.Entities.Enums;
using L2L.WebApi.Controllers;
using L2L.WebApi.Models;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using L2L.WebApi.Utilities;
using System.Data.Entity;

namespace L2L.WebApi.Services
{
    public class AssignmentGroupService : BaseService, IResource
    {
        public AssignmentGroupService(BaseApiController controller)
            : base(controller)
        {
        }

        // id - pageNum
        // id2 - 0 - not yet completed, 1 - completed
        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            try
            {
                int pageNum = id;
                bool isCompleted = id2 == 1;
                var list = _uow.AssignmentGroups.GetAll()
                    .Where(a => a.AssignedById == _currentUser.Id && a.IsDeleted == false && a.IsCompleted == isCompleted)
                    .OrderBy(a => a.TargetDate)
                    .ThenBy(a => a.DateAssigned)
                    .Skip((pageNum - 1) * 25)
                    .Take(25)
                    .Include(a => a.Quizz)
                    .ProjectTo<AssignmentGroupModel>()
                    .ToList();

                foreach (var item in list)
                    ConvertToLocalTime(item);

                return list;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        private void ConvertToLocalTime(AssignmentGroupModel item)
        {
            item.DateAssigned = item.DateAssigned.ToLocalTime();
            item.TargetDate = item.TargetDate.ToLocalTime();
            foreach (var dependent in item.Assignments)
                dependent.CompletedDate = dependent.CompletedDate.ToLocalTime();
        }

        public object Get(int id)
        {
            try
            {
                var model = _uow.AssignmentGroups.GetAll()
                    .Where(a => a.Id == id)
                    .Include(a => a.Quizz)
                    .ProjectTo<AssignmentGroupModel>()
                    .FirstOrDefault();

                ConvertToLocalTime(model);

                return model;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        public object Post(object modelParam)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<AssignmentGroupModel>(modelParam.ToString());
                if (model == null)
                    return null;

                AssignmentGroup assignmentGroup;
                model.MapToNew<AssignmentGroupModel, AssignmentGroup>(out assignmentGroup);
                assignmentGroup.AssignedById = _currentUser.Id;
                assignmentGroup.DateAssigned = DateTime.UtcNow;
                if (assignmentGroup.NoDueDate)
                    assignmentGroup.TargetDate = new DateTime(2050, 01, 01);

                foreach (var item in assignmentGroup.Assignments)
                {
                    item.TestResultId = 1;
                    item.IsCompleted = false;
                    item.CompletedDate = DateTime.UtcNow;
                }

                _uow.AssignmentGroups.Add(assignmentGroup);
                _uow.SaveChanges();

                MappingUtil.Map(assignmentGroup, model);

                foreach (var item in assignmentGroup.Assignments)
                {
                    _notificationSvc.AssignmentNotificationSvc.AddAssignmentAssignedNotification(assignmentGroup.QuizzId, assignmentGroup.Id, item.Id, item.DependentId, false);
                }

                _uow.SaveChanges();

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
                var model = JsonConvert.DeserializeObject<AssignmentGroupModel>(modelParam.ToString());
                if (model == null)
                    return false;

                model.Quizz = null;
                AssignmentGroup assignmentGroup;
                model.MapToNew<AssignmentGroupModel, AssignmentGroup>(out assignmentGroup);

                assignmentGroup.Assignments = null;
                TestSetting testSetting = assignmentGroup.TestSetting;

                if (assignmentGroup.NoDueDate)
                    assignmentGroup.TargetDate = new DateTime(2050, 01, 01);

                _uow.AssignmentGroups.Update(assignmentGroup);
                _uow.TestSettings.Update(testSetting);
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
            try
            {
                var entity = _uow.AssignmentGroups.GetAll()
                    .Where(a => a.Id == id)
                    .Include(a => a.Assignments)
                    .FirstOrDefault();

                if (entity == null)
                    return false;

                for (int i = entity.Assignments.Count - 1; i >= 0; i--)
                {
                    var item = entity.Assignments[i];
                    if (item.IsCompleted == false)
                    {
                        _uow.Assignments.Delete(item.Id);
                        _svcContainer.DeleteNotificationSvc.DeleteAssignmentNotification(id);
                    }
                }               

                entity.IsDeleted = true;
                _uow.AssignmentGroups.Update(entity);
                _uow.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        public object GetManyAlt(int id, int id2, int id3, string str, string str2, string str3)
        {
            throw new NotImplementedException();
        }

        public object GetAlt(string str)
        {
            throw new NotImplementedException();
        }

        private NotificationService __notificationSvc;
        private NotificationService _notificationSvc
        {
            get
            {
                if (__notificationSvc == null)
                    __notificationSvc = _svcContainer.NotificationSvc;
                return __notificationSvc;
            }
        }
    }
}