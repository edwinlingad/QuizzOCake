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
    public class AssignmentService : BaseService, IResource
    {
        public AssignmentService(BaseApiController controller)
            : base(controller)
        {
        }

        // id - dependentId
        // id2 - pageNum
        // id3 - 0 - not completed, 1 - completed
        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            try
            {
                int dependentId = id;
                var dependent = _uow.Users.GetAll()
                    .Where(u => u.Id == dependentId)
                    .Include(u => u.AsChildDependsOn)
                    .FirstOrDefault();
                
                if (dependentId != _currentUser.Id)
                {
                    // Check if parent
                    bool isParent = false;
                    foreach (var item in dependent.AsChildDependsOn)
                    {
                        if (item.UserId == _currentUser.Id)
                            isParent = true;
                    }

                    if (isParent == false)
                        return null;
                }

                int pageNum = id2;
                bool completed = id3 == 1;
                var list = _uow.Assignments.GetAll()
                    .Where(a => a.DependentId == dependentId && a.IsCompleted == completed)
                    .Include(a => a.AssignmentGroup)
                    .OrderBy(a => a.AssignmentGroup.TargetDate)
                    .ThenBy(a => a.AssignmentGroup.DateAssigned)
                    .Skip(25 * (pageNum - 1))
                    .Take(25)
                    .ProjectTo<AssignmentModel>()
                    .ToList();

                foreach (var item in list)
                {
                    ConvertToLocalTime(item);
                    ConvertToCompleted(item);
                }

                return list;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        private void ConvertToCompleted(AssignmentModel model)
        {
            if (model.IsCompleted)
            {
                model.AssignmentGroup.Message = model.CompletedMessage;
                model.AssignmentGroup.TestSetting = model.CompletedTestSetting.FromJson<TestSettingModel>();
            }
        }

        private void ConvertToLocalTime(AssignmentModel model)
        {
            model.CompletedDate = model.CompletedDate.ToLocalTime();
            model.AssignmentGroup.DateAssigned = model.AssignmentGroup.DateAssigned.ToLocalTime();
            model.AssignmentGroup.TargetDate = model.AssignmentGroup.TargetDate.ToLocalTime();
        }

        public object Get(int id)
        {
            try
            {
                var model = _uow.Assignments.GetAll()
                    .Where(a => a.Id == id)
                    .Include(a => a.AssignmentGroup)
                    .ProjectTo<AssignmentModel>()
                    .FirstOrDefault();

                ConvertToCompleted(model);
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
                var model = JsonConvert.DeserializeObject<AssignmentModel>(modelParam.ToString());
                if (model == null)
                    return null;

                Assignment entity;
                model.MapToNew<AssignmentModel, Assignment>(out entity);
                entity.AssignmentGroup = null;
                entity.CompletedDate = DateTime.UtcNow; ;

                _uow.Assignments.Add(entity);
                _uow.SaveChanges();

                MappingUtil.Map(entity, model);

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
                var model = JsonConvert.DeserializeObject<AssignmentModel>(modelParam.ToString());
                if (model == null)
                    return false;
                AssignmentGroupModel assignmentGroupModel = model.AssignmentGroup;
                model.AssignmentGroup = null;

                Assignment entity;
                model.MapToNew<AssignmentModel, Assignment>(out entity);

                if (entity.IsCompleted)
                {
                    entity.CompletedDate = DateTime.UtcNow;
                    entity.CompletedMessage = assignmentGroupModel.Message;
                    entity.CompletedTestSetting = assignmentGroupModel.TestSetting.ToJson();

                    _notificationSvc.AssignmentNotificationSvc.AddAssignmentFinishedNotification(assignmentGroupModel.QuizzId, assignmentGroupModel.Id, assignmentGroupModel.AssignedById, false);
                }

                _uow.Assignments.Update(entity);

                UpdateAssignmentGroup(entity.AssignmentGroupId, entity.Id);

                _uow.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        private void UpdateAssignmentGroup(int assGId, int assId)
        {
            var assGroup = _uow.AssignmentGroups.GetAll()
                .Where(ag => ag.Id == assGId)
                .Include(ag => ag.Assignments)
                .FirstOrDefault();
            bool isComplete = true;
            foreach (var item in assGroup.Assignments)
            {
                if (item.Id == assId)
                    continue;

                if (item.IsCompleted == false)
                    isComplete = false;
            }

            if(isComplete)
            {
                assGroup.IsCompleted = true;
                _uow.AssignmentGroups.Update(assGroup);
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var entity = _uow.Assignments.GetById(id);
                if (entity != null)
                {
                    if (entity.IsCompleted)
                        return false;
                }

                _uow.Assignments.Delete(id);

                UpdateAssignmentGroup(entity.AssignmentGroupId, entity.Id);

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