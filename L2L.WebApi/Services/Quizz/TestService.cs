using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using L2L.WebApi.Models;
using L2L.Entities;
using L2L.WebApi.Utilities;
using L2L.Entities.Enums;
using L2L.WebApi.Controllers;

namespace L2L.WebApi.Services
{
    public class TestService : BaseService, IResource
    {
        public TestService(BaseApiController controller) :
            base(controller)
        {
        }

        public int GetNumberOfQuestions(int testId)
        {
            int numQuestions = _uow.Questions.GetAll()
                .Where(q => q.TestId == testId)
                .Count();

            return numQuestions;
        }

        public TestModel GetTest(int id)
        {
            var entity = _questionTypeSvc.GetTestEntity(id);

            if (entity == null)
                return null;

            var model = MappingUtil.Map<Test, TestModel>(entity);

            foreach (var item in model.Questions)
            {
                item.Question
                    = _questionTypeSvc.GetQuestionString(item.QuestionType, item.QuestionId, entity);
           }

            return model;
        }

        public TakeTestModel GetTakeTestModel(int id)
        {
            var entity = _questionTypeSvc.GetTestEntityWithAnswers(id);

            if (entity == null)
                return null;

            var model = MappingUtil.Map<Test, TakeTestModel>(entity);
            foreach (var item in model.Questions)
                item.QuestionViewType = Enums.QuestionViewTypeEnum.Tinymce;

            return model;
        }

        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            throw new NotImplementedException();
        }

        public object GetManyAlt(int id, int id2, int id3, string str, string str2, string str3)
        {
            throw new NotImplementedException();
        }

        public object Get(int id)
        {
            try
            {
                //var count = _uow.quizz
                return null;
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

        public object Post(object modelParam)
        {
            throw new NotImplementedException();
        }

        public bool Patch(object modelParam)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        private QuestionTypeService __questionTypeSvc;
        private QuestionTypeService _questionTypeSvc
        {
            get
            {
                if (__questionTypeSvc == null)
                    __questionTypeSvc = _svcContainer.QuestionTypeSvc;
                return __questionTypeSvc;
            }
        }
    }
}