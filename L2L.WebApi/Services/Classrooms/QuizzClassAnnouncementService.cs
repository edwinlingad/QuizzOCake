using AutoMapper.QueryableExtensions;
using L2L.Entities;
using L2L.WebApi.Controllers;
using L2L.WebApi.Models;
using L2L.WebApi.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace L2L.WebApi.Services
{
    [Authorize]
    public class QuizzClassAnnouncementService : BaseService, IResource
    {
        public QuizzClassAnnouncementService(BaseApiController controller)
            : base(controller)
        {
        }

        public QuizzClassAnnouncementModel GetLatestAnnouncement(int qcId)
        {
            try
            {
                var model = _uow.QuizzClassAnnouncements.GetAll()
                    .Where(qca => qca.QuizzClassId == qcId && qca.IsDeleted == false)
                    .OrderByDescending(qca => qca.PostedDate)
                    .ProjectTo<QuizzClassAnnouncementModel>()
                    .FirstOrDefault();

                return model;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        public object GetManyAlt(int id, int id2, int id3, string str, string str2, string str3)
        {
            throw new NotImplementedException();
        }

        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            try
            {
                var quizzClassId = id;
                var pageNum = id2;
                var numPerPage = id3;

                var list = _uow.QuizzClassAnnouncements.GetAll()
                    .Where(qca => qca.QuizzClassId == quizzClassId && qca.IsDeleted == false)
                    .OrderByDescending(qca => qca.PostedDate)
                    .Skip((pageNum - 1) * numPerPage)
                    .Take(numPerPage)
                    .ProjectTo<QuizzClassAnnouncementModel>(new { userId = _currentUser.Id})
                    .ToList();

                var numCount = _svcContainer.QuizzClassMemberUpdateSvc.RemoveAnnouncement(quizzClassId);

                foreach (var item in list)
                {
                    item.PostedDate = item.PostedDate.ToLocalTime();
                    item.IsNew = numCount-- > 0;
                }

                return list;
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

        public object GetAlt(string str)
        {
            throw new NotImplementedException();
        }

        public object Post(object modelParam)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<QuizzClassAnnouncementModel>(modelParam.ToString());
                if (model == null)
                    return null;

                QuizzClassAnnouncement entity;
                model.MapToNew(out entity);

                entity.PostedDate = DateTime.UtcNow;
                _uow.QuizzClassAnnouncements.Add(entity);

                _svcContainer.QuizzClassMemberUpdateSvc.AddAnnouncement(model.QuizzClassId, false);

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
                var model = JsonConvert.DeserializeObject<QuizzClassAnnouncementModel>(modelParam.ToString());
                if (model == null)
                    return false;

                model.PostedDate = model.PostedDate.ToUniversalTime();

                QuizzClassAnnouncement entity;
                model.MapToNew(out entity);

                _uow.QuizzClassAnnouncements.Update(entity);
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
                var entity = _uow.QuizzClassAnnouncements.GetById(id);
                entity.IsDeleted = true;
                _uow.QuizzClassAnnouncements.Update(entity);

                _svcContainer.QuizzClassMemberUpdateSvc.SubAnnouncement(entity.QuizzClassId, false);

                _uow.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }
    }
}