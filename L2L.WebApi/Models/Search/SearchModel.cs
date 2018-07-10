using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{
    public enum SearchTypeEnum
    {
        User,
        Quizz,
        Quizzroom,
    }

    public class SearchModel : WithAgeModel
    {
        public SearchTypeEnum SearchType { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public bool IsQuizzmate { get; set; }
        public string ProfileImageUrl { get; set; }

        public int QuizzId { get; set; }
        public bool IsLive { get; set; }
        public int QuizzCategory { get; set; }
        public string QuizzName { get; set; }
        public string QuizzDescription { get; set; }
        public string QuizzAuthor { get; set; }
        public int QuizzAuthorId { get; set; }
        public string QuizzTags { get; set; }

        // from code
        public string QuizzAuthorUserName { get; set; }
        public string QuizzAuthorFullName { get; set; }

        // from query
        public int QuizzClassId { get; set; }
        public int TeacherId { get; set; }
        public bool IsRequestSent { get; set; }
        public bool IsInviteSent { get; set; }
        public bool IsMember { get; set; }
        public string TeacherUserName { get; set; }
        public string TeacherFullName { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; }
        public string QcTags { get; set; }
        public string ImageUrl { get; set; }

        // from code
        public string UserDisplayName { get; set; }
        public string TeacherName { get; set; }
        public bool IsTeacher { get; set; }
    }
}