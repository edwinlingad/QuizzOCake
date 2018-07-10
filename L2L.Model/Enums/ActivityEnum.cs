using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities.Enums
{
    public enum ActivityEnum
    {
        QuizzLike,              // 0
        QuizzCreate,            // 1        // For Parent
        QuizzLive,              // 2
        QuizzTake,              // 3        // Score for parent

        QuizzCommentCreate,     // 4
        QuizzCommentModify,     // 5        // For Parent
        QuizzCommentLike,       // 6
        QuizzCommentFlag,       // 7        // For Parent
        
        QuizzRecivedComment,    // 8        // For Parent
        QuizzCommentFlagged,    // 9        // For Parent

        QuestionFlagged         // 10        // For Parent
    }
}
