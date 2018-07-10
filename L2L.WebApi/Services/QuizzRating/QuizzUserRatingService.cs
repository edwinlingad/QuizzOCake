using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.Entities;
using L2L.Entities.Enums;
using L2L.WebApi.Controllers;
using L2L.WebApi.Models;
using AutoMapper.QueryableExtensions;

namespace L2L.WebApi.Services
{
    public class QuizzUserRatingService : BaseService, IResource
    {
        public QuizzUserRatingService(BaseApiController controller)
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
                    .ProjectTo<QuizzUserRatingModel>()
                    .FirstOrDefault();

                return model;
            }
            catch (Exception)
            {
                QuizzUserRatingModel model = new QuizzUserRatingModel()
                {
                    QuizzRatingId = 0,
                    RatingAvg = 0,
                    NumRatings = 0
                };

                return model;
                //_svcContainer.LoggingSvc.Log(ex);
                //return null;
            }
        }

        public object Post(object model)
        {
            throw new NotImplementedException();
        }

        public bool Patch(object model)
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