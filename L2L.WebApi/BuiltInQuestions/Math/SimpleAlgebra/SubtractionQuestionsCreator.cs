using L2L.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using L2L.WebApi.BuiltInQuestions.Helpers;

namespace L2L.WebApi.BuiltInQuestions.Math.SimpleAlgebra
{
    public class SubtractionQuestionsCreator
    {
        private TakeTestModelCreator _takeTestModelCreator;
        private QuizzModel _quizz;
        private Random _random;
        public SubtractionQuestionsCreator(QuizzModel quizz)
        {
            _quizz = quizz;
            _takeTestModelCreator = new TakeTestModelCreator();
            _random = new Random();
        }

        // subType - 0 - any, 1 - no negative, 2 - no regrouping
        // subType2 - number of digits
        public TakeTestModel CreateTest(int numQuestions)
        {
            switch (_quizz.SubType)
            {
                case 0:
                    AddQuestions(numQuestions);
                    break;
                case 1:
                    AddNoNegativeQuestions(numQuestions);
                    break;
                case 2:
                    AddNoRegroupingQuestions(numQuestions);
                    break;
                default:
                    break;
            }

            return _takeTestModelCreator.GetTest();
        }

        private void AddNoRegroupingQuestions(int numQuestions)
        {
            StringBuilder operandStr1 = new StringBuilder();
            StringBuilder operandStr2 = new StringBuilder();
            for (int i = 0; i < numQuestions; i++)
            {
                operandStr1.Clear();
                operandStr2.Clear();

                for (int j = 0; j < _quizz.SubType2 + 1; j++)
                {
                    int number1 = _random.Next(10);
                    int number2 = _random.Next(number1);

                    if (j == 0)
                    {
                        if (number1 == 0)
                            number1++;
                        if (number2 == 0 && _quizz.SubType2 != 0)
                            number2++;
                    }

                    operandStr1.Append(number1);
                    operandStr2.Append(number2);
                }

                int operand1 = Int32.Parse(operandStr1.ToString());
                int operand2 = Int32.Parse(operandStr2.ToString());
                int answer = operand1 - operand2;

                int[] operands = { operand1, operand2 };
                string questionStr = FormattedQuestionString.GenerateQuestionString("-", operands);

                var questionCreator = new QandQuestionModelCreator(questionStr);
                questionCreator.AddAnswer(answer.ToString());

                var question = questionCreator.GetQuestion();
                _takeTestModelCreator.AddQandAQuestion(question);
            }
        }

        private void AddQuestions(int numQuestions)
        {
            StringBuilder operandStr1 = new StringBuilder();
            StringBuilder operandStr2 = new StringBuilder();
            for (int i = 0; i < numQuestions; i++)
            {
                operandStr1.Clear();
                operandStr2.Clear();

                for (int j = 0; j < _quizz.SubType2 + 1; j++)
                {
                    int number1 = _random.Next(10);
                    int number2 = _random.Next(10);

                    if (j == 0)
                    {
                        if (number1 == 0)
                            number1++;
                        if (number2 == 0 && _quizz.SubType2 != 0)
                            number2++;
                    }

                    operandStr1.Append(number1);
                    operandStr2.Append(number2);
                }

                int operand1 = Int32.Parse(operandStr1.ToString());
                int operand2 = Int32.Parse(operandStr2.ToString());
                int answer = operand1 - operand2;

                int[] operands = { operand1, operand2 };
                string questionStr = FormattedQuestionString.GenerateQuestionString("-", operands);

                var questionCreator = new QandQuestionModelCreator(questionStr);
                questionCreator.AddAnswer(answer.ToString());

                var question = questionCreator.GetQuestion();
                _takeTestModelCreator.AddQandAQuestion(question);
            }
        }

        private void AddNoNegativeQuestions(int numQuestions)
        {
            StringBuilder operandStr1 = new StringBuilder();
            StringBuilder operandStr2 = new StringBuilder();
            for (int i = 0; i < numQuestions; i++)
            {
                operandStr1.Clear();
                operandStr2.Clear();

                for (int j = 0; j < _quizz.SubType2 + 1; j++)
                {
                    int number1 = _random.Next(10);
                    int number2 = _random.Next(10);

                    if (j == 0)
                    {
                        if (number1 == 0)
                            number1++;
                        if (number2 == 0 && _quizz.SubType2 != 0)
                            number2++;

                        if(number1 < number2)
                        {
                            var tmp = number1;
                            number1 = number2;
                            number2 = tmp;
                        }
                    }

                    operandStr1.Append(number1);
                    operandStr2.Append(number2);
                }

                int operand1 = Int32.Parse(operandStr1.ToString());
                int operand2 = Int32.Parse(operandStr2.ToString());
                int answer = operand1 - operand2;

                int[] operands = { operand1, operand2 };
                string questionStr = FormattedQuestionString.GenerateQuestionString("-", operands);

                var questionCreator = new QandQuestionModelCreator(questionStr);
                questionCreator.AddAnswer(answer.ToString());

                var question = questionCreator.GetQuestion();
                _takeTestModelCreator.AddQandAQuestion(question);
            }
        }
    }
}