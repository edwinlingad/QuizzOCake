using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.Models;

namespace L2L.WebApi.BuiltInQuestions.Helpers
{
    public class MultiChoiceQuestionModelCreator
    {
        MCQuestionModel _question;
        public MultiChoiceQuestionModelCreator(string questionStr, bool isMultiplePoints = true, bool shuffleChoices = true)
        {
            _question = new MCQuestionModel
            {
                //Question = questionStr,
                TextContent = questionStr,
                IsMultiplePoints = isMultiplePoints,
                ShuffleChoices = shuffleChoices
            };
        }

        public MCQuestionModel GetQuestion()
        {
            return _question;
        }

        public void AddChoice(string choiceStr, bool isAnswer)
        {
            var choice = new MChoiceModel
            {
                Value = choiceStr,
                IsAnswer = isAnswer
            };

            _question.Choices.Add(choice);
        }
    }
}