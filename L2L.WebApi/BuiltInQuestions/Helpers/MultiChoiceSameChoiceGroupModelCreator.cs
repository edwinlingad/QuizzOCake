using L2L.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.BuiltInQuestions.Helpers
{
    public class MultiChoiceSameChoiceGroupModelCreator
    {
        MultiChoiceSameChoiceGroupModel _choiceGroup;
        int _idx = 0;
        public MultiChoiceSameChoiceGroupModelCreator(int id, string name, bool shuffleChoices = true, bool isMultiPoints = true)
        {
            _choiceGroup = new MultiChoiceSameChoiceGroupModel
            {
                Id = id, 
                Name = name,
                ShuffleChoices = shuffleChoices,
                IsMultiplePoints = isMultiPoints
            };
        }

        public MultiChoiceSameChoiceGroupModel GetChoiceGroup()
        {
            return _choiceGroup;
        }

        public void AddChoice(string valueStr)
        {
            var choice = new MultiChoiceSameChoiceModel
            {
                Id = _idx,
                Value = valueStr
            };
            _idx++;
        }
    }
}