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
    public class QuizzClassCommentService : BaseService, IResource
    {
        public QuizzClassCommentService(BaseApiController controller)
            : base(controller)
        {
        }

        public object GetManyAlt(int id, int id2, int id3, string str, string str2, string str3)
        {
            try
            {
                // 0 - get latest
                // 1 - get older with date
                // 2 - get newer with date
                var type = id;
                var quizzClassId = id2;
                var numItemsToGet = id3;
                int? depId = null;
                int depIdTmp = 0;
                if (Int32.TryParse(str, out depIdTmp))
                    depId = depIdTmp;

                List<QuizzClassCommentModel> list = null;

                switch (type)
                {
                    case 0:
                        list = _uow.QuizzClassComments.GetAll()
                            .Where(qc => qc.QuizzClassId == quizzClassId && qc.IsDeleted == false)
                            .OrderByDescending(qc => qc.PostedDate)
                            .Take(numItemsToGet)
                            .ProjectTo<QuizzClassCommentModel>(new { userId = _currentUser.Id })
                            .ToList();
                        break;
                    case 1:
                        var date = DateTimeUtil.GetTimeFromClientStr(str);
                        list = list = _uow.QuizzClassComments.GetAll()
                            .Where(qc => qc.QuizzClassId == quizzClassId && qc.IsDeleted == false && qc.PostedDate < date)
                            .OrderByDescending(qc => qc.PostedDate)
                            .Take(numItemsToGet)
                            .ProjectTo<QuizzClassCommentModel>(new { userId = _currentUser.Id })
                            .ToList();
                        break;
                    case 2:
                        break;
                    default:
                        break;
                }

                var numCount = _svcContainer.QuizzClassMemberUpdateSvc.RemoveClassDiscussion(quizzClassId, numItemsToGet, depId);

                UpdateModelList(list, numCount);

                if(type == 0)
                {
                    list.Reverse();
                }

                return list;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            try
            {
                var quizzClassId = id;
                var pageNum = id2;
                var numPerPage = id3;
                int? depId = null;
                if (id4 != 0)
                    depId = id4;

                var list = _uow.QuizzClassComments.GetAll()
                    .Where(qcc => qcc.QuizzClassId == quizzClassId && qcc.IsDeleted == false)
                    .OrderByDescending(qcc => qcc.PostedDate)
                    .Skip((pageNum - 1) * numPerPage)
                    .Take(numPerPage)
                    .ProjectTo<QuizzClassCommentModel>(new { userId = _currentUser.Id })
                    .ToList();

                var numCount = _svcContainer.QuizzClassMemberUpdateSvc.RemoveClassDiscussion(quizzClassId, numPerPage, depId);

                foreach (var item in list)
                {
                    UpdateModelList(list, numCount);
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
                var model = JsonConvert.DeserializeObject<QuizzClassCommentModel>(modelParam.ToString());
                if (model == null)
                    return null;

                QuizzClassComment entity;
                model.MapToNew(out entity);

                entity.AuthorId = _currentUser.Id;
                entity.PostedDate = DateTime.UtcNow;

                _uow.QuizzClassComments.Add(entity);

                _svcContainer.QuizzClassMemberUpdateSvc.AddClassDiscussion(model.QuizzClassId, false);
                _uow.SaveChanges();

                model = _uow.QuizzClassComments.GetAll()
                    .Where(qcc => qcc.Id == entity.Id)
                    .ProjectTo<QuizzClassCommentModel>(new { userId = _currentUser.Id })
                    .FirstOrDefault();

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
            try
            {
                var model = JsonConvert.DeserializeObject<QuizzClassCommentModel>(modelParam.ToString());
                if (model == null)
                    return false;

                model.PostedDate = model.PostedDate.ToUniversalTime();

                QuizzClassComment entity;
                model.MapToNew(out entity);

                _uow.QuizzClassComments.Update(entity);
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
                var entity = _uow.QuizzClassComments.GetById(id);
                entity.IsDeleted = true;
                _uow.QuizzClassComments.Update(entity);

                _svcContainer.QuizzClassMemberUpdateSvc.SubClassDiscussion(entity.QuizzClassId, false);

                _uow.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        public void UpdateModelList(IEnumerable<QuizzClassCommentModel> list, int numNewCount)
        {
            foreach (var item in list)
            {
                UpdateModel(item);
                item.IsNew = numNewCount-- > 0;
            }
        }

        public void UpdateModel(QuizzClassCommentModel model)
        {
            SetAge(model);

            if (model.AuthorId == _currentUser.Id)
            {
                model.IsAuthor = true;
                model.IsQuizzmate = true;
            }
            model.AuthorName = model.IsQuizzmate ? model.AuthorFullName : model.AuthorUserName;
            model.PostedDate = model.PostedDate.ToLocalTime();
        }
    }
}