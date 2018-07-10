using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using L2L.WebApi.Models;
using L2L.Entities;
using L2L.WebApi.Utilities;
using L2L.Entities.Enums;
using L2L.WebApi.Controllers;
using AutoMapper.QueryableExtensions;

namespace L2L.WebApi.Services
{
    public class DependentService : BaseService
    {
        public DependentService(BaseApiController controller)
            :base(controller)
        {
        }

        public DependentUserModel GetDependentInfo(int id)
        {
            try
            {
                var model = _uow.Users.GetAll()
                    .Where(u => u.Id == id)
                    .Include(u => u.DependentPermission)
                    .ProjectTo<DependentUserModel>(new { parentId = _currentUser.Id })
                    .FirstOrDefault();

                model.Profile.BirthDate = model.Profile.BirthDate.ToLocalTime();

                if (model.UserType != UserTypeEnum.Child)
                    return null;

                return model;
            }
            catch (Exception ex)
            {
                _loggingSvc.Log(ex);
                return null;
            }
        }

        public IEnumerable<int> GetParentsOfUserId(int childId)
        {
            try
            {
                var list = _uow.Users.GetAll()
                    .Where(u => u.Id == childId)
                    .Select(u => u.AsChildDependsOn.Select(d => d.UserId))
                    .FirstOrDefault();

                return list;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        public bool UpdatePermissions(DependentPermissionModel model)
        {
            try
            {
                var entity = MappingUtil.Map<DependentPermissionModel, DependentPermission>(model);
                _uow.DependentPermissions.Update(entity);
                _uow.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        public bool UpdateNotifications(DependentNotificationModel model)
        {
            try
            {
                var dependentEntity = _uow.Dependents.GetAll()
                    .Where(d => d.ChildId == model.ChildId && d.UserId == _currentUser.Id)
                    .FirstOrDefault();
                if (dependentEntity != null)
                {
                    var entity = MappingUtil.Map<DependentNotificationModel, Dependent>(model);

                    entity.Id = dependentEntity.Id;
                    entity.IsPrimary = dependentEntity.IsPrimary;
                    entity.UserId = dependentEntity.UserId;
                    entity.ChildId = dependentEntity.ChildId;

                    _uow.Dependents.Detach(dependentEntity);
                    
                    _uow.Dependents.Update(entity);
                    _uow.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        private LoggingService __loggingSvc;
        private LoggingService _loggingSvc
        {
            get
            {
                if (__loggingSvc == null)
                    __loggingSvc = _svcContainer.LoggingSvc;
                return __loggingSvc;
            }
        }
    }
}