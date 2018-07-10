using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class QuizzmateMsgThread
    {
        public QuizzmateMsgThread()
        {
            MsgThreadMembers = new List<QuizzmateMsgThreadMember>();
        }

        public int Id { get; set; }
        public bool IsGroupMsg { get; set; }
        public string GroupMessageName { get; set; }
        public string SignalRGroupName { get; set; }
        public bool IsDeleted { get; set; }

        // Foreign Keys

        // Navigation Properties
        public IList<QuizzmateMsgThreadMember> MsgThreadMembers { get; set; }
        public IList<QuizzmateMsg1> Messages { get; set; }
    }

    public class QuizzmateMsgThreadMember
    {
        public int Id { get; set; }
        public bool HasNew { get; set; }
        public int NewCount { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Foreign Keys
        public int QuizzmateMsgThreadId { get; set; }
        public int UserId { get; set; }

        // Navigation Properties
        public QuizzmateMsgThread QuizzmateMsgThread { get; set; }
        public User User { get; set; }
        
    }

    public class QuizzmateMsg1
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime PostedDate { get; set; }

        // Foreign Keys
        public int AuthorId { get; set; }
        public int QuizzmateMsgThreadId { get; set; }

        // Navigation Properties
        public User Author { get; set; }
        public QuizzmateMsgThread QuizzmateMsgThread { get; set; }
    }
}
