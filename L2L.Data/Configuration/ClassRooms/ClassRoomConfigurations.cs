using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Data.Configuration
{
    class QuizzClassConfiguration : EntityTypeConfiguration<QuizzClass>
    {
        public QuizzClassConfiguration()
        {
            this.Property(p => p.ClassName)
                .IsRequired();
            this.HasMany(p => p.Materials)
                .WithRequired(q => q.QuizzClass);
            this.HasMany(p => p.Lessons)
                .WithRequired(q => q.QuizzClass);
            this.HasMany(p => p.Students)
                .WithRequired(q => q.QuizzClass);
            this.HasMany(p => p.JoinRequests)
                .WithRequired(q => q.QuizzClass);
            this.HasMany(p => p.InviteRequests)
                .WithRequired(q => q.QuizzClass);
            this.HasMany(p => p.LessonQuizzes)
                .WithRequired(q => q.QuizzClass);
            this.HasMany(p => p.ClassQuizzes)
                .WithRequired(q => q.QuizzClass);
            this.HasMany(p => p.ClassComments)
                .WithRequired(q => q.QuizzClass);

        }
    }

    class QuizzClassCommentConfiguration : EntityTypeConfiguration<QuizzClassComment>
    {
        public QuizzClassCommentConfiguration()
        {
            this.Property(p => p.Comment)
                .IsRequired();
            this.HasRequired(p => p.QuizzClass);
        }
    } 

    class QuizzClassAnnouncementConfiguration : EntityTypeConfiguration<QuizzClassAnnouncement>
    {
        public QuizzClassAnnouncementConfiguration()
        {
            this.Property(p => p.Announcement)
                .IsRequired();
            this.HasRequired(p => p.QuizzClass);
        }
    }

    class QuizzClassLessonConfiguration : EntityTypeConfiguration<QuizzClassLesson>
    {
        public QuizzClassLessonConfiguration()
        {
            this.Property(p => p.Title)
                .IsRequired();
            this.HasRequired(p => p.QuizzClass);
            this.HasMany(p => p.Comments)
                .WithRequired(q => q.QuizzClassLesson);
            this.HasMany(p => p.Materials)
                .WithOptional(q => q.QuizzClassLesson);
            this.HasMany(p => p.Messages)
                .WithRequired(q => q.QuizzClassLesson);
            this.HasMany(p => p.Quizzes)
                .WithRequired(q => q.QuizzClassLesson);
        }
    }

    class QuizzClassLessonCommentConfiguration : EntityTypeConfiguration<QuizzClassLessonComment>
    {
        public QuizzClassLessonCommentConfiguration()
        {
            this.Property(p => p.Comment)
                .IsRequired();
            this.HasRequired(p => p.QuizzClassLesson);
            this.HasRequired(p => p.Author);
        }
    }

    class QuizzClassLessonQuizzConfiguration : EntityTypeConfiguration<QuizzClassLessonQuizz>
    {
        public QuizzClassLessonQuizzConfiguration()
        {
            this.HasRequired(p => p.Quizz);
            this.HasRequired(p => p.QuizzClass);
            this.HasRequired(p => p.QuizzClassLesson);
        }
    }

    class QuizzClassQuizzConfiguration : EntityTypeConfiguration<QuizzClassQuizz>
    {
        public QuizzClassQuizzConfiguration()
        {
            this.HasRequired(p => p.Quizz);
            this.HasRequired(p => p.QuizzClass);
        }
    } 

    class QuizzClassMemberConfiguration : EntityTypeConfiguration<QuizzClassMember>
    {
        public QuizzClassMemberConfiguration()
        {
            this.HasRequired(p => p.Student);
            this.HasRequired(p => p.QuizzClass);
            //this.HasOptional(p => p.Dependent);
        }
    }

    class QuizzClassJoinRequestConfiguration : EntityTypeConfiguration<QuizzClassJoinRequest>
    {
        public QuizzClassJoinRequestConfiguration()
        {
            this.HasRequired(p => p.QuizzClass);
            this.HasRequired(p => p.User);
        }
    }

    class QuizzClassInviteRequestConfiguration : EntityTypeConfiguration<QuizzClassInviteRequest>
    {
        public QuizzClassInviteRequestConfiguration()
        {
            this.HasRequired(p => p.QuizzClass);
            this.HasRequired(p => p.User);
        }
    }

    class QuizzClassMaterialConfiguration : EntityTypeConfiguration<QuizzClassMaterial>
    {
        public QuizzClassMaterialConfiguration()
        {
            this.Property(p => p.Content).IsRequired();
            this.Property(p => p.Title).IsRequired();
            this.HasOptional(p => p.Quizz);
            this.HasOptional(p => p.QuizzClassLesson);
            this.HasRequired(p => p.QuizzClass);
        }
    }

    class QuizzClassChatConfiguration : EntityTypeConfiguration<QuizzClassChat>
    {
        public QuizzClassChatConfiguration()
        {
            this.Property(p => p.Message).IsRequired();
            this.HasRequired(p => p.QuizzClass);
            this.HasRequired(p => p.Author);
        }
    } 
}
