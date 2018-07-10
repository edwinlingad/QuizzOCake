using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.BuiltInQuestions.Helpers;
using L2L.WebApi.Enums;
using L2L.WebApi.Models;

namespace L2L.WebApi.BuiltInQuestions.Math.Clock
{
    public class HourHalfHourQuestionsCreator
    {
        private const int numExtraChoices = 3;
        private bool _isHalfHour;

        private TakeTestModelCreator _takeTestModelCreator = new TakeTestModelCreator();
        private Random _random = new Random();

        private int _hour;
        private int _lowValue;
        private int _highValue;

        private List<int> _availableHourList = new List<int>();
        private List<int> _intHourList = new List<int>();

        public TakeTestModel CreateTest(int numQuestions, bool isHalfHour = false)
        {
            _isHalfHour = isHalfHour;
            for (int i = 0; i < 12; i++)
                _availableHourList.Add(i + 1);

            for (int i = 0; i < numQuestions; i++)
                CreateQuestion();

            return _takeTestModelCreator.GetTest();
        }

        private void CreateQuestion()
        {
            _intHourList.Clear();

            if (_availableHourList.Count() != 0)
            {
                int idx = _random.Next(_availableHourList.Count());
                _hour = _availableHourList[idx];
                _availableHourList.Remove(_hour);
            }
            else
                _hour = _random.Next(12) + 1;

            _lowValue = _hour;
            _highValue = _hour;

            _intHourList.Add(_hour);

            for (int i = 0; i < numExtraChoices; i++)
            {
                int option = 0;
                int type = _random.Next(3);
                switch (type)
                {
                    case 0: // go up
                        option = GenerateLowerValue();
                        break;
                    case 1: // go down
                        option = GenerateHigherValue();
                        break;
                    case 2: // any
                        option = GenerateRandomValue();
                        break;
                }

                _intHourList.Add(option);
            }

            CreateMultiChoiceQuestion();
        }

        private int GenerateLowerValue()
        {
            int option;
            do
            {
                _lowValue--;
                if (_lowValue <= 0)
                    _lowValue = 12;

                option = _lowValue;
            } while (_intHourList.Contains(option));

            return option;
        }

        private int GenerateHigherValue()
        {
            int option;
            do
            {
                _highValue++;
                if (_highValue > 12)
                    _highValue = 1;
                option = _highValue;
            } while (_intHourList.Contains(option));

            return option;
        }

        private int GenerateRandomValue()
        {
            int option;

            do
            {
                option = _random.Next(12) + 1;
            } while (_intHourList.Contains(option));

            return option;
        }

        private void CreateMultiChoiceQuestion()
        {
            string minuteStr = _isHalfHour ? ":30" : ":00";
            MultiChoiceQuestionModelCreator questionCreator = new MultiChoiceQuestionModelCreator(_hour.ToString() + minuteStr, false, false);
            foreach (var item in _intHourList.OrderBy(h => h))
            {
                questionCreator.AddChoice(item.ToString() + minuteStr, item == _hour);
            }
            _takeTestModelCreator.AddMultiChoiceQuestion(questionCreator.GetQuestion(), QuestionViewTypeEnum.Clock);
        }

    }
}