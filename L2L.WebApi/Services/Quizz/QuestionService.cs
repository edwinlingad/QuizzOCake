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
    public class QuestionService : BaseService
    {
        public QuestionService(BaseApiController controller)
            : base(controller)
        {
        }

        public bool UpdateQuestions(IEnumerable<QuestionModel> models)
        {
            try
            {
                foreach (var item in models)
                {
                    var entity = MappingUtil.Map<QuestionModel, Question>(item);
                    _uow.Questions.Update(entity);
                }

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
                var entity = _uow.Questions.GetById(id);

                if (entity == null &&
                    _questionTypeSvc.DeleteQuestion(entity.QuestionType, entity.QuestionId, false) == false)
                {
                    return false;
                }

                _uow.Questions.Delete(id);

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

        public QuestionModel CreateQuestion(QuestionTypeEnum type, object model)
        {
            try
            {
                var question = _questionTypeSvc.CreateQuestion(type, model);
                if (question != null)
                {
                    question.IsFlashCard = true;
                    var entity = MappingUtil.Map<QuestionModel, Question>(question);
                    entity.AuthorId = _currentUser.Id;
                    _uow.Questions.Add(entity);
                    _uow.SaveChanges();

                    question.Id = entity.Id;
                }
                return question;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        public bool IncludeInFlashCard(int id)
        {
            try
            {
                return UpdateInFlashCard(id, true);
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        public bool ExcludeInFlashCard(int id)
        {
            try
            {
                return UpdateInFlashCard(id, false);
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        public bool UpdateInFlashCard(int id, bool isInFlashCard)
        {
            var entity = _uow.Questions.GetById(id);
            entity.IsFlashCard = isInFlashCard;
            _uow.Questions.Update(entity);
            _uow.SaveChanges();

            return true;
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

        private QAQuestionService __qaQuestionSvc;
        private QAQuestionService _qaQuestionSvc
        {
            get
            {
                if (__qaQuestionSvc == null)
                    __qaQuestionSvc = _svcContainer.QAQuestionSvc;
                return __qaQuestionSvc;
            }
        }

        private MCQuestionService __mcQuestionSvc;
        private MCQuestionService _mcQuestionSvc
        {
            get
            {
                if (__mcQuestionSvc == null)
                    __mcQuestionSvc = _svcContainer.MCQuestionSvc;
                return __mcQuestionSvc;
            }
        }
    }
}