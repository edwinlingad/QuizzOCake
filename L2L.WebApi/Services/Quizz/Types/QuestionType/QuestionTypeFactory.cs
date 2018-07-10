using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.Entities.Enums;
using L2L.Entities;
using L2L.WebApi.Models;
using L2L.WebApi.Controllers;

namespace L2L.WebApi.Services
{
    public class QuestionTypeFactory: BaseService
    {
        public QuestionTypeFactory(BaseApiController controller)
            :base(controller)
        {
        }

        public IQuestionType GetQuestion(QuestionTypeEnum type)
        {
            IQuestionType qType = null;
            switch (type)
            {
                case QuestionTypeEnum.BuiltIn:
                    break;
                case QuestionTypeEnum.QandA:
                    qType = _svcContainer.QAQuestionSvc;
                    break;
                case QuestionTypeEnum.MultipleChoice:
                    qType = _svcContainer.MCQuestionSvc;
                    break;
                case QuestionTypeEnum.MultiChoiceSame:
                    qType = _svcContainer.MultiChoiceSameQuestionSvc;
                    break;
                default:
                    break;
            }

            return qType;
        }
    }
}