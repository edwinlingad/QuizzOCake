using L2L.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.BuiltInQuestions.Helpers
{
    public class MultiChoiceSameQuestionModelCreator
    {
        private MultiChoiceSameQuestionModel _question;
        public MultiChoiceSameQuestionModelCreator(string questionStr, int cgId, bool isMultiPoints = true)
        {
            _question = new MultiChoiceSameQuestionModel
            {
                //Question = questionStr,
                TextContent = questionStr,
                IsMultiplePoints = isMultiPoints,
                ChoiceGroupId = cgId
            };
        }

        public MultiChoiceSameQuestionModel GetQuestion()
        {
            return _question;
        }

        public void AddAnswers(params int[] answers)
        {
            string strAnswers = "";
            foreach (var item in answers)
            {
                strAnswers += item.ToString() + ",";
            }

            _question.Answers = strAnswers;
        }
    }
}