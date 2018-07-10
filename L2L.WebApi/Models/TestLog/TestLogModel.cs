using L2L.Entities;
using L2L.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{
    public class TestLogModel
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

        // Foreign keys
        public int UserId { get; set; }
        public int TestSettingId { get; set; }
        public int QuizzId { get; set; }
        public int AssignmentId { get; set; }

        // Navigation Properties
        public TestSetting TestSetting { get; set; }

        // generated from query
        public bool IsQuizzmate { get; set; }
        public bool IsAuthor { get; set; }
        public string AuthorUserName { get; set; }
        public string AuthorFullName { get; set; }

        // generated from code
        public bool IsPassed { get; set; }
        public string AuthorName { get; set; }
    }

    public class TestLogGroup
    {
        public QuizzSummary QuizzSummary { get; set; }
        public IEnumerable<TestLogSummary> TestLogs { get; set; }
    }

    public class QuizzSummary
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ScoreSum { get; set; }
        public int TotalSum { get; set; }
        public int Category { get; set; }
    }

    public class TestLogSummary
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public int Total { get; set; }
        public DateTime DateTaken { get; set; }
    }
}