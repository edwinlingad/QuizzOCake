using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities.Enums
{
    public enum UserTypeEnum
    {
        Standard = 0,
        Child
    }

    public enum QuizzVisibilityEnum
    {
        Public = 0, 
        PublicGroup, 
        Friends, 
        UserGroup, 
        Self
    }

    // add-category-tag
    public enum QuizzCategoryEnum
    {
        Math,      
        Science,
        English,
        Filipino,
        AralingPanlipunan,
        Mandarin,
        ComputerEducation
    }

    public enum QuizzDifficultyEnum
    {
        Unassigned = 0,
        Easy,
        Medium,
        Hard,
        VeryHard
    }

    public enum QuizzGradeLevelEnum
    {
        Unassigned = 0,
        PreK,
        K,
        Grade1,     // 3
        Grade2,
        Grade3, 
        Grade4,
        Grade5,
        Grade6,     // 9
        Grade7,
        Grade8,
        Grade9,     // 12
        Grade10,
        Grade11,
        Grade12,
        College,
        Professional,
        // Add here
        MaxGradeLevel
    }

    public enum QuizTakenTypeEnum
    {
        NotTimed = 0,
        Timed,
        TimedPerQuestion
    }

    public enum ReviewerTypeEnum
    {
        QuickNote,
        TextFlashCard
    }

    public enum QuestionTypeEnum
    {
        BuiltIn,
        QandA,
        MultipleChoice,
        MultiChoiceSame
    }

    public enum QuizzFlagEnum
    {
        Others,
        WrongCategory,
        WrongGradeLevel,
    }

    public enum QuestionFlagEnum
    {
        Others,
        UnclearQuestion,
        QuestionNotRelevant, // Not relevant for the topic
        WrongAnswer,
        SpellingGrammarError,
    }
}
