using L2L.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.Entities;
using L2L.WebApi.Models;
using AutoMapper.QueryableExtensions;

namespace L2L.WebApi.Services
{
    public class NewRelationshipNotificationService : BaseService, IResource
    {
        public NewRelationshipNotificationService(BaseApiController controller)
            : base(controller)
        {
        }

        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            try
            {
                var list = _uow.RelationshipNotifications.GetAll()
                    .OrderByDescending(r => r.IsNew == true)
                    .Where(r => r.ToUserId == _currentUser.Id)
                    .Take(5)
                    .ProjectTo<RelationshipNotificationModel>()
                    .ToList();

                return list;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        // Get new relationship request count
        public object Get(int id)
        {
            try
            {
                var count = GetNewRNotificationCount();

                return new { count = count };
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        public int GetNewRNotificationCount()
        {
            var count = _uow.RelationshipNotifications.GetAll()
                   .Where(r => r.ToUserId == _currentUser.Id && r.IsNew == true)
                   .Count();

            return count;
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

        public object GetManyAlt(int id, int id2, int id3, string str, string str2, string str3)
        {
            throw new NotImplementedException();
        }

        public object GetAlt(string str)
        {
            throw new NotImplementedException();
        }
    }
}