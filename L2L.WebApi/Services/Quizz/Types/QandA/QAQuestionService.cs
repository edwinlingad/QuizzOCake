using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.Controllers;
using System.Data.Entity;
using L2L.WebApi.Models;
using L2L.Entities;
using L2L.WebApi.Utilities;
using Newtonsoft.Json;
using L2L.Entities.Enums;

namespace L2L.WebApi.Services
{
    public class QAQuestionService : BaseService, IQuestionType
    {
        public QAQuestionService(BaseApiController controller)
            : base(controller)
        {
        }

        public QAQuestionModel GetQuestionById(int id)
        {
            QAQuestionModel model = null;
            var entity = _uow.QandAQuestions.GetAll()
                .Include(q => q.Answers)
                .Where(q => q.Id == id)
                .FirstOrDefault();

            if (entity != null)
                model = MappingUtil.Map<QandAQuestion, QAQuestionModel>(entity);

            UpdateModel(model);

            return model;
        }

        public QuestionModel CreateQuestion(object model)
        {
            QuestionModel questionModel = null;
            QAQuestionModel actualModel = JsonConvert.DeserializeObject<QAQuestionModel>(model.ToString());
            if (actualModel != null)
            {
                if (CreateQuestion(actualModel) == true)
                {
                    questionModel = new QuestionModel()
                    {
                        Id = actualModel.Id,
                        QuestionId = actualModel.Id,
                        QuestionType = QuestionTypeEnum.QandA,
                        Question = actualModel.Question,
                        TestId = actualModel.TestId
                    };
                }
            }

            return questionModel;
        }

        private bool CreateQuestion(QAQuestionModel model)
        {
            try
            {
                var entity = MappingUtil.Map<QAQuestionModel, QandAQuestion>(model);
                entity.AuthorId = _currentUser.Id;
                entity.Question = "";

                if (entity.AddContentType == AddContentTypeEnum.PictureOnly)
                {
                    string tmpString;
                    ImageUtil.SaveImage("Test_" + model.TestId.ToString(), model.NewImageFileName, "", model.ImageContent, out tmpString);
                    entity.ImageUrl = tmpString;
                }

                _uow.QandAQuestions.Add(entity);
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

        public bool UpdateQuestion(QAQuestionModel model)
        {
            try
            {
                var entity = MappingUtil.Map<QAQuestionModel, QandAQuestion>(model);
                entity.Answers = null;

                if (entity.AddContentType == AddContentTypeEnum.PictureOnly && model.IsImageChanged)
                {
                    string tmpString;
                    ImageUtil.SaveImage("Test_" + model.TestId.ToString(), model.NewImageFileName, "", model.ImageContent, out tmpString, true);
                    entity.ImageUrl = tmpString;
                }

                _uow.QandAQuestions.Update(entity);
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
                var entity = _uow.QandAQuestions.GetAll()
                    .Include(q => q.Answers)
                    .Where(q => q.Id == id)
                    .FirstOrDefault();

                if (entity == null)
                    return false;

                foreach (var item in entity.Answers.ToList())
                {
                    _uow.QandAAnswers.Delete(item.Id);
                }

                _uow.QandAQuestions.Delete(id);

                if (callSaveChanges)
                    _uow.SaveChanges();
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
            return true;
        }

        public void UpdateModel(QAQuestionModel model)
        {
            if (model.Question != null && model.Question.Length != 0)
            {
                model.TextContent = model.Question;
                model.Question = "";

                var entity = _uow.QandAQuestions.GetById(model.Id);
                entity.TextContent = model.TextContent;
                entity.Question = "";
                _uow.QandAQuestions.Update(entity);
                _uow.SaveChanges();
            }
        }
    }
}