using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.Controllers;
using L2L.WebApi.BuiltInQuestions.Helpers;
using L2L.WebApi.Models;
using L2L.WebApi.BuiltInQuestions.Math.Clock;

namespace L2L.WebApi.BuiltInQuestions
{
    public class ClockQuestionService : IBuiltInQuestionGenerator
    {
        private int _numQuestions;
        private QuizzModel _quizz;
        private const int numOptions = 5;

        public TakeTestModel GetTest(int numQuestions, QuizzModel quizz)
        {
            TakeTestModel test = null;
            _numQuestions = numQuestions;
            _quizz = quizz;

            switch (_quizz.MainType)
            {
                case 0:
                    test = HourGetTest();
                    break;
                case 1:
                    test = HalfHourGetTest();
                    break;
                case 2:
                    test = QuarterHourGetTest();
                    break;
                case 3:
                    test = FiveMinutesGetTest();
                    break;
                case 4:
                    test = MinutesGetTest();
                    break;
            }

            return test;
        }

        private TakeTestModel HourGetTest()
        {
            HourHalfHourQuestionsCreator creator = new HourHalfHourQuestionsCreator();
            return creator.CreateTest(_numQuestions);
        }

        private TakeTestModel HalfHourGetTest()
        {
            HourHalfHourQuestionsCreator creator = new HourHalfHourQuestionsCreator();
            return creator.CreateTest(_numQuestions, true);
        }

        private TakeTestModel QuarterHourGetTest()
        {
            MinuteIncQuestionsGenerator creator = new MinuteIncQuestionsGenerator();
            TakeTestModel test = null;
            switch (_quizz.SubType)
            {
                case 0:
                    test = creator.CreateMultipleChoiceTest(_numQuestions, 15);
                    break;
                case 1:
                    test = creator.CreateQandATest(_numQuestions, 15);
                    break;
                default:
                    break;
            }

            return test;
        }

        private TakeTestModel FiveMinutesGetTest()
        {
            MinuteIncQuestionsGenerator creator = new MinuteIncQuestionsGenerator();

            TakeTestModel test = null;
            switch (_quizz.SubType)
            {
                case 0:
                    test = creator.CreateMultipleChoiceTest(_numQuestions, 5);
                    break;
                case 1:
                    test = creator.CreateQandATest(_numQuestions, 5);
                    break;
                default:
                    break;
            }

            return test;
        }

        private TakeTestModel MinutesGetTest()
        {
            MinuteIncQuestionsGenerator creator = new MinuteIncQuestionsGenerator();

            TakeTestModel test = null;
            switch (_quizz.SubType)
            {
                case 0:
                    test = creator.CreateMultipleChoiceTest(_numQuestions, 1);
                    break;
                case 1:
                    test = creator.CreateQandATest(_numQuestions, 1);
                    break;
                default:
                    break;
            }

            return test;
        }
    }
}