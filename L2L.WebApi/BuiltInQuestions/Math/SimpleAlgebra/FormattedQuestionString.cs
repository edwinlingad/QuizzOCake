using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace L2L.WebApi.BuiltInQuestions.Math.SimpleAlgebra
{
    public static class FormattedQuestionString
    {
        public static string GenerateQuestionString(string op, int[] operands)
        {
            StringBuilder question = new StringBuilder();

            question.Append("<div class='simple-algebra' style=\"margin: 0 auto;\">");

            question.Append("<div class='operand'>");
            foreach (var item in operands)
            {
                question.Append("<div>" + item.ToString() + "</div>");
            }
            question.Append("</div>");

            question.Append("<div class='operator'>" + op + "</div>");

            question.Append("<div class='result'>? ? ?</div>");

            question.Append("<div>");

            return question.ToString();
        }
    }
}