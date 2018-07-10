using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class QuizzClass
    {
        public QuizzClass()
        {
            Materials = new List<QuizzClassMaterial>();
            Lessons = new List<QuizzClassLesson>();
            Students = new List<QuizzClassMember>();
            Comments = new List<QuizzComment>(); // mistake - canot remove because error in DB
            ClassComments = new List<QuizzClassComment>();
        }

        public int Id { get; set; }
        public bool MembershipNeedsApproval { get; set; }
        public bool IsDeleted { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public int QuizzClassLessonIdx { get; set; }
        public string QuizzChatGuid { get; set; }
        public string ImageUrl { get; set; }

        // Foreign Keys
        public int TeacherId { get; set; }

        // Navigation Properties
        public User Teacher { get; set; }
        public IList<QuizzClassMaterial> Materials { get; set; }
        public IList<QuizzClassLesson> Lessons { get; set; }
        public IList<QuizzClassJoinRequest> JoinRequests { get; set; }
        public IList<QuizzClassInviteRequest> InviteRequests { get; set; }
        public IList<QuizzClassMember> Students { get; set; }
        public IList<QuizzComment> Comments { get; set; }  // mistake - canot remove because error in DB
        public IList<QuizzClassAnnouncement> Announcements { get; set; }
        public IList<QuizzClassLessonQuizz> LessonQuizzes { get; set; }
        public IList<QuizzClassQuizz> ClassQuizzes { get; set; }
        public IList<QuizzClassComment> ClassComments { get; set; }
    }

    public class QuizzClassComment
    {
        public int Id { get; set; }
        public DateTime PostedDate { get; set; }
        public string Comment { get; set; }
        public bool IsDeleted { get; set; }

        // Foreign Keys
        public int AuthorId { get; set; }
        public int QuizzClassId { get; set; }

        // Navigation Properties
        public User Author { get; set; }
        public QuizzClass QuizzClass { get; set; }
    } 

    public class QuizzClassAnnouncement
    {
        public int Id { get; set; }
        public string Announcement { get; set; }
        public DateTime PostedDate { get; set; }
        public bool IsDeleted { get; set; }

        // Foreign Keys
        public int QuizzClassId { get; set; }

        // Navigation Properties
        public QuizzClass QuizzClass { get; set; }
    }

    public class QuizzClassQuizz
    {
        public int Id { get; set; }
        public DateTime PostedDate { get; set; }

        // Foreign Keys
        public int QuizzId { get; set; }
        public int QuizzClassId { get; set; }

        // Navigation Properties
        public Quizz Quizz { get; set; }
        public QuizzClass QuizzClass { get; set; }
    }

    #region Quizz Class Lesson
    public class QuizzClassLesson
    {
        public int Id { get; set; }
        public int QuizzClassLesssonIdx { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }

        // Foreign Keys
        public int QuizzClassId { get; set; }

        // Navigation Properties
        public QuizzClass QuizzClass { get; set; }
        public IList<QuizzClassLessonComment> Comments { get; set; }
        public IList<QuizzClassMaterial> Materials { get; set; }
        public IList<QuizzClassLessonMessage> Messages { get; set; }
        public IList<QuizzClassLessonQuizz> Quizzes { get; set; }
    }

    public class QuizzClassLessonMessage : QzEditor
    {
        public int Id { get; set; }
        public DateTime PostedDate { get; set; }
        public string Message { get; set; }
        public bool IsDeleted { get; set; }

        // Foreign Keys
        public int QuizzClassLessonId { get; set; }

        // Navigation Properties
        public QuizzClassLesson QuizzClassLesson { get; set; }
    } 

    public class QuizzClassLessonComment
    {
        public int Id { get; set; }
        public DateTime PostedDate { get; set; }
        public string Comment { get; set; }
        public bool IsDeleted { get; set; }

        // Foreign Keys
        public int QuizzClassLessonId { get; set; }
        public int AuthorId { get; set; }

        // Navigation Properties
        public QuizzClassLesson QuizzClassLesson { get; set; }
        public User Author { get; set; }
    }

    public class QuizzClassLessonQuizz
    {
        public int Id { get; set; }
        public DateTime PostedDate { get; set; }

        // Foreign Keys
        public int QuizzId { get; set; }
        public int QuizzClassId { get; set; }
        public int QuizzClassLessonId { get; set; }

        // Navigation Properties
        public Quizz Quizz { get; set; }
        public QuizzClass QuizzClass { get; set; }
        public QuizzClassLesson QuizzClassLesson { get; set; }
    }
    #endregion

    #region Quizz Class Member
    public class QuizzClassMember
    {
        public int Id { get; set; }
        public bool IsNew { get; set; }
        public bool IsNewInviteAccepted { get; set; }
        public bool HasNew { get; set; }
        public int NewChatCount { get; set; }
        public bool IsParent { get; set; }
        public int? DependentId { get; set; }

        // *** Class
        public int NewClassCommentCount { get; set; }
        public int NewClassMaterialCount { get; set; }
        public int NewAnnouncementCount { get; set; }
        public int NewClassLesson { get; set; }
        public int NewClassQuizzCount { get; set; }

        // *** Lessons
        public string NewLessonMaterialCount { get; set; }
        // lesson discussion
        public string NewLessonCommentCount { get; set; }
        // lesson content
        public string NewLessonMessageCount { get; set; }
        // lesson quizz
        public string NewLessonQuizzCount { get; set; }

        // Foreign Keys
        public int QuizzClassId { get; set; }
        public int StudentId { get; set; }
        

        // Navigation Properties
        public User Student { get; set; }
        public QuizzClass QuizzClass { get; set; }
        //public User Dependent { get; set; }
    }

    public class QuizzClassJoinRequest
    {
        public int Id { get; set; }
        public DateTime PostedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsNew { get; set; }
        public string Message { get; set; }

        // Foreign Keys
        public int QuizzClassId { get; set; }
        public int UserId { get; set; }

        // Navigation Properties
        public QuizzClass QuizzClass { get; set; }
        public User User { get; set; }
    }

    public class QuizzClassInviteRequest
    {
        public int Id { get; set; }
        public DateTime PostedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsNew { get; set; }
        public string Message { get; set; }

        // Foreign Keys
        public int QuizzClassId { get; set; }
        public int UserId { get; set; }

        // Navigation Properties
        public QuizzClass QuizzClass { get; set; }
        public User User { get; set; }
    }
    #endregion

    #region Class Material
    public enum QuizzClassMaterialtypeEnum
    {
        Quizz,
        File,
        WebsiteUrl
    }

    public class QuizzClassMaterial
    {
        public int Id { get; set; }
        public QuizzClassMaterialtypeEnum MaterialType { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        // Foreign Keys
        public int? QuizzId { get; set; }
        public int? QuizzClassLessonId { get; set; }
        public int QuizzClassId { get; set; }

        // Navigation Properties
        public Quizz Quizz { get; set; }
        public QuizzClassLesson QuizzClassLesson { get; set; }
        public QuizzClass QuizzClass { get; set; }
    }
    #endregion

    #region Chat
    public class QuizzClassChat
    {
        public int Id { get; set; }
        public DateTime PostedDate { get; set; }
        public string Message { get; set; }

        // Foreign Keys
        public int QuizzClassId { get; set; }
        public int AuthorId { get; set; }

        // Navigation Properties
        public QuizzClass QuizzClass { get; set; }
        public User Author { get; set; }
    } 
    #endregion
}
