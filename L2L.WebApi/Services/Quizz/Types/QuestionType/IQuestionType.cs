using L2L.Entities.Enums;
using L2L.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.WebApi.Services
{
    public interface IQuestionType
    {
        bool DeleteQuestion(int id, bool callSaveChanges = true);
        QuestionModel CreateQuestion(object model);
    }
}
