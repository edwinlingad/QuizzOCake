using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Data.Configuration
{
    class NotificationConfiguration : EntityTypeConfiguration<Notification>
    {
        public NotificationConfiguration()
        {
            this.HasOptional(p => p.QuizzNotification)
                .WithRequired(q => q.Notification);
            this.HasOptional(p => p.QuestionNotification)
                .WithRequired(q => q.Notification);
            this.HasRequired(p => p.User);
        }
    }

    class QuizzNotificationConfiguration : EntityTypeConfiguration<QuizzNotification>
    {
        public QuizzNotificationConfiguration()
        {
            this.HasRequired(p => p.User);
            this.HasRequired(p => p.Quizz);
            this.HasRequired(p => p.Notification);
            this.HasMany(p => p.NotificationSources)
                .WithRequired(q => q.QuizzNotification);
        }
    }

    class QuizzNotificationSourceConfiguration : EntityTypeConfiguration<QuizzNotificationSource>
    {
        public QuizzNotificationSourceConfiguration()
        {
            this.HasRequired(p => p.Source);
            this.HasRequired(p => p.QuizzNotification);
        }
    } 

    class QuizzCommentNotificationConfiguration : EntityTypeConfiguration<QuizzCommentNotification>
    {
        public QuizzCommentNotificationConfiguration()
        {
            this.HasRequired(p => p.User);
            this.HasRequired(p => p.QuizzComment);
            this.HasRequired(p => p.Notification);
            this.HasMany(p => p.NotificationSources)
                .WithRequired(q => q.QuizzCommentNotification);
        }
    }

    class QuizzCommentNotificationSourceConfiguration : EntityTypeConfiguration<QuizzCommentNotificationSource>
    {
        public QuizzCommentNotificationSourceConfiguration()
        {
            this.HasRequired(p => p.Source);
            this.HasRequired(p => p.QuizzCommentNotification);
        }
    } 

    class QuestionNotificationConfiguration : EntityTypeConfiguration<QuestionNotification>
    {
        public QuestionNotificationConfiguration()
        {
            this.HasRequired(p => p.User);
            this.HasRequired(p => p.Question);
            this.HasRequired(p => p.Notification);
            this.HasMany(p => p.NotificationSources)
                .WithRequired(q => q.QuestionNotification);
        }
    }

    class QuestionNotificationSourceConfiguration : EntityTypeConfiguration<QuestionNotificationSource>
    {
        public QuestionNotificationSourceConfiguration()
        {
            this.HasRequired(p => p.Source);
            this.HasRequired(p => p.QuestionNotification);
        }
    } 
}
