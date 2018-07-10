using L2L.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.Models;
using L2L.Entities;
using L2L.WebApi.Utilities;
using L2L.Entities.Enums;

namespace L2L.WebApi.Services
{
    public class TestSettingService : BaseService
    {
        public TestSettingService(BaseApiController controller)
            : base(controller)
        {
        }

        public bool CreateTestSetting(TestSettingModel model)
        {
            var entity = MappingUtil.Map<TestSettingModel, TestSetting>(model);

            _uow.TestSettings.Add(entity);
            _uow.SaveChanges();

            MappingUtil.Map(entity, model);

            return true;
        }

        public TestSettingModel GetTestSetting(int id)
        {
            TestSettingModel model = null;
            var entity = _uow.TestSettings.GetById(id);
            if (entity != null)
                model = MappingUtil.Map<TestSetting, TestSettingModel>(entity);

            return model;
        }

        public bool UpdateTestSetting(TestSettingModel model)
        {
            var entity = MappingUtil.Map<TestSettingModel, TestSetting>(model);
            try {
                _uow.TestSettings.Update(entity);
                _uow.SaveChanges();
            }
            catch (Exception ex)
            {
                _loggingSvc.Log(ex);
                return false;
            }
            
            return true;
        }

        public bool DeleteTestSetting(int id)
        {
            try
            {
                _uow.TestSettings.Delete(id);
                _uow.SaveChanges();
            }
            catch (Exception ex)
            {
                _loggingSvc.Log(ex);
                return false;
            }
            return true;
        }

        private LoggingService __loggingSvc;
        private LoggingService _loggingSvc
        {
            get
            {
                if (__loggingSvc == null)
                    __loggingSvc = _svcContainer.LoggingSvc;
                return __loggingSvc;
            }
        }
    }
}