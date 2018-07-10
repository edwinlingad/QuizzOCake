using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using L2L.Data.Configuration;
using L2L.Entities;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace L2L.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<DependentPermission> DependentPermissions { get; set; }

        public DbSet<TestLog> QuizLogs { get; set; }

        public DbSet<QuizzRating> QuizRatings { get; set; }
        public DbSet<QuizzUpvote> QuizUpvotes { get; set; }
        public DbSet<QuizzUserRating> QuizUserRatings { get; set; }

        public DbSet<Quizz> Quizzess { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<TestSetting> TestSettings { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<QuizzTag> QuizTags { get; set; }
        public DbSet<QuizzFlag> QuizzFlags { get; set; }

        public DbSet<Reviewer> Reviewers { get; set; }
        public DbSet<ReviewerItem> ReviewerItems { get; set; }
        public DbSet<QuickNote> QuickNotes { get; set; }
        public DbSet<TextFlashCard> TextFlashCards { get; set; }

        public DbSet<QuestionFlag> QuestionFlags { get; set; }

        public DbSet<QuizzBuiltIn> QuizBuiltIns { get; set; }
        public DbSet<QuizzBuiltInQuestionFlag> QuizBuiltInTypesQuestionFlags { get; set; }

        public DbSet<MultipleChoiceChoice> QuizMultipleChoiceChoices { get; set; }
        public DbSet<MultipleChoiceQuestion> QuizMultipleChoiceQuestions { get; set; }
        public DbSet<MultipleChoiceQuestionFlag> QuizMultipleChoiceQuestionFlags { get; set; }

        public DbSet<QandAAnswer> QuizQandAAnswers { get; set; }
        public DbSet<QandAQuestion> QuizQandAQuestions { get; set; }
        public DbSet<QandAQuestionFlag> QuizQandAQuestionFlags { get; set; }

        public DbSet<MultiChoiceSameQuestion> MultiChoiceSameQuestions { get; set; }
        public DbSet<MultiChoiceSameChoiceGroup> MultiChoiceSameChoiceGroups { get; set; }
        public DbSet<MultiChoiceSameChoice> MultiChoiceSameChoices { get; set; }

        public DbSet<FriendRelationship> FriendRelationships { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }

        public DbSet<PublicGroup> PublicGroups { get; set; }
        public DbSet<PublicGroupMember> PublicGroupMembers { get; set; }
        public DbSet<PublicGroupMembershipRequest> PublicGroupMembershipRequests { get; set; }

        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserGroupMember> UserGroupMembers { get; set; }

        public DbSet<Dependent> Dependents { get; set; }
        public DbSet<DependentRequestFromUser> DependentRequestFromUser { get; set; }
        public DbSet<DependentRequestFromChild> DependentRequestFromChild { get; set; }

        public DbSet<NewNotification> NewNotifications { get; set; }

        public DbSet<QuizzClassMember> QuizzClassMembers { get; set; }

        public static string ConnectionStringName
        {
            get
            {
                if (ConfigurationManager.AppSettings["ConnectionStringName"]
                    != null)
                {
                    return ConfigurationManager.
                        AppSettings["ConnectionStringName"].ToString();
                }

                return "DefaultConnection";
            }
        }

        static DataContext()
        {
            //Database.SetInitializer(new CustomDatabaseInitializer());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, L2L.Data.Migrations.Configuration>()); 
            //Database.SetInitializer(new DropCreateDatabaseAlways<DataContext>());
        }

        public DataContext()
            : base(nameOrConnectionString: DataContext.ConnectionStringName) 
        {
            Configuration.LazyLoadingEnabled = false;

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;

            _modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            InitializeQuizLogModels();
            InitializeUserModels();

            InitializeRelationshipModels();

            InitializeQuizRatingModels();

            InitializeQuizzModels();

            InitializeNotificationModels();
            InitializeNewNotificationModels();
            InitializeActivityModels();
            InitializeAssignmentModels();
            InitializeDailyRewardModels();

            InitializeMessageModels();
            InitializeClassroomModels();
        }

        

        private void InitializeQuizLogModels()
        {
            _modelBuilder.Configurations.Add(new QuizzLogConfiguration());
        }

        private void InitializeUserModels()
        {
            _modelBuilder.Configurations.Add(new ProfileConfiguration());
            _modelBuilder.Configurations.Add(new UserConfiguration());
            _modelBuilder.Configurations.Add(new DependentPermissionConfiguration());
        }

        private void InitializeRelationshipModels()
        {
            InitializeFriendModels();
            InitializePublicGroupModels();
            InitializeUserGroupModels();
            InitializeDependentsModels();

            _modelBuilder.Configurations.Add(new RelationshipNotificationConfiguration());
        }

        private void InitializeFriendModels()
        {
            _modelBuilder.Configurations.Add(new FriendRelationshipConfiguration());
            _modelBuilder.Configurations.Add(new FriendRequestConfiguration());
        }

        private void InitializePublicGroupModels()
        {
            _modelBuilder.Configurations.Add(new PublicGroupConfiguration());
            _modelBuilder.Configurations.Add(new PublicGroupMemberConfiguration());
            _modelBuilder.Configurations.Add(new PublicGroupMembershipRequestConfiguration());
        }

        private void InitializeUserGroupModels()
        {
            _modelBuilder.Configurations.Add(new UserGroupConfiguration());
            _modelBuilder.Configurations.Add(new UserGroupMemberConfiguration());
        }

        private void InitializeDependentsModels()
        {
            _modelBuilder.Configurations.Add(new DependentConfiguration());
            _modelBuilder.Configurations.Add(new DependentRequestFromUserConfiguration());
            _modelBuilder.Configurations.Add(new DependentRequestFromChildConfiguration());
        }

        private void InitializeQuizRatingModels()
        {
            _modelBuilder.Configurations.Add(new QuizzRatingConfiguration());
            _modelBuilder.Configurations.Add(new QuizzUpvoteConfiguration());
            _modelBuilder.Configurations.Add(new QuizzUserRatingConfiguration());
        }

        private void InitializeQuizzModels()
        {
            _modelBuilder.Configurations.Add(new CategoryConfiguration());
            _modelBuilder.Configurations.Add(new QuizzConfiguration());
            _modelBuilder.Configurations.Add(new QuestionConfiguration());
            _modelBuilder.Configurations.Add(new QuizSettingConfiguration());
            _modelBuilder.Configurations.Add(new TestConfiguration());
            _modelBuilder.Configurations.Add(new TestSnapshotConfiguration());
            _modelBuilder.Configurations.Add(new QuizzTagConfiguration());
            _modelBuilder.Configurations.Add(new QuestionFlagConfiguration());
            _modelBuilder.Configurations.Add(new QuizzFlagConfiguration());

            _modelBuilder.Configurations.Add(new QuizzCommentConfiguration());
            _modelBuilder.Configurations.Add(new QuizzCommentLikeConfiguration());
            _modelBuilder.Configurations.Add(new QuizzCommentFlagConfiguration());

            InitializeReviewerModels();
            InitializeQuizBuiltInModels();
            InitializeQuizMultipleChoiceModels();
            InitializeQuizQandAModels();
            InitializeMultiChoiceSameModels();

        }

        private void InitializeReviewerModels()
        {
            _modelBuilder.Configurations.Add(new ReviewerConfiguration());
            _modelBuilder.Configurations.Add(new ReviewerItemConfiguration());
            _modelBuilder.Configurations.Add(new QuickNoteConfiguration());
            _modelBuilder.Configurations.Add(new TextFlashCardConfiguration());
        }

        private void InitializeQuizBuiltInModels()
        {
            _modelBuilder.Configurations.Add(new QuizBuiltInConfiguration());
            _modelBuilder.Configurations.Add(new QuizBuiltInTypeQuestionFlagConfiguration());
        }

        private void InitializeQuizMultipleChoiceModels()
        {
            _modelBuilder.Configurations.Add(new MultipleChoiceChoiceConfiguration());
            _modelBuilder.Configurations.Add(new MultipleChoiceQuestionFlagConfiguration());
            _modelBuilder.Configurations.Add(new MultipleChoiceQuestionConfiguration());
        }

        private void InitializeQuizQandAModels()
        {
            _modelBuilder.Configurations.Add(new QandAQuestionFlagConfiguration());
            _modelBuilder.Configurations.Add(new QandAQuestionConfiguration());
            _modelBuilder.Configurations.Add(new QandAAnswerConfiguration());
        }

        private void InitializeMultiChoiceSameModels()
        {
            _modelBuilder.Configurations.Add(new MultiChoiceSameQuestionConfiguration());
            _modelBuilder.Configurations.Add(new MultiChoiceSameChoiceGroupConfiguration());
            _modelBuilder.Configurations.Add(new MultiChoiceSameChoiceConfiguration());
        }

        private void InitializeNotificationModels()
        {
            _modelBuilder.Configurations.Add(new NotificationConfiguration());

            _modelBuilder.Configurations.Add(new QuizzNotificationConfiguration());
            _modelBuilder.Configurations.Add(new QuizzNotificationSourceConfiguration());

            _modelBuilder.Configurations.Add(new QuizzCommentNotificationConfiguration());
            _modelBuilder.Configurations.Add(new QuizzCommentNotificationSourceConfiguration());

            _modelBuilder.Configurations.Add(new QuestionNotificationConfiguration());
            _modelBuilder.Configurations.Add(new QuestionNotificationSourceConfiguration());
        }

        private void InitializeNewNotificationModels()
        {
            _modelBuilder.Configurations.Add(new NewNotificationConfiguration());
        }

        private void InitializeActivityModels()
        {
            _modelBuilder.Configurations.Add(new ActivitiesConfiguration());
        }

        private void InitializeAssignmentModels()
        {
            _modelBuilder.Configurations.Add(new AssignmentGroupConfiguration());
            _modelBuilder.Configurations.Add(new AssignmentConfiguration());
        }

        private void InitializeDailyRewardModels()
        {
            _modelBuilder.Configurations.Add(new DailyRewardConfiguration());
            _modelBuilder.Configurations.Add(new DailyRewardPerUserConfiguration());
        }

        private void InitializeMessageModels()
        {
            _modelBuilder.Configurations.Add(new QuizzmateMsgThreadConfiguration());
            _modelBuilder.Configurations.Add(new QuizzmateMsgThreadMemberConfiguration());
            _modelBuilder.Configurations.Add(new QuizzmateMsg1Configuration());
        }

        private void InitializeClassroomModels()
        {
            _modelBuilder.Configurations.Add(new QuizzClassConfiguration());
            _modelBuilder.Configurations.Add(new QuizzClassCommentConfiguration());
            _modelBuilder.Configurations.Add(new QuizzClassAnnouncementConfiguration());
            _modelBuilder.Configurations.Add(new QuizzClassQuizzConfiguration());
            _modelBuilder.Configurations.Add(new QuizzClassLessonConfiguration());
            _modelBuilder.Configurations.Add(new QuizzClassLessonCommentConfiguration());
            _modelBuilder.Configurations.Add(new QuizzClassLessonQuizzConfiguration());
            _modelBuilder.Configurations.Add(new QuizzClassMemberConfiguration());
            _modelBuilder.Configurations.Add(new QuizzClassJoinRequestConfiguration());
            _modelBuilder.Configurations.Add(new QuizzClassInviteRequestConfiguration());
            _modelBuilder.Configurations.Add(new QuizzClassMaterialConfiguration());
            _modelBuilder.Configurations.Add(new QuizzClassChatConfiguration());
        }

        private DbModelBuilder _modelBuilder;
    }
}
