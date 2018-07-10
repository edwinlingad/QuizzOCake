using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Enums
{

    public enum QuizTakenTypeEnum
    {
        NotTimed,
        Timed,
        TimedPerQuestion
    }

    public enum QuizTypeEnum
    {
        BuiltIn,
        QandA,
        MultipleChoice
    }

    enum QuizDifficultyEnum
    {
        Easy,
        Medium,
        Hard,
        VeryHard
    }
}