using L2L.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{
    public class AssignmentQuizzModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsBuiltIn { get; set; }
        public int OwnerId { get; set; }
        public int Category { get; set; }

        public int NumQuestions { get; set; }
        public int NumQuickNotes { get; set; }

        public string OwnerName { get; set; }
        public int ReviewerId { get; set; }
        public int TestId { get; set; }
        
    }

    public class AssignmentGroupModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int TargetScore { get; set; }
        public bool IsDefaultTestSetting { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DateAssigned { get; set; }
        public bool NoDueDate { get; set; }
        public DateTime TargetDate { get; set; }
        
        // Extra values        
        public string AssignedByFullName { get; set; }

        // Foreign Keys
        public int TestSettingId { get; set; }
        public int QuizzId { get; set; }
        public int AssignedById { get; set; }

        // Navigation Properties
        public TestSettingModel TestSetting { get; set; }
        public IList<AssignmentInfo> Assignments { get; set; }
        public AssignmentQuizzModel Quizz { get; set; }
    }

    public class AssignmentInfo
    {
        public int Id { get; set; }
        public string DependentFullName { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CompletedDate { get; set; }
        public int CompletedScore { get; set; }

        // Foreign Keys
        public int DependentId { get; set; }
        public int TestResultId { get; set; }
    }

    public class AssignmentModel  
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

        public AssignmentGroupModel AssignmentGroup { get; set; }
    }
}