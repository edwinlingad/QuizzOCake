using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class AssignmentGroup
    {
        public AssignmentGroup()
        {
            Assignments = new List<Assignment>();
        }

        public int Id { get; set; }
        public string Message { get; set; }
        public int TargetScore { get; set; }
        public bool IsDefaultTestSetting { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DateAssigned { get; set; }
        public bool NoDueDate { get; set; }
        public DateTime TargetDate { get; set; }

        // Foreign Keys
        public int TestSettingId { get; set; }
        public int QuizzId { get; set; }
        public int AssignedById { get; set; }

        // Navigation Properties
        public TestSetting TestSetting { get; set; }
        public Quizz Quizz { get; set; }
        public User AssignedBy { get; set; }
        public IList<Assignment> Assignments { get; set; }
    } 

    public class Assignment
    {
        public int Id { get; set; }        
        public bool IsCompleted { get; set; }
        public DateTime CompletedDate { get; set; }
        public string CompletedMessage { get; set; }
        public string CompletedTestSetting { get; set; }
        public int CompletedScore { get; set; }

        // Foreign Keys
        public int DependentId { get; set; }
        public int AssignmentGroupId { get; set; }
        public int TestResultId { get; set; }

        // Navigation Properties
        public virtual User Dependent { get; set; }
        public virtual AssignmentGroup AssignmentGroup { get; set; }
        public virtual TestLog TestResult { get; set; }
    } 
}
