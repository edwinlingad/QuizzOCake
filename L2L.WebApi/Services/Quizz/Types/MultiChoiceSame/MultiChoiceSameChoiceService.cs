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
    public class MultiChoiceSameChoiceService : BaseService
    {
        public MultiChoiceSameChoiceService(BaseApiController controller)
            : base(controller)
        {
        }

        public bool CreateChoice(MultiChoiceSameChoiceModel model)
        {
            var entity = MappingUtil.Map<MultiChoiceSameChoiceModel, MultiChoiceSameChoice>(model);

            _uow.MultiChoiceSameChoices.Add(entity);
            _uow.SaveChanges();

            MappingUtil.Map(entity, model);

            return true;
        }

        public bool UpdateChoice(MultiChoiceSameChoiceModel model)
        {
            var entity = MappingUtil.Map<MultiChoiceSameChoiceModel, MultiChoiceSameChoice>(model);

            try
            {
                _uow.MultiChoiceSameChoices.Update(entity);
                _uow.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        public bool DeleteChoice(int id, bool callSaveChanges = true)
        {
            try
            {
                _uow.MultiChoiceSameChoices.Delete(id);
                if (callSaveChanges)
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