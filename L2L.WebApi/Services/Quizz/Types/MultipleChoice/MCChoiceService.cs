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
    public class MCChoiceService : BaseService
    {
        public MCChoiceService(BaseApiController controller)
            : base(controller)
        {
        }

        // Get Not needed: MCChoice will already get them thru DB Call

        public bool CreateChoice(MChoiceModel model)
        {
            try
            {
                var entity = MappingUtil.Map<MChoiceModel, MultipleChoiceChoice>(model);

                _uow.MultipleChoiceChoices.Add(entity);
                _uow.SaveChanges();

                MappingUtil.Map(entity, model);
                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }


        }

        public bool UpdateChoice(MChoiceModel model)
        {
            try
            {
                var entity = MappingUtil.Map<MChoiceModel, MultipleChoiceChoice>(model);
                _uow.MultipleChoiceChoices.Update(entity);
                _uow.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        public bool DeleteChoice(int id)
        {
            try
            {
                _uow.MultipleChoiceChoices.Delete(id);
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