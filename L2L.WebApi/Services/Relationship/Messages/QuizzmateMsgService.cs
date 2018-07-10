using L2L.WebApi.Controllers;
using L2L.WebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.Utilities;
using L2L.Entities;
using System.Data.Entity;
using AutoMapper.QueryableExtensions;

namespace L2L.WebApi.Services
{
    public class QuizzmateMsgService : BaseService, IResource
    {
        public QuizzmateMsgService(BaseApiController controller)
            : base(controller)
        {
        }

        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            throw new NotImplementedException();
        }

        public object GetManyAlt(int id, int id2, int id3, string str, string str2, string str3)
        {
            try
            {
                var userId = id;
                var threadId = str;


                if (userId != 0)
                {

                }
                else if (string.IsNullOrEmpty(threadId) == false)
                {

                }

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

        public object GetAlt(string str)
        {
            throw new NotImplementedException();
        }

        public object Post(object modelParam)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<QuizzmateMsgModel>(modelParam.ToString());
                if (model == null)
                    return null;

                QuizzmateMsg1 entity = null;
                model.MapToNew(out entity);

                _uow.QuizzmateMsg1s.Add(entity);
                _uow.SaveChanges();

                model = MappingUtil.Map<QuizzmateMsg1, QuizzmateMsgModel>(entity);

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
            throw new NotImplementedException();
        }


    }
}