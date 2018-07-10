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
    public class TextFlashCardService : BaseService, IResource
    {
        public TextFlashCardService(BaseApiController controller)
            : base(controller)
        {
        }

        public IList<TextFlashCardModel> GetTextFlashCards(int id)
        {
            var list = _uow.TextFlashCards.GetAll()
                .Where(q => q.ReviewerId == id)
                .ProjectTo<TextFlashCardModel>()
                .ToList();

            return list;
        }

        public TextFlashCardModel GetTextFlashCardById(int id)
        {
            TextFlashCardModel model = null;
            var entity = _uow.TextFlashCards.GetById(id);
            if (entity != null)
                model = MappingUtil.Map<TextFlashCard, TextFlashCardModel>(entity);
            return model;
        }

        public bool CreateTextFlashCard(TextFlashCardModel model)
        {
            try
            {
                var entity = MappingUtil.Map<TextFlashCardModel, TextFlashCard>(model);
                _uow.TextFlashCards.Add(entity);
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

        public bool UpdateTextFlashCard(TextFlashCardModel model)
        {
            try
            {
                var entity = MappingUtil.Map<TextFlashCardModel, TextFlashCard>(model);
                _uow.TextFlashCards.Update(entity);
                _uow.SaveChanges();
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }

            return true;
        }

        public bool DeleteTextFlashCard(int id, bool callSaveChanges = true)
        {
            try
            {
                _uow.TextFlashCards.Delete(id);
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

                var list = _uow.TextFlashCards.GetAll()
                    .Where(q => q.ReviewerId == reviewerId)
                    .ProjectTo<TextFlashCardModel>(new { userId = _currentUser.Id })
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
                var model = JsonConvert.DeserializeObject<TextFlashCardModel>(modelParam.ToString());
                if (model == null)
                    return null;

                TextFlashCard entity;
                model.MapToNew(out entity);
                entity.OwnerId = _currentUser.Id;

                if (entity.AddContentType == AddContentTypeEnum.PictureOnly)
                {
                    string tmpString;
                    ImageUtil.SaveImage("Reviewer_" + model.ReviewerId.ToString(), model.NewImageFileName, "", model.ImageContent, out tmpString);
                    entity.ImageUrl = tmpString;
                }

                _uow.TextFlashCards.Add(entity);
                _uow.SaveChanges();

                model = _uow.TextFlashCards.GetAll()
                    .Where(qn => qn.Id == entity.Id)
                    .ProjectTo<TextFlashCardModel>()
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
            var model = JsonConvert.DeserializeObject<TextFlashCardModel>(modelParam.ToString());
            if (model == null)
                return false;

            TextFlashCard entity;
            model.MapToNew(out entity);

            if (entity.AddContentType == AddContentTypeEnum.PictureOnly && model.IsImageChanged)
            {
                string tmpString;
                ImageUtil.SaveImage("Reviewer_" + model.ReviewerId.ToString(), model.NewImageFileName, model.ImageUrl, model.ImageContent, out tmpString);
                entity.ImageUrl = tmpString;
            }

            _uow.TextFlashCards.Update(entity);

            _uow.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            try
            {
                _uow.TextFlashCards.Delete(id);
                _uow.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        public void UpdateModelList(IEnumerable<TextFlashCardModel> list)
        {
            foreach (var item in list)
            {
                UpdateModel(item);
            }
        }

        public void UpdateModel(TextFlashCardModel model)
        {
        }
    }
}