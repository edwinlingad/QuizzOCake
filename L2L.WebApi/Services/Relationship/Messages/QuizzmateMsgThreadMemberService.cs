using L2L.WebApi.Controllers;
using L2L.WebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.Utilities;
using L2L.Entities;

namespace L2L.WebApi.Services
{
    public class QuizzmateMsgThreadMemberService : BaseService, IResource
    {
        public QuizzmateMsgThreadMemberService(BaseApiController controller)
            : base(controller)
        {
        }

        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        public object Get(int id)
        {
            throw new NotImplementedException();
        }

        public object Post(object modelParam)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<QuizzmateMsgThreadMemberModel>(modelParam.ToString());
                if (model == null)
                    return null;

                var tmpEntity = _uow.QuizzmateMsgThreadMembers.GetAll()
                    .Where(qm => qm.QuizzmateMsgThreadId == model.QuizzmateMsgThreadId && qm.UserId == model.UserId)
                    .FirstOrDefault();

                if (tmpEntity != null)
                    return null;

                QuizzmateMsgThreadMember entity = null;
                model.MapToNew(out entity);
                entity.HasNew = false;
                entity.NewCount = 0;

                _uow.QuizzmateMsgThreadMembers.Add(entity);
                _uow.SaveChanges();

                model = MappingUtil.Map<QuizzmateMsgThreadMember, QuizzmateMsgThreadMemberModel>(entity);
                UpdateModel(model);
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
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            try
            {
                _uow.QuizzmateMsgThreadMembers.Delete(id);
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

        public void UpdateModelList(IEnumerable<QuizzmateMsgThreadMemberModel> list)
        {
            foreach (var item in list)
            {
                UpdateModel(item);
            }
        }

        public void UpdateModel(QuizzmateMsgThreadMemberModel model)
        {
            SetAge(model);
        }
    }
}