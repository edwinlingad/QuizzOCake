using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Entities.Enums;

namespace L2L.Entities
{
    public class Quizz : ITrackableModel
    {
        public Quizz()
        {
            Tests = new HashSet<Test>();
            Reviewers = new HashSet<Reviewer>();
            Tags = new HashSet<QuizzTag>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsLive { get; set; }
        public bool IsDeleted { get; set; }
        public QuizzVisibilityEnum Visibility { get; set; }
        public int ModifyPermission { get; set; }
        public int Category { get; set; }
        public QuizzDifficultyEnum Difficulty { get; set; }
        public QuizzGradeLevelEnum GradeLevelMin { get; set; }
        public QuizzGradeLevelEnum GradeLevelMax { get; set; }
        public string QuizzTags { get; set; }

        public bool IsBuiltIn { get; set; }
        public BuiltInTypeEnum BuiltInType { get; set; }
        public int MainType { get; set; }
        public int SubType { get; set; }
        public int SubType2 { get; set; }
        public int SubType3 { get; set; }
        public int SubType4 { get; set; }
        public int SubType5 { get; set; }

        // Foreign Keys
        public int OwnerId { get; set; }

        // Navigation Properties
        public virtual User Owner { get; set; }
        public virtual QuizzRating QuizRating { get; set; }
        public virtual ICollection<Test> Tests { get; set; }
        public virtual ICollection<Reviewer> Reviewers { get; set; }
        public virtual ICollection<QuizzTag> Tags { get; set; }
        public virtual IList<QuizzComment> Comments { get; set; }
        public virtual IList<TestLog> TestLogs { get; set; }
    }

    public class QuizzComment
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime PostedDate { get; set; }
        public bool IsDeleted { get; set; }

        // Foreign Keys
        public int AuthorId { get; set; }
        public int QuizzId { get; set; }

        // Navigation Properties
        public virtual User Author { get; set; }
        public virtual Quizz Quizz { get; set; }
        public virtual IList<QuizzCommentLike> Likes { get; set; }
        public virtual IList<QuizzCommentFlag> Flags { get; set; }
    }

    public class QuizzCommentLike
    {
        public int Id { get; set; }
        public int UpVote { get; set; }

        // Foreign Keys
        public int AuthorId { get; set; }
        public int QuizzCommentId { get; set; }

        // Navigation Properties
        public virtual User Author { get; set; }
        public virtual QuizzComment QuizzComment { get; set; }
    }

    public class QuizzCommentFlag
    {
        public int Id { get; set; }
        public int UpVote { get; set; }

        // Foreign Key
        public int AuthorId { get; set; }
        public int QuizzCommentId { get; set; }

        // Navigation Properties
        public virtual User Author { get; set; }
        public virtual QuizzComment QuizzComment { get; set; }
    } 
}
