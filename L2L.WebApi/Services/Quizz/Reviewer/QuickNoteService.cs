using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.Controllers;
using System.Data.Entity;
using L2L.WebApi.Models;
using L2L.Entities;
using L2L.WebApi.Utilities;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;

namespace L2L.WebApi.Services
{
    public class QuickNoteService : BaseService, IResource
    {
        public QuickNoteService(BaseApiController controller)
            : base(controller)
        {
        }

        public IList<QuickNoteSimpleModel> GetQuickNotes(int id)
        {
            var list = _uow.QuickNotes.GetAll()
                .Where(q => q.ReviewerId == id)
                .ProjectTo<QuickNoteSimpleModel>()
                .ToList();

            return list;
        }

        public QuickNoteModel GetQuickNoteById(int id)
        {
            QuickNoteModel model = null;
            var entity = _uow.QuickNotes.GetById(id);

            if (entity != null)
                model = MappingUtil.Map<QuickNote, QuickNoteModel>(entity);

            return model;
        }

        public bool CreateQuickNote(QuickNoteModel model)
        {
            try
            {
                var entity = MappingUtil.Map<QuickNoteModel, QuickNote>(model);
                _uow.QuickNotes.Add(entity);
                _uow.SaveChanges();

                model.Id = entity.Id;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
            return true;
        }

        public bool UpdateQuickNote(QuickNoteModel model)
        {
            try
            {
                var entity = MappingUtil.Map<QuickNoteModel, QuickNote>(model);
                _uow.QuickNotes.Update(entity);
                _uow.SaveChanges();
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }

            return true;
        }

        public bool DeleteQuickNote(int id, bool callSaveChanges = true)
        {
            try
            {
                _uow.QuickNotes.Delete(id);
                if (callSaveChanges)
                    _uow.SaveChanges();
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
            return true;
        }

        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            throw new NotImplementedException();
        }

        public object GetManyAlt(int id, int id2, int id3, string str, string str2, string str3)
        {
            try
            {
                var reviewerId = id;

                var list = _uow.QuickNotes.GetAll()
                    .Where(q => q.ReviewerId == reviewerId)
                    .ProjectTo<QuickNoteModel>(new { userId = _currentUser.Id })
                    .ToList();

                UpdateModelList(list);

                return list;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        public object GetAlt(string str)
        {
            throw new NotImplementedException();
        }

        public object Get(int id)
        {
            throw new NotImplementedException();
        }

        public object Post(object modelParam)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<QuickNoteModel>(modelParam.ToString());
                if (model == null)
                    return null;

                QuickNote entity;
                model.MapToNew(out entity);
                entity.OwnerId = _currentUser.Id;
                entity.Notes = "";

                if (entity.AddContentType == AddContentTypeEnum.PictureOnly)
                {
                    string tmpString;
                    ImageUtil.SaveImage("Reviewer_" + model.ReviewerId.ToString(), model.NewImageFileName, "", model.ImageContent, out tmpString);
                    entity.ImageUrl = tmpString;
                }

                _uow.QuickNotes.Add(entity);
                _uow.SaveChanges();

                model = _uow.QuickNotes.GetAll()
                    .Where(qn => qn.Id == entity.Id)
                    .ProjectTo<QuickNoteModel>()
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
                var model = JsonConvert.DeserializeObject<QuickNoteModel>(modelParam.ToString());
                if (model == null)
                    return false;

                QuickNote entity;
                model.MapToNew(out entity);

                if (entity.AddContentType == AddContentTypeEnum.PictureOnly && model.IsImageChanged)
                {
                    string tmpString;
                    ImageUtil.SaveImage("Reviewer_" + model.ReviewerId.ToString(), model.NewImageFileName, model.ImageUrl, model.ImageContent, out tmpString);
                    entity.ImageUrl = tmpString;
                }

                _uow.QuickNotes.Update(entity);

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
                _uow.QuickNotes.Delete(id);
                _uow.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        public void UpdateModelList(IEnumerable<QuickNoteModel> list)
        {
            foreach (var item in list)
            {
                UpdateModel(item);
            }
        }

        public void UpdateModel(QuickNoteModel model)
        {
            //var hasChanges = false;
            //if(model.Notes != null && model.Notes.Length != 0)
            //{
            //    model.TextContent = model.Notes;

            //    var entity = _uow.QuickNotes.GetById(model.Id);
            //    entity.TextContent = entity.Notes;
            //    entity.Notes = "";
            //    _uow.QuickNotes.Update(entity);

            //    hasChanges = true;
            //}

            //if (hasChanges)
            //    _uow.SaveChanges();
        }
    }
}