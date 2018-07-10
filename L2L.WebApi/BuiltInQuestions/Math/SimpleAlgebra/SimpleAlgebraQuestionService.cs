using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.Controllers;
using L2L.WebApi.Models;
using L2L.WebApi.BuiltInQuestions.Helpers;
using L2L.WebApi.BuiltInQuestions.Math.SimpleAlgebra;

namespace L2L.WebApi.BuiltInQuestions
{
    public class SimpleAlgebraQuestionService: IBuiltInQuestionGenerator
    {
        private int _numQuestions;
        private QuizzModel _quizz;

        public TakeTestModel GetTest(int numQuestions, QuizzModel quizz)
        {
            TakeTestModel test = null;
            _numQuestions = numQuestions;
            _quizz = quizz;

            switch (_quizz.MainType)
            {
                case 0:
                    test = AdditionGetTest();
                    break;
                case 1:
                    test = SubtractionGetTest();
                    break;
                case 2:
                    test = MultiplicationGetTest();
                    break;
                case 3:
                    test = DivisionGetTest();
                    break;
            }

            return test;
        }

        // subType - number of digits
        private TakeTestModel AdditionGetTest()
        {
            return null;
        }

        private TakeTestModel SubtractionGetTest()
        {
            SubtractionQuestionsCreator creator = new SubtractionQuestionsCreator(_quizz);

            return creator.CreateTest(_numQuestions);
        }

        private TakeTestModel MultiplicationGetTest()
        {
            return null;
        }

        private TakeTestModel DivisionGetTest()
        {
            return null;
        }
    }
}