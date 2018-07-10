using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.Models;

namespace L2L.WebApi.BuiltInQuestions.Helpers
{
    public class QandQuestionModelCreator
    {
        private QAQuestionModel _question;
        public QandQuestionModelCreator(string questionStr, bool answersInOrder = false, bool isMultiPoints = true)
        {
            _question = new QAQuestionModel()
            {
                TextContent = questionStr,
                //Question = questionStr,
                AnswersInOrder = answersInOrder,
                IsMultiplePoints = isMultiPoints
            };
        }

        public QAQuestionModel GetQuestion()
        {
            return _question;
        }

        public void AddAnswer(string answerStr)
        {
            var answer = new QAAnswerModel
            {
                Answer = answerStr
            };
            _question.Answers.Add(answer);
        }
    }
}