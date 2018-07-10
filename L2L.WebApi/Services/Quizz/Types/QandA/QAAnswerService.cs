using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.Models;
using L2L.Entities;
using L2L.WebApi.Utilities;
using L2L.WebApi.Controllers;

namespace L2L.WebApi.Services
{
    public class QAAnswerService : BaseService
    {
        public QAAnswerService(BaseApiController controller)
            : base(controller)
        {
        }

        // Get Not needed: QAQuestion will already get them thru DB Call

        public bool CreateAnswer(QAAnswerModel model)
        {
            var entity = MappingUtil.Map<QAAnswerModel, QandAAnswer>(model);
            
            _uow.QandAAnswers.Add(entity);
            _uow.SaveChanges();

            MappingUtil.Map(entity, model);

            return true;
        }

        public bool UpdateAnswer(QAAnswerModel model)
        {
            var entity = MappingUtil.Map<QAAnswerModel, QandAAnswer>(model);

            try
            {
                _uow.QandAAnswers.Update(entity);
                _uow.SaveChanges();
            } 
            catch(Exception)
            {
                return false;
            }

            return true;
        }

        public bool DeleteAnswer(int id)
        {
            try
            {
                _uow.QandAAnswers.Delete(id);
                _uow.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}