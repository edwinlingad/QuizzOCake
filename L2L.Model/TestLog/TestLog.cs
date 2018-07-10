using L2L.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class TestLog
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public int Total { get; set; }
        public int Duration { get; set; }
        public DateTime DateTaken { get; set; }
        public string Comment { get; set; }
        public string ResultBlob { get; set; }
        public string QuizzName { get; set; }
        public int Category { get; set; }

        // Foreign Keys
        public int UserId { get; set; }
        public int TestSettingId { get; set; }
        public int QuizzId { get; set; }
        public int AssignmentId { get; set; }

        // Navigation Properties
        public virtual Quizz Quizz { get; set; }
        public virtual User User { get; set; }
        public virtual TestSetting TestSetting { get; set; }
    } 
}
