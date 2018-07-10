using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.Models;
using L2L.WebApi.Services;
using L2L.Entities.Enums;
using L2L.WebApi.BuiltInQuestions;

namespace L2L.WebApi.Controllers
{
    public interface IBuiltInQuestionGenerator
    {
        TakeTestModel GetTest(int numQuestions, QuizzModel quizz);
    }

    public static class BuiltInTypeFactory
    {
        public static IBuiltInQuestionGenerator GetBuiltIn(BuiltInTypeEnum type)
        {
            IBuiltInQuestionGenerator questionGenerator = null;

            switch (type)
            {
                case BuiltInTypeEnum.MathSimpleAlgebra:
                    questionGenerator = new SimpleAlgebraQuestionService();
                    break;
                case BuiltInTypeEnum.Clock:
                    questionGenerator = new ClockQuestionService();
                    break;
                default:
                    break;
            }

            return questionGenerator;
        }
    }
}