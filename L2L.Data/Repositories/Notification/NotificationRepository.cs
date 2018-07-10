using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Data.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>
    {
        public NotificationRepository(DbContext context) : base(context) { }
    }

    public class QuizzNotificationRepository : GenericRepository<QuizzNotification>
    {
        public QuizzNotificationRepository(DbContext context) : base(context) { }
    }

    public class QuizzNotificationSourceRepository : GenericRepository<QuizzNotificationSource>
    {
        public QuizzNotificationSourceRepository(DbContext context) : base(context) { }
    }

    public class QuizzCommentNotificationRepository : GenericRepository<QuizzCommentNotification>
    {
        public QuizzCommentNotificationRepository(DbContext context) : base(context) { }
    } 

    public class QuizzCommentNotificationSourceRepository : GenericRepository<QuizzCommentNotificationSource>
    {
        public QuizzCommentNotificationSourceRepository(DbContext context) : base(context) { }
    } 

    public class QuestionNotificationRepository : GenericRepository<QuestionNotification>
    {
        public QuestionNotificationRepository(DbContext context) : base(context) { }
    }

    public class QuestionNotificationSourceRepository : GenericRepository<QuestionNotificationSource>
    {
        public QuestionNotificationSourceRepository(DbContext context) : base(context) { }
    } 
}
