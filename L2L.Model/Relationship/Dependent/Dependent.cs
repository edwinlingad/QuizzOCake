using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class Dependent
    {
        public Dependent()
        {
            NwQuizzSubmit = true;
            NwQuizzLive = true;

            NwQuizzReceiveComment = true;
            NwPostComment = true;
            NwPostedCommentFlagged = true;
            NwQuestionFlagged = true;

            NwMessageSent = true;
            NwMessageReceived = true;

            NwQuizzmateRequest = true;
            NwQuizzmateRequestAccept = true;
        }

        public int Id { get; set; }
        public bool IsPrimary { get; set; }

        public bool NwQuizzSubmit { get; set; }
        public bool NwQuizzLive { get; set; }
        public bool NwQuizzReceiveComment { get; set; }

        public bool NwPostComment { get; set; }
        public bool NwPostedCommentFlagged { get; set; }
        public bool NwQuestionFlagged { get; set; }

        public bool NwMessageSent { get; set; }
        public bool NwMessageReceived { get; set; }

        public bool NwQuizzmateRequest { get; set; }
        public bool NwQuizzmateRequestAccept { get; set; }

        // Foreign Keys
        public int UserId { get; set; }
        public int ChildId { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual User Child { get; set; }
    } 
}
