using L2L.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{
    public class LeftSideBarModel
    {
        public LeftSideBarModel()
        {
            TestSnapshots = new List<LayoutTestSnapshot>();
            Dependents = new List<LayoutDependentModel>();
            Assignments = new List<LayoutAssignmentModel>();
            AssignmentsGiven = new List<LayoutAssignmentGroupModel>();
            RecentQuizzes = new List<LayoutRecentQuizzModel>();
            MyQuizzes = new List<LayoutQuizzModel>();
            Bookmarks = new List<LayoutBookmarkModel>();
            SuggestedQuizzes = new List<LayoutSuggestedQuizzModel>();
            Groups = new List<LayoutGroupModel>();
        }

        public IList<LayoutTestSnapshot> TestSnapshots { get; set; }
        public IList<LayoutDependentModel> Dependents { get; set; }
        public bool HasMoreAssignments { get; set; }
        public IList<LayoutAssignmentModel> Assignments { get; set; }
        public bool HasMoreAssignmentsGiven { get; set; }
        public IList<LayoutAssignmentGroupModel> AssignmentsGiven { get; set; }
        public bool HasMoreRecentQuizzes { get; set; }
        public IList<LayoutRecentQuizzModel> RecentQuizzes { get; set; }
        public bool HasMoreMyQuizzes { get; set; }
        public IList<LayoutQuizzModel> MyQuizzes { get; set; }
        public bool HasMoreBookmarks { get; set; }
        public IList<LayoutBookmarkModel> Bookmarks { get; set; }
        public IList<LayoutSuggestedQuizzModel> SuggestedQuizzes { get; set; }
        public IList<LayoutGroupModel> Groups { get; set; }
    }

    public class LayoutTestSnapshot
    {
        public int Id { get; set; }
    }

    public class LayoutAssignmentModel
    {
        public int Id { get; set; }
        public string QuizzName { get; set; }
        public int Category { get; set; }
    }

    public class LayoutAssignmentGroupModel
    {
        public int Id { get; set; }
        public string QuizzName { get; set; }
        public int Category { get; set; }
    }

    public class LayoutBookmarkModel
    {
        public int Id { get; set; }
        public string QuizzName { get; set; }
    }

    public class LayoutDependentModel
    {
        public int UserId { get; set; }
        public string DependentName { get; set; }
    }

    public class LayoutGroupModel
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
    }

    public class LayoutQuizzModel : QuizzModel
    {
    }

    public class LayoutRecentQuizzModel
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public int Total { get; set; }
        public int Duration { get; set; }
        public DateTime DateTaken { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
        public int TestSettingId { get; set; }
        public string QuizzName { get; set; }
        public int QuizzId { get; set; }
        public int Category { get; set; }
    }

    public class LayoutSuggestedQuizzModel
    {
        public int Id { get; set; }
        public string QuizzName { get; set; }
    }
}