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

namespace L2L.WebApi.Services
{
    public class QuizzCurrentUserRatingService : BaseService, IResource
    {
        public QuizzCurrentUserRatingService(BaseApiController controller)
            : base(controller)
        {
        }

        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            throw new NotImplementedException();
        }

        public object Get(int quizzRatingId)
        {
            try
            {
                var model = _uow.QuizzRatings.GetAll()
                    .Where(qr => qr.Id == quizzRatingId)
                    .ProjectTo<QuizzCurrentUserRatingModel>(new { userId = _currentUser.Id })
                    .FirstOrDefault();

                return model;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        public object Post(object model)
        {
            throw new NotImplementedException();
        }

        public bool Patch(object modelParam)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<QuizzUserRatingUpdateRateModel>(modelParam.ToString());
                if (model == null)
                    return false;

                int quizzRatingId = model.QuizzRatingId;
                int value = model.Rating;
 
                if (!(value > 0 && value <= 5))
                    return false;

                var entity = _uow.QuizzUserRatings.GetAll()
                    .Where(qr => qr.UserId == _currentUser.Id && qr.QuizzRatingId == quizzRatingId)
                    .FirstOrDefault();

                if (entity == null)
                {
                    entity = new QuizzUserRating
                    {
                        QuizzRatingId = quizzRatingId,
                        Rating = value,
                        UserId = _currentUser.Id
                    };
                    _uow.QuizzUserRatings.Add(entity);
                }
                else
                {
                    entity.Rating = value;
                    _uow.QuizzUserRatings.Update(entity);
                }

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