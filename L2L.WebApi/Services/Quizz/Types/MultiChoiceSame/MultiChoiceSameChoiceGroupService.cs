using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using L2L.WebApi.Models;
using L2L.Entities;
using L2L.WebApi.Utilities;
using L2L.WebApi.Controllers;


namespace L2L.WebApi.Services
{
    public class MultiChoiceSameChoiceGroupService: BaseService
    {
        public MultiChoiceSameChoiceGroupService(BaseApiController controller)
            :base(controller)
        {
        }

        public bool CreateChoiceGroup(MultiChoiceSameChoiceGroupModel model)
        {
            try
            {
                var entity = MappingUtil.Map<MultiChoiceSameChoiceGroupModel, MultiChoiceSameChoiceGroup>(model);
                _uow.MultiChoiceSameChoiceGroups.Add(entity);
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

        public bool UpdateChoiceGroup(MultiChoiceSameChoiceGroupModel model)
        {
            try
            {
                var entity = MappingUtil.Map<MultiChoiceSameChoiceGroupModel, MultiChoiceSameChoiceGroup>(model);
                _uow.MultiChoiceSameChoiceGroups.Update(entity);
                _uow.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        public bool DeleteChoiceGroup(int id)
        {
            try
            {
                var entity = _uow.MultiChoiceSameChoiceGroups.GetAll()
                    .Include(q => q.Choices)
                    .FirstOrDefault();

                if (entity.Choices.Count != 0)
                {
                    foreach (var item in entity.Choices)
                    {
                        _multiChoiceSameChoiceSvc.DeleteChoice(item.Id, false);
                    }
                }

                _uow.MultiChoiceSameChoiceGroups.Delete(entity.Id);
                _uow.SaveChanges();
                    
                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        private MultiChoiceSameChoiceService __multiChoiceSameChoiceSvc;
        private MultiChoiceSameChoiceService _multiChoiceSameChoiceSvc
        {
            get
            {
                if (__multiChoiceSameChoiceSvc == null)
                    __multiChoiceSameChoiceSvc = _svcContainer.MultiChoiceSameChoiceSvc;
                return __multiChoiceSameChoiceSvc;
            }
        }
    }
}