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
    public class TestSnapshotService : BaseService, IResource
    {
        public TestSnapshotService(BaseApiController controller)
            : base(controller)
        {
        }

        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            throw new NotImplementedException();
        }

        public object Get(int id)
        {
            try
            {
                var entity = _uow.TestSnapshots.GetById(id);
                return entity;
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
                var entity = JsonConvert.DeserializeObject<TestSnapshot>(modelParam.ToString());
                entity.OwnerId = _currentUser.Id;
                if (entity == null)
                    return null;

                var list = _uow.TestSnapshots.GetAll()
                    .Where(t => t.OwnerId == _currentUser.Id)
                    .ToList();

                foreach (var item in list)
                    _uow.TestSnapshots.Delete(item);

                _uow.TestSnapshots.Add(entity);
                _uow.SaveChanges();

                return entity;
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
                var entity = JsonConvert.DeserializeObject<TestSnapshot>(modelParam.ToString());
                if (entity == null)
                    return false;

                _uow.TestSnapshots.Update(entity);
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
                _uow.TestSnapshots.Delete(id);
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
    }
}