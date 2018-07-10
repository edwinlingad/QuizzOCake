using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.BuiltInQuestions.Helpers;
using L2L.WebApi.Enums;
using L2L.WebApi.Models;

namespace L2L.WebApi.BuiltInQuestions.Math.Clock
{
    public class MinuteIncQuestionsGenerator
    {
        private const int numExtraChoices = 3;

        private TakeTestModelCreator _takeTestModelCreator;
        private Random _random = new Random();

        private int _hour;
        private int _minute;

        private TimeSpan _bigIncValue;
        private DateTime _lowBigIncDate;
        private DateTime _highBigIncDate;

        private TimeSpan _smallIncValue;
        private DateTime _lowSmallIncDate;
        private DateTime _highSmallIncDate;

        private int _minuteInc;
        private int _numValuesPerHour;

        private List<int> _timeTaken = new List<int>();
        private List<int> _intTimeList = new List<int>();

        public TakeTestModel CreateQandATest(int numQuestions, int minuteInc)
        {
            _takeTestModelCreator = new TakeTestModelCreator();

            _minuteInc = minuteInc;
            _numValuesPerHour = (60 / _minuteInc) - 1;

            for (int i = 0; i < numQuestions; i++)
            {
                CreateQandAQuestion();
            }

            return _takeTestModelCreator.GetTest();
        }

        private void CreateQandAQuestion()
        {
            do
            {
                _hour = _random.Next(12) + 1;
                _minute = (_random.Next(_numValuesPerHour) + 1) * _minuteInc;
            } while (_intTimeList.Contains((_hour * 100) + _minute));

            QandQuestionModelCreator qaQuestion = new QandQuestionModelCreator(_hour.ToString() + ":" + _minute.ToString().PadLeft(2, '0'));
            qaQuestion.AddAnswer(_hour.ToString() + ":" + _minute.ToString().PadLeft(2, '0'));

            _takeTestModelCreator.AddQandAQuestion(qaQuestion.GetQuestion(), QuestionViewTypeEnum.Clock);
        }

        public TakeTestModel CreateMultipleChoiceTest(int numQuestions, int minuteInc)
        {
            _takeTestModelCreator = new TakeTestModelCreator();

            _minuteInc = minuteInc;
            _numValuesPerHour = (60 / _minuteInc) - 1;

            _smallIncValue = new TimeSpan(0, _minuteInc, 0);

            switch (minuteInc)
            {
                case 15:
                    _bigIncValue = new TimeSpan(0, 60, 0);
                    break;
                case 5:
                    _bigIncValue = new TimeSpan(0, 30, 0);
                    break;
                case 1:
                    _bigIncValue = new TimeSpan(0, 5, 0);
                    break;
                default:
                    break;
            }

            for (int i = 0; i < numQuestions; i++)
            {
                CreateMultipleChoiceQuestion();
            }

            return _takeTestModelCreator.GetTest();
        }

        private void CreateMultipleChoiceQuestion()
        {
            _intTimeList.Clear();

            _hour = _random.Next(12) + 1;
            _minute = (_random.Next(_numValuesPerHour) + 1) * _minuteInc;

            _lowSmallIncDate = new DateTime(2000, 1, 1, _hour, _minute, 0);
            _highSmallIncDate = new DateTime(2000, 1, 1, _hour, _minute, 0);

            _lowBigIncDate = new DateTime(2000, 1, 1, _hour, _minute, 0);
            _highBigIncDate = new DateTime(2000, 1, 1, _hour, _minute, 0);

            _intTimeList.Add(_hour * 100 + _minute);

            switch (_random.Next(3))
            {
                case 0:
                    CreateAllByBigIncChoices();
                    break;
                case 1:
                    CreateAllByMinuteIncChoices();
                    break;
                case 2:
                    CreateAllRandomChoices();
                    break;
            }

            CreateMultiChoiceQuestion();
        }

        private void CreateAllByBigIncChoices()
        {
            for (int i = 0; i < numExtraChoices; i++)
            {
                CreateByBigIncChoice();
            }
        }

        private void CreateByBigIncChoice()
        {
            int newTime = 0;
            switch (_random.Next(3))
            {
                case 0: // go up
                    newTime = GenerateLowBigInc();
                    break;
                case 1: // go down
                    newTime = GenerateHighBigInc();
                    break;
                case 2: // random
                    newTime = GenerateRandom();
                    break;
                default:
                    break;
            }

            _intTimeList.Add(newTime);
        }

        private int GenerateLowBigInc()
        {
            int newTime;

            do
            {
                _lowBigIncDate = _lowBigIncDate.Subtract(_bigIncValue);
                newTime = _lowBigIncDate.Hour * 100 + _lowBigIncDate.Minute;
                newTime = ConvertToNormalTime(newTime);

            } while (_intTimeList.Contains(newTime));

            return newTime;
        }

        private int ConvertToNormalTime(int time)
        {
            int retTime = time;
            if (time < 100)
                retTime += 1200;
            else if (time >= 1300)
                retTime -= 1200;

            return retTime;
        }

        private int GenerateHighBigInc()
        {
            int newTime;
            do
            {
                _highBigIncDate = _highBigIncDate.Add(_bigIncValue);
                newTime = _highBigIncDate.Hour * 100 + _highBigIncDate.Minute;
                newTime = ConvertToNormalTime(newTime);

            } while (_intTimeList.Contains(newTime));

            return newTime;
        }

        private int GenerateRandom()
        {
            int newHour;
            int newMinute;

            do
            {
                newHour = _random.Next(12) + 1;
                newMinute = (_random.Next(_numValuesPerHour) + 1) * _minuteInc;
            } while (_intTimeList.Contains(newHour * 100 + _minute));

            return newHour * 100 + newMinute;
        }

        private void CreateAllByMinuteIncChoices()
        {
            for (int i = 0; i < numExtraChoices; i++)
            {
                CreateByMinuteIncChoice();
            }
        }

        private void CreateByMinuteIncChoice()
        {
            int newTime = 0;

            switch (_random.Next(3))
            {
                case 0:
                    newTime = GenerateLowMinute();
                    break;
                case 1:
                    newTime = GenerateHighMinute();
                    break;
                case 2:
                    newTime = GenerateRandom();
                    break;
                default:
                    break;
            }

            _intTimeList.Add(newTime);
        }

        private int GenerateLowMinute()
        {
            int newTime;
            do
            {
                _lowSmallIncDate = _lowSmallIncDate.Subtract(_smallIncValue);
                newTime = _lowSmallIncDate.Hour * 100 + _lowSmallIncDate.Minute;
                newTime = ConvertToNormalTime(newTime);

            } while (_intTimeList.Contains(newTime));

            return newTime;
        }

        private int GenerateHighMinute()
        {
            int newTime;
            do
            {
                _highSmallIncDate = _highSmallIncDate.Add(_smallIncValue);
                newTime = _highSmallIncDate.Hour * 100 + _highSmallIncDate.Minute;
                newTime = ConvertToNormalTime(newTime);

            } while (_intTimeList.Contains(newTime));

            return newTime;
        }

        private void CreateAllRandomChoices()
        {
            for (int i = 0; i < numExtraChoices; i++)
            {
                switch (_random.Next(2))
                {
                    case 0:
                        CreateByBigIncChoice();
                        break;
                    case 1:
                        CreateByMinuteIncChoice();
                        break;
                    default:
                        break;
                }
            }
        }

        private void CreateMultiChoiceQuestion()
        {
            MultiChoiceQuestionModelCreator questionCreator = new MultiChoiceQuestionModelCreator(_hour.ToString() + ":" + _minute.ToString().PadLeft(2, '0'), false, false);
            foreach (var item in _intTimeList.OrderBy(h => h))
            {
                int hour = item / 100;
                int minute = item % 100;
                questionCreator.AddChoice(hour.ToString() + ":" + minute.ToString().PadLeft(2, '0'), item == _hour * 100 + _minute);
            }

            _takeTestModelCreator.AddMultiChoiceQuestion(questionCreator.GetQuestion(), QuestionViewTypeEnum.Clock);
        }

    }
}