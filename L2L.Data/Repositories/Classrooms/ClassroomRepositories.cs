using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Data.Repositories
{
    public class QuizzClassRepository : GenericRepository<QuizzClass>
    {
        public QuizzClassRepository(DbContext context) : base(context) { }
    }

    public class QuizzClassCommentRepository : GenericRepository<QuizzClassComment>
    {
        public QuizzClassCommentRepository(DbContext context) : base(context) { }
    }

    public class QuizzClassAnnouncementRepository : GenericRepository<QuizzClassAnnouncement>
    {
        public QuizzClassAnnouncementRepository(DbContext context) : base(context) { }
    }

    public class QuizzClassQuizzRepository : GenericRepository<QuizzClassQuizz>
    {
        public QuizzClassQuizzRepository(DbContext context) : base(context) { }
    } 

    public class QuizzClassLessonRepository : GenericRepository<QuizzClassLesson>
    {
        public QuizzClassLessonRepository(DbContext context) : base(context) { }
    }

    public class QuizzClassLessonMessageRepository : GenericRepository<QuizzClassLessonMessage>
    {
        public QuizzClassLessonMessageRepository(DbContext context) : base(context) { }
    }

    public class QuizzClassLessonCommentRepository : GenericRepository<QuizzClassLessonComment>
    {
        public QuizzClassLessonCommentRepository(DbContext context) : base(context) { }
    }

    public class QuizzClassLessonQuizzRepository : GenericRepository<QuizzClassLessonQuizz>
    {
        public QuizzClassLessonQuizzRepository(DbContext context) : base(context) { }
    } 

    public class QuizzClassMemberRepository : GenericRepository<QuizzClassMember>
    {
        public QuizzClassMemberRepository(DbContext context) : base(context) { }
    }

    public class QuizzClassJoinRequestRepository : GenericRepository<QuizzClassJoinRequest>
    {
        public QuizzClassJoinRequestRepository(DbContext context) : base(context) { }
    }

    public class QuizzClassInviteRequestRepository : GenericRepository<QuizzClassInviteRequest>
    {
        public QuizzClassInviteRequestRepository(DbContext context) : base(context) { }
    } 

    public class QuizzClassMaterialRepository : GenericRepository<QuizzClassMaterial>
    {
        public QuizzClassMaterialRepository(DbContext context) : base(context) { }
    } 

    public class QuizzClassChatRepository : GenericRepository<QuizzClassChat>
    {
        public QuizzClassChatRepository(DbContext context) : base(context) { }
    } 
}
