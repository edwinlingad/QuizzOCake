using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using L2L.WebApi.Models;
using L2L.Entities;
using L2L.WebApi.Utilities;
using L2L.WebApi.Controllers;
using Newtonsoft.Json;
using L2L.Entities.Enums;

namespace L2L.WebApi.Services
{
    public class MultiChoiceSameQuestionService : BaseService, IQuestionType
    {
        public MultiChoiceSameQuestionService(BaseApiController controller)
            : base(controller)
        {
        }

        public MultiChoiceSameQuestionModel GetQuestionById(int id)
        {
            var entity = _uow.MultiChoiceSameQuestions.GetAll()
                .Include(q => q.ChoiceGroup.Choices)
                .Where(q => q.Id == id)
                .FirstOrDefault();

            var model = MappingUtil.Map<MultiChoiceSameQuestion, MultiChoiceSameQuestionModel>(entity);

            return model;
        }

        public QuestionModel CreateQuestion(object model)
        {
            QuestionModel questionModel = null;
            MultiChoiceSameQuestionModel actualModel = JsonConvert.DeserializeObject<MultiChoiceSameQuestionModel>(model.ToString());
            if (actualModel != null)
            {
                if (CreateQuestion(actualModel) == true)
                {
                    questionModel = new QuestionModel()
                    {
                        Id = actualModel.Id,
                        QuestionId = actualModel.Id,
                        QuestionType = QuestionTypeEnum.MultiChoiceSame,
                        Question = actualModel.Question,
                        TestId = actualModel.TestId
                    };
                }
            }
            return questionModel;
        }

        private bool CreateQuestion(MultiChoiceSameQuestionModel model)
        {
            try
            {
                var entity = MappingUtil.Map<MultiChoiceSameQuestionModel, MultiChoiceSameQuestion>(model);

                entity.Question = "";

                if (entity.AddContentType == AddContentTypeEnum.PictureOnly)
                {
                    string tmpString;
                    ImageUtil.SaveImage("Test_" + model.TestId.ToString(), model.NewImageFileName, "", model.ImageContent, out tmpString);
                    entity.ImageUrl = tmpString;
                }

                _uow.MultiChoiceSameQuestions.Add(entity);
                _uow.SaveChanges();

                MappingUtil.Map(entity, model);
                model.Question = model.TextContent;
                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        public bool UpdateQuestion(MultiChoiceSameQuestionModel model)
        {
            try
            {
                var entity = MappingUtil.Map<MultiChoiceSameQuestionModel, MultiChoiceSameQuestion>(model);

                if (entity.AddContentType == AddContentTypeEnum.PictureOnly && model.IsImageChanged)
                {
                    string tmpString;
                    ImageUtil.SaveImage("Test_" + model.TestId.ToString(), model.NewImageFileName, "", model.ImageContent, out tmpString, true);
                    entity.ImageUrl = tmpString;
                }

                _uow.MultiChoiceSameQuestions.Update(entity);
                _uow.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        public bool DeleteQuestion(int id, bool callSaveChanges = true)
        {
            try
            {
                _uow.MultiChoiceSameQuestions.Delete(id);
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