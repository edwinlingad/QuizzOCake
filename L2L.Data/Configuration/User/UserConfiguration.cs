using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Data.Configuration
{
    class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            this.Property(p => p.LocalAuthUserId)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(p => p.UserName)
                .IsRequired()
                .HasMaxLength(128);

            this.HasRequired(p => p.Profile)
                .WithRequiredPrincipal(q => q.User);

            ConfigureQuizzProperties();
            ConfigureQuestionProperties();
            ConfigureFriendProperties();
            ConfigureGroupProperties();
            ConfigureDependentProperties();
            ConfigureDependentPermissionProperties();

            ConfigureQuizPointsProperties();
        }

        private void ConfigureQuestionProperties()
        {
            //this.HasMany(p => p.QuestionsCreated)
            //        .WithRequired(p => p.Author);
        }

        private void ConfigureQuizzProperties()
        {
            this.HasMany(p => p.Quizzes)
                .WithRequired(q => q.Owner);
            this.HasMany(p => p.QuizLogs)
                .WithRequired(q => q.User);
            this.HasMany(p => p.UserRatings)
                .WithRequired(q => q.User);
            this.HasMany(p => p.QuizUpvotes)
                .WithRequired(q => q.User);
        }

        private void ConfigureFriendProperties()
        {
            this.HasMany(p => p.FriendsAsUser1)
                .WithRequired(q => q.User1);
            this.HasMany(p => p.FriendsAsUser2)
                .WithRequired(q => q.User2);
            this.HasMany(p => p.FriendRequestsTo)
                .WithRequired(q => q.RequestTo);
            this.HasMany(p => p.FriendRequestsFrom)
                .WithRequired(q => q.RequestFrom);
        }

        private void ConfigureGroupProperties()
        {
            this.HasMany(p => p.UserGroups)
                .WithRequired(q => q.Owner);
            this.HasMany(p => p.PublicGroupsCreated)
                .WithRequired(q => q.Creator);
            this.HasMany(p => p.PublicGroupsMemberOf)
                .WithRequired(q => q.Member);
            this.HasMany(p => p.PublicGroupRequests)
                .WithRequired(q => q.User);
        }

        private void ConfigureDependentProperties()
        {
            this.HasMany(p => p.AsUserDependents)
                .WithRequired(q => q.User);
            this.HasMany(p => p.AsChildDependsOn)
                .WithRequired(q => q.Child);

            this.HasMany(p => p.AsUserDependentRequestsFrom)
                .WithRequired(q => q.FromUser);
            this.HasMany(p => p.AsUserDependentRequestsTo)
                .WithRequired(q => q.ToChild);

            this.HasMany(p => p.AsChildDependentRequestsFrom)
                .WithRequired(q => q.FromChild);
            this.HasMany(p => p.AsChildDependentRequestsTo)
                .WithRequired(q => q.ToUser);

        }

        private void ConfigureDependentPermissionProperties()
        {
            this.HasOptional(p => p.DependentPermission)
                .WithRequired(q => q.User);
        }

        private void ConfigureQuizPointsProperties()
        {
            //this.Property(p => p.DailyPointsQuizzSelf)
            //    .IsRequired();
            //this.Property(p => p.DailyPointsQuizzOthers)
            //    .IsRequired();
        }
    }
}
