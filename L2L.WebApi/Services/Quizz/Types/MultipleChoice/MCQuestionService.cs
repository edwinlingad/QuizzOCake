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
    public class MCQuestionService : BaseService, IQuestionType
    {
        public MCQuestionService(BaseApiController controller)
            : base(controller)
        {
        }

        public MCQuestionModel GetQuestionById(int id)
        {
            var entity = _uow.MultipleChoiceQuestions.GetAll()
                .Include(q => q.Choices)
                .Where(q => q.Id == id)
                .FirstOrDefault();

            var model = MappingUtil.Map<MultipleChoiceQuestion, MCQuestionModel>(entity);

            return model;
        }

        public QuestionModel CreateQuestion(object model)
        {
            QuestionModel questionModel = null;
            MCQuestionModel actualModel = JsonConvert.DeserializeObject<MCQuestionModel>(model.ToString());
            if (actualModel != null)
            {
                if (CreateQuestion(actualModel) == true)
                {
                    questionModel = new QuestionModel()
                    {
                        Id = actualModel.Id,
                        QuestionId = actualModel.Id,
                        QuestionType = QuestionTypeEnum.MultipleChoice,
                        Question = actualModel.Question,
                        TestId = actualModel.TestId
                    };
                }
            }

            return questionModel;
        }

        private bool CreateQuestion(MCQuestionModel model)
        {
            var entity = MappingUtil.Map<MCQuestionModel, MultipleChoiceQuestion>(model);
            entity.AuthorId = _currentUser.Id;
            entity.Question = "";

            if (entity.AddContentType == AddContentTypeEnum.PictureOnly)
            {
                string tmpString;
                ImageUtil.SaveImage("Test_" + model.TestId.ToString(), model.NewImageFileName, "", model.ImageContent, out tmpString);
                entity.ImageUrl = tmpString;
            }

            _uow.MultipleChoiceQuestions.Add(entity);
            _uow.SaveChanges();

            MappingUtil.Map(entity, model);
            model.Question = model.TextContent;

            return true;
        }

        public bool UpdateQuestion(MCQuestionModel model)
        {
            try
            {
                var entity = MappingUtil.Map<MCQuestionModel, MultipleChoiceQuestion>(model);
                entity.Choices = null;

                if (entity.AddContentType == AddContentTypeEnum.PictureOnly && model.IsImageChanged)
                {
                    string tmpString;
                    ImageUtil.SaveImage("Test_" + model.TestId.ToString(), model.NewImageFileName, "", model.ImageContent, out tmpString, true);
                    entity.ImageUrl = tmpString;
                }

                _uow.MultipleChoiceQuestions.Update(entity);
                _uow.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteQuestion(int id, bool callSaveChanges = true)
        {
            try
            {
                var entity = _uow.MultipleChoiceQuestions.GetAll()
                    .Include(q => q.Choices)
                    .Where(q => q.Id == id)
                    .FirstOrDefault();

                if (entity == null)
                    return false;

                foreach (var item in entity.Choices.ToList())
                {
                    _uow.MultipleChoiceChoices.Delete(item.Id);
                }

                _uow.MultipleChoiceQuestions.Delete(id);

                if (callSaveChanges)
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