using L2L.Entities.Enums;
using L2L.WebApi.Controllers;
using L2L.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using L2L.Entities;
using L2L.WebApi.Utilities;

namespace L2L.WebApi.Services
{
    public class QuizzCategoryService : BaseService, IResource
    {
        public QuizzCategoryService(BaseApiController controller)
            : base(controller)
        {
        }

        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            try
            {
                var list = _uow.QuizzCategories.GetAll()
                    .OrderBy(c => c.Title)
                    .ProjectTo<QuizzCategoryModel>()
                    .ToList();

                foreach (var item in list)
                {
                    item.QuizzCount = _uow.Quizzes.GetAll()
                        .Where(q => q.Category == item.QuizzCategoryType && q.IsLive == true)
                        .Count();
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

        public object Post(object modelParam)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<QuizzCategoryModel>(modelParam.ToString());
                if (model == null)
                   return null;

                int highest = _uow.QuizzCategories.GetAll()
                    .OrderByDescending(c => c.CategoryValue)
                    .Select(c => c.CategoryValue)
                    .FirstOrDefault();

                QuizzCategory entity;
                model.MapToNew(out entity);
                entity.CategoryValue = highest + 1;

                _uow.QuizzCategories.Add(entity);
                _uow.SaveChanges();

                Utilities.MappingUtil.Map(entity, model);

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
                var model = JsonConvert.DeserializeObject<QuizzCategoryModel>(modelParam.ToString());
                if (model == null)
                    return true;
                QuizzCategory entity;
                model.MapToNew(out entity);

                _uow.QuizzCategories.Update(entity);
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
                _uow.QuizzCategories.Delete(id);
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