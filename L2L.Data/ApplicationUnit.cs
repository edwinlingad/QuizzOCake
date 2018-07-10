using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Data.Repositories;
using System.Data.Entity.Infrastructure;

namespace L2L.Data
{
    public class ApplicationUnit : IDisposable
    {
        private DataContext _context = new DataContext();

        public ApplicationUnit()
        {
            //_context.Database.Initialize(true);
            _context.Database.Log = Console.Write;
        }

        #region User
        private ProfileRepository _profiles = null;
        public ProfileRepository Profiles
        {
            get
            {
                if (this._profiles == null)
                    this._profiles = new ProfileRepository(this._context);
                return this._profiles;
            }
        }

        private UserRepository _users = null;
        public UserRepository Users
        {
            get
            {
                if (this._users == null)
                    this._users = new UserRepository(this._context);
                return this._users;
            }
        }

        private DependentPermissionRepository _DependentPermissions = null;
        public DependentPermissionRepository DependentPermissions
        {
            get
            {
                if (this._DependentPermissions == null)
                    this._DependentPermissions = new DependentPermissionRepository(this._context);
                return this._DependentPermissions;
            }
        }
        #endregion

        #region QuizLog
        private QuizzLogRepository _quizLogs = null;
        public QuizzLogRepository QuizLogs
        {
            get
            {
                if (this._quizLogs == null)
                    this._quizLogs = new QuizzLogRepository(this._context);
                return this._quizLogs;
            }
        }
        #endregion

        #region QuizRating

        private QuizzRatingRepository _quizzRatings = null;
        public QuizzRatingRepository QuizzRatings
        {
            get
            {
                if (this._quizzRatings == null)
                    this._quizzRatings = new QuizzRatingRepository(this._context);
                return this._quizzRatings;
            }
        }

        private QuizzUpvoteRepository _quizzUpvotes = null;
        public QuizzUpvoteRepository QuizzUpvotes
        {
            get
            {
                if (this._quizzUpvotes == null)
                    this._quizzUpvotes = new QuizzUpvoteRepository(this._context);
                return this._quizzUpvotes;
            }
        }

        private QuizzUserRatingRepository _quizzUserRatings = null;
        public QuizzUserRatingRepository QuizzUserRatings
        {
            get
            {
                if (this._quizzUserRatings == null)
                    this._quizzUserRatings = new QuizzUserRatingRepository(this._context);
                return this._quizzUserRatings;
            }
        }

        #endregion

        #region Quizz
        private QuizzCategoryRepository _quizzCategories = null;
        public QuizzCategoryRepository QuizzCategories
        {
            get
            {
                if (this._quizzCategories == null)
                    this._quizzCategories = new QuizzCategoryRepository(this._context);
                return this._quizzCategories;
            }
        }

        private QuizzRepository _quizzes = null;
        public QuizzRepository Quizzes
        {
            get
            {
                if (this._quizzes == null)
                    this._quizzes = new QuizzRepository(this._context);
                return this._quizzes;
            }
        }

        private QuestionRepository _questions = null;
        public QuestionRepository Questions
        {
            get
            {
                if (this._questions == null)
                    this._questions = new QuestionRepository(this._context);
                return this._questions;
            }
        }

        private QuizSettingRepository _testSettings = null;
        public QuizSettingRepository TestSettings
        {
            get
            {
                if (this._testSettings == null)
                    this._testSettings = new QuizSettingRepository(this._context);
                return this._testSettings;
            }
        }

        private TestRepository _tests = null;
        public TestRepository Tests
        {
            get
            {
                if (this._tests == null)
                    this._tests = new TestRepository(this._context);
                return this._tests;
            }
        }

        private TestSnapshotRepository _testSnapshots = null;
        public TestSnapshotRepository TestSnapshots
        {
            get
            {
                if (this._testSnapshots == null)
                    this._testSnapshots = new TestSnapshotRepository(this._context);
                return this._testSnapshots;
            }
        }

        private QuizzTagRepository _quizTags = null;
        public QuizzTagRepository QuizTags
        {
            get
            {
                if (this._quizTags == null)
                    this._quizTags = new QuizzTagRepository(this._context);
                return this._quizTags;
            }
        }

        private QuizzFlagRepository _QuizzFlags = null;
        public QuizzFlagRepository QuizzFlags
        {
            get
            {
                if (this._QuizzFlags == null)
                    this._QuizzFlags = new QuizzFlagRepository(this._context);
                return this._QuizzFlags;
            }
        }

        private QuizzCommentRepository _QuizzComments = null;
        public QuizzCommentRepository QuizzComments
        {
            get
            {
                if (this._QuizzComments == null)
                    this._QuizzComments = new QuizzCommentRepository(this._context);
                return this._QuizzComments;
            }
        }

        private QuizzCommentLikeRepository _QuizzCommentLikes = null;
        public QuizzCommentLikeRepository QuizzCommentLikes
        {
            get
            {
                if (this._QuizzCommentLikes == null)
                    this._QuizzCommentLikes = new QuizzCommentLikeRepository(this._context);
                return this._QuizzCommentLikes;
            }
        }

        private QuizzCommentFlagRepository _QuizzCommentFlags = null;
        public QuizzCommentFlagRepository QuizzCommentFlags
        {
            get
            {
                if (this._QuizzCommentFlags == null)
                    this._QuizzCommentFlags = new QuizzCommentFlagRepository(this._context);
                return this._QuizzCommentFlags;
            }
        }

        #region Reviewer
        private ReviewerRepository _Reviewers = null;
        public ReviewerRepository Reviewers
        {
            get
            {
                if (this._Reviewers == null)
                    this._Reviewers = new ReviewerRepository(this._context);
                return this._Reviewers;
            }
        }

        private ReviewerItemRepository _reviewerItems = null;
        public ReviewerItemRepository ReviewerItems
        {
            get
            {
                if (this._reviewerItems == null)
                    this._reviewerItems = new ReviewerItemRepository(this._context);
                return this._reviewerItems;
            }
        }

        private TextFlashCardRepository _textFlashCards = null;
        public TextFlashCardRepository TextFlashCards
        {
            get
            {
                if (this._textFlashCards == null)
                    this._textFlashCards = new TextFlashCardRepository(this._context);
                return this._textFlashCards;
            }
        }

        private QuickNoteRepository _quickNotes = null;
        public QuickNoteRepository QuickNotes
        {
            get
            {
                if (this._quickNotes == null)
                    this._quickNotes = new QuickNoteRepository(this._context);
                return this._quickNotes;
            }
        }
        #endregion

        #region Type

        private QuestionFlagRepository _QuestionFlags = null;
        public QuestionFlagRepository QuestionFlags
        {
            get
            {
                if (this._QuestionFlags == null)
                    this._QuestionFlags = new QuestionFlagRepository(this._context);
                return this._QuestionFlags;
            }
        }

        #region BuiltIn

        private QuizBuiltInRepository _quizBuiltIns = null;
        public QuizBuiltInRepository QuizBuiltIns
        {
            get
            {
                if (this._quizBuiltIns == null)
                    this._quizBuiltIns = new QuizBuiltInRepository(this._context);
                return this._quizBuiltIns;
            }
        }

        private QuizBuiltInQuestionFlagRepository _quizBuiltInTypeQuestionFlags = null;
        public QuizBuiltInQuestionFlagRepository QuizBuiltInTypeQuestionFlags
        {
            get
            {
                if (this._quizBuiltInTypeQuestionFlags == null)
                    this._quizBuiltInTypeQuestionFlags = new QuizBuiltInQuestionFlagRepository(this._context);
                return this._quizBuiltInTypeQuestionFlags;
            }
        }

        #endregion

        #region MultipleChoice

        private MultipleChoiceChoiceRepository _multipleChoiceChoices = null;
        public MultipleChoiceChoiceRepository MultipleChoiceChoices
        {
            get
            {
                if (this._multipleChoiceChoices == null)
                    this._multipleChoiceChoices = new MultipleChoiceChoiceRepository(this._context);
                return this._multipleChoiceChoices;
            }
        }

        private MultipleChoiceQuestionRepository _multipleChoiceQuestions = null;
        public MultipleChoiceQuestionRepository MultipleChoiceQuestions
        {
            get
            {
                if (this._multipleChoiceQuestions == null)
                    this._multipleChoiceQuestions = new MultipleChoiceQuestionRepository(this._context);
                return this._multipleChoiceQuestions;
            }
        }

        private MultipleChoiceQuestionFlagRepository _multipleChoiceQuestionFlags = null;
        public MultipleChoiceQuestionFlagRepository MultipleChoiceQuestionFlags
        {
            get
            {
                if (this._multipleChoiceQuestionFlags == null)
                    this._multipleChoiceQuestionFlags = new MultipleChoiceQuestionFlagRepository(this._context);
                return this._multipleChoiceQuestionFlags;
            }
        }

        #endregion

        #region QandA

        private QandAAnswerRepository _qandAAnswers = null;
        public QandAAnswerRepository QandAAnswers
        {
            get
            {
                if (this._qandAAnswers == null)
                    this._qandAAnswers = new QandAAnswerRepository(this._context);
                return this._qandAAnswers;
            }
        }

        private QandAQuestionRepository _qandAQuestions = null;
        public QandAQuestionRepository QandAQuestions
        {
            get
            {
                if (this._qandAQuestions == null)
                    this._qandAQuestions = new QandAQuestionRepository(this._context);
                return this._qandAQuestions;
            }
        }

        private QandAQuestionFlagRepository _qandAQuestionsFlags = null;
        public QandAQuestionFlagRepository QandAQuestionFlags
        {
            get
            {
                if (this._qandAQuestionsFlags == null)
                    this._qandAQuestionsFlags = new QandAQuestionFlagRepository(this._context);
                return this._qandAQuestionsFlags;
            }
        }
        #endregion

        #region MultiChoiceSame
        private MultiChoiceSameQuestionRepository _MultiChoiceSameQuestions = null;
        public MultiChoiceSameQuestionRepository MultiChoiceSameQuestions
        {
            get
            {
                if (this._MultiChoiceSameQuestions == null)
                    this._MultiChoiceSameQuestions = new MultiChoiceSameQuestionRepository(this._context);
                return this._MultiChoiceSameQuestions;
            }
        }

        private MultiChoiceSameChoiceGroupRepository _MultiChoiceSameChoiceGroups = null;
        public MultiChoiceSameChoiceGroupRepository MultiChoiceSameChoiceGroups
        {
            get
            {
                if (this._MultiChoiceSameChoiceGroups == null)
                    this._MultiChoiceSameChoiceGroups = new MultiChoiceSameChoiceGroupRepository(this._context);
                return this._MultiChoiceSameChoiceGroups;
            }
        }

        private MultiChoiceSameChoiceRepository _MultiChoiceSameChoices = null;
        public MultiChoiceSameChoiceRepository MultiChoiceSameChoices
        {
            get
            {
                if (this._MultiChoiceSameChoices == null)
                    this._MultiChoiceSameChoices = new MultiChoiceSameChoiceRepository(this._context);
                return this._MultiChoiceSameChoices;
            }
        }
        #endregion

        #endregion
        #endregion

        #region Relationship
        #region Friends

        private FriendRelationshipRepository _friendRelationships = null;
        public FriendRelationshipRepository FriendRelationships
        {
            get
            {
                if (this._friendRelationships == null)
                    this._friendRelationships = new FriendRelationshipRepository(this._context);
                return this._friendRelationships;
            }
        }

        private FriendRequestRepository _friendRequests = null;
        public FriendRequestRepository FriendRequests
        {
            get
            {
                if (this._friendRequests == null)
                    this._friendRequests = new FriendRequestRepository(this._context);
                return this._friendRequests;
            }
        }

        #endregion
        #region PublicGroup

        private PublicGroupMemberRepository _publicGroupMembers = null;
        public PublicGroupMemberRepository PublicGroupMembers
        {
            get
            {
                if (this._publicGroupMembers == null)
                    this._publicGroupMembers = new PublicGroupMemberRepository(this._context);
                return this._publicGroupMembers;
            }
        }

        private PublicGroupMembershipRequestRepository _publicGroupMembershipRequests = null;
        public PublicGroupMembershipRequestRepository PublicGroupMembershipRequests
        {
            get
            {
                if (this._publicGroupMembershipRequests == null)
                    this._publicGroupMembershipRequests = new PublicGroupMembershipRequestRepository(this._context);
                return this._publicGroupMembershipRequests;
            }
        }

        private PublicGroupRepository _publicGroups = null;
        public PublicGroupRepository PublicGroups
        {
            get
            {
                if (this._publicGroups == null)
                    this._publicGroups = new PublicGroupRepository(this._context);
                return this._publicGroups;
            }
        }

        #endregion
        #region UserGroup

        private UserGroupMemberRepository _userGroupMembers = null;
        public UserGroupMemberRepository UserGroupMembers
        {
            get
            {
                if (this._userGroupMembers == null)
                    this._userGroupMembers = new UserGroupMemberRepository(this._context);
                return this._userGroupMembers;
            }
        }

        private UserGroupRepository _userGroups = null;
        public UserGroupRepository UserGroups
        {
            get
            {
                if (this._userGroups == null)
                    this._userGroups = new UserGroupRepository(this._context);
                return this._userGroups;
            }
        }

        #endregion
        #region Dependent
        private DependentRepository _dependents = null;
        public DependentRepository Dependents
        {
            get
            {
                if (this._dependents == null)
                    this._dependents = new DependentRepository(this._context);
                return this._dependents;
            }
        }

        private DependentRequestFromUserRepository _dependentRequestsFromUser = null;
        public DependentRequestFromUserRepository DependentRequestsFromUser
        {
            get
            {
                if (this._dependentRequestsFromUser == null)
                    this._dependentRequestsFromUser = new DependentRequestFromUserRepository(this._context);
                return this._dependentRequestsFromUser;
            }
        }

        private DependentRequestFromChildRepository _dependentRequestsFromChild = null;
        public DependentRequestFromChildRepository DependentRequestsFromChild
        {
            get
            {
                if (this._dependentRequestsFromChild == null)
                    this._dependentRequestsFromChild = new DependentRequestFromChildRepository(this._context);
                return this._dependentRequestsFromChild;
            }
        }
        #endregion

        #region
        private RelationshipNotificationRepository _relationshipNotifications = null;
        public RelationshipNotificationRepository RelationshipNotifications
        {
            get
            {
                if (this._relationshipNotifications == null)
                    this._relationshipNotifications = new RelationshipNotificationRepository(this._context);
                return this._relationshipNotifications;
            }
        }
        #endregion
        #endregion

        #region NewNotification
        private NewNotificationRepository _newNotifications = null;
        public NewNotificationRepository NewNotifications
        {
            get
            {
                if (this._newNotifications == null)
                    this._newNotifications = new NewNotificationRepository(this._context);
                return this._newNotifications;
            }
        }
        #endregion

        #region Notification
        private NotificationRepository _notifications = null;
        public NotificationRepository Notifications
        {
            get
            {
                if (this._notifications == null)
                    this._notifications = new NotificationRepository(this._context);
                return this._notifications;
            }
        }

        #region Quizz Notification
        private QuizzNotificationRepository _quizzNotifications = null;
        public QuizzNotificationRepository QuizzNotifications
        {
            get
            {
                if (this._quizzNotifications == null)
                    this._quizzNotifications = new QuizzNotificationRepository(this._context);
                return this._quizzNotifications;
            }
        }

        private QuizzNotificationSourceRepository _QuizzNotificationSources = null;
        public QuizzNotificationSourceRepository QuizzNotificationSources
        {
            get
            {
                if (this._QuizzNotificationSources == null)
                    this._QuizzNotificationSources = new QuizzNotificationSourceRepository(this._context);
                return this._QuizzNotificationSources;
            }
        }
        #endregion

        #region Quizz Comment Notification
        private QuizzCommentNotificationRepository _QuizzCommentNotifications = null;
        public QuizzCommentNotificationRepository QuizzCommentNotifications
        {
            get
            {
                if (this._QuizzCommentNotifications == null)
                    this._QuizzCommentNotifications = new QuizzCommentNotificationRepository(this._context);
                return this._QuizzCommentNotifications;
            }
        }

        private QuizzCommentNotificationSourceRepository _QuizzCommentNotificationSources = null;
        public QuizzCommentNotificationSourceRepository QuizzCommentNotificationSources
        {
            get
            {
                if (this._QuizzCommentNotificationSources == null)
                    this._QuizzCommentNotificationSources = new QuizzCommentNotificationSourceRepository(this._context);
                return this._QuizzCommentNotificationSources;
            }
        }

        #endregion

        #region Question Notification
        private QuestionNotificationRepository _questionNotifications = null;
        public QuestionNotificationRepository QuestionNotifications
        {
            get
            {
                if (this._questionNotifications == null)
                    this._questionNotifications = new QuestionNotificationRepository(this._context);
                return this._questionNotifications;
            }
        }

        private QuestionNotificationSourceRepository _QuestionNotificationSources = null;
        public QuestionNotificationSourceRepository QuestionNotificationSources
        {
            get
            {
                if (this._QuestionNotificationSources == null)
                    this._QuestionNotificationSources = new QuestionNotificationSourceRepository(this._context);
                return this._QuestionNotificationSources;
            }
        }
        #endregion 
        #endregion

        #region Activities
        private ActivityRepository _activities = null;
        public ActivityRepository Activities
        {
            get
            {
                if (this._activities == null)
                    this._activities = new ActivityRepository(this._context);
                return this._activities;
            }
        }
        #endregion

        #region Assignment
        private AssignmentGroupRepository _assignmentGroups = null;
        public AssignmentGroupRepository AssignmentGroups
        {
            get
            {
                if (this._assignmentGroups == null)
                    this._assignmentGroups = new AssignmentGroupRepository(this._context);
                return this._assignmentGroups;
            }
        }

        private AssignmentRepository _assignments = null;
        public AssignmentRepository Assignments
        {
            get
            {
                if (this._assignments == null)
                    this._assignments = new AssignmentRepository(this._context);
                return this._assignments;
            }
        }
        #endregion

        #region QuizzPoints
        private DailyRewardRepository _DailyRewards = null;
        public DailyRewardRepository DailyRewards
        {
            get
            {
                if (this._DailyRewards == null)
                    this._DailyRewards = new DailyRewardRepository(this._context);
                return this._DailyRewards;
            }
        }

        private DailyRewardPerUserRepository _DailyRewardPerUsers = null;
        public DailyRewardPerUserRepository DailyRewardPerUsers
        {
            get
            {
                if (this._DailyRewardPerUsers == null)
                    this._DailyRewardPerUsers = new DailyRewardPerUserRepository(this._context);
                return this._DailyRewardPerUsers;
            }
        }
        #endregion

        #region Messages
        private QuizzmateMsgThreadRepository _QuizzmateMsgThreads = null;
        public QuizzmateMsgThreadRepository QuizzmateMsgThreads
        {
            get
            {
                if (this._QuizzmateMsgThreads == null)
                    this._QuizzmateMsgThreads = new QuizzmateMsgThreadRepository(this._context);
                return this._QuizzmateMsgThreads;
            }
        }

        private QuizzmateMsgThreadMemberRepository _QuizzmateMsgThreadMembers = null;
        public QuizzmateMsgThreadMemberRepository QuizzmateMsgThreadMembers
        {
            get
            {
                if (this._QuizzmateMsgThreadMembers == null)
                    this._QuizzmateMsgThreadMembers = new QuizzmateMsgThreadMemberRepository(this._context);
                return this._QuizzmateMsgThreadMembers;
            }
        }

        private QuizzmateMsg1Repository _QuizzmateMsg1s = null;
        public QuizzmateMsg1Repository QuizzmateMsg1s
        {
            get
            {
                if (this._QuizzmateMsg1s == null)
                    this._QuizzmateMsg1s = new QuizzmateMsg1Repository(this._context);
                return this._QuizzmateMsg1s;
            }
        }
        #endregion

        #region Classrooms
        private QuizzClassRepository _quizzClasss = null;
        public QuizzClassRepository QuizzClasses
        {
            get
            {
                if (this._quizzClasss == null)
                    this._quizzClasss = new QuizzClassRepository(this._context);
                return this._quizzClasss;
            }
        }

        private QuizzClassCommentRepository _quizzClassComments = null;
        public QuizzClassCommentRepository QuizzClassComments
        {
            get
            {
                if (this._quizzClassComments == null)
                    this._quizzClassComments = new QuizzClassCommentRepository(this._context);
                return this._quizzClassComments;
            }
        }

        private QuizzClassAnnouncementRepository _quizzClassAnnouncements = null;
        public QuizzClassAnnouncementRepository QuizzClassAnnouncements
        {
            get
            {
                if (this._quizzClassAnnouncements == null)
                    this._quizzClassAnnouncements = new QuizzClassAnnouncementRepository(this._context);
                return this._quizzClassAnnouncements;
            }
        }

        private QuizzClassQuizzRepository _quizzClassQuizzes = null;
        public QuizzClassQuizzRepository QuizzClassQuizzes
        {
            get
            {
                if (this._quizzClassQuizzes == null)
                    this._quizzClassQuizzes = new QuizzClassQuizzRepository(this._context);
                return this._quizzClassQuizzes;
            }
        }

        private QuizzClassLessonRepository _quizzClassLessons = null;
        public QuizzClassLessonRepository QuizzClassLessons
        {
            get
            {
                if (this._quizzClassLessons == null)
                    this._quizzClassLessons = new QuizzClassLessonRepository(this._context);
                return this._quizzClassLessons;
            }
        }

        private QuizzClassLessonMessageRepository _quizzClassLessonMessages = null;
        public QuizzClassLessonMessageRepository QuizzClassLessonMessages
        {
            get
            {
                if (this._quizzClassLessonMessages == null)
                    this._quizzClassLessonMessages = new QuizzClassLessonMessageRepository(this._context);
                return this._quizzClassLessonMessages;
            }
        }

        private QuizzClassLessonCommentRepository _quizzClassLessonComments = null;
        public QuizzClassLessonCommentRepository QuizzClassLessonComments
        {
            get
            {
                if (this._quizzClassLessonComments == null)
                    this._quizzClassLessonComments = new QuizzClassLessonCommentRepository(this._context);
                return this._quizzClassLessonComments;
            }
        }

        private QuizzClassLessonQuizzRepository _quizzClassLessonQuizzes = null;
        public QuizzClassLessonQuizzRepository QuizzClassLessonQuizzes
        {
            get
            {
                if (this._quizzClassLessonQuizzes == null)
                    this._quizzClassLessonQuizzes = new QuizzClassLessonQuizzRepository(this._context);
                return this._quizzClassLessonQuizzes;
            }
        }

        private QuizzClassMemberRepository _quizzClassMembers = null;
        public QuizzClassMemberRepository QuizzClassMembers
        {
            get
            {
                if (this._quizzClassMembers == null)
                    this._quizzClassMembers = new QuizzClassMemberRepository(this._context);
                return this._quizzClassMembers;
            }
        }

        private QuizzClassJoinRequestRepository _quizzClassJoinRequests = null;
        public QuizzClassJoinRequestRepository QuizzClassJoinRequests
        {
            get
            {
                if (this._quizzClassJoinRequests == null)
                    this._quizzClassJoinRequests = new QuizzClassJoinRequestRepository(this._context);
                return this._quizzClassJoinRequests;
            }
        }

        private QuizzClassInviteRequestRepository _QuizzClassInviteRequests = null;
        public QuizzClassInviteRequestRepository QuizzClassInviteRequests
        {
            get
            {
                if (this._QuizzClassInviteRequests == null)
                    this._QuizzClassInviteRequests = new QuizzClassInviteRequestRepository(this._context);
                return this._QuizzClassInviteRequests;
            }
        }

        private QuizzClassMaterialRepository _quizzClassMaterials = null;
        public QuizzClassMaterialRepository QuizzClassMaterials
        {
            get
            {
                if (this._quizzClassMaterials == null)
                    this._quizzClassMaterials = new QuizzClassMaterialRepository(this._context);
                return this._quizzClassMaterials;
            }
        }

        private QuizzClassChatRepository _quizzClassChats = null;
        public QuizzClassChatRepository QuizzClassChats
        {
            get
            {
                if (this._quizzClassChats == null)
                    this._quizzClassChats = new QuizzClassChatRepository(this._context);
                return this._quizzClassChats;
            }
        }
        #endregion

        public void SaveChanges()
        {
            foreach (DbEntityEntry item in _context.ChangeTracker.Entries())
            {
                ITrackableModel model = item.Entity as ITrackableModel;
                if (model != null)
                {
                    if (item.State == System.Data.Entity.EntityState.Added)
                    {
                        model.Created = DateTime.UtcNow;
                    }
                    else
                    {
                        _context.Entry(model).Property(x => x.Created).IsModified = false;
                    }
                    model.Modified = DateTime.UtcNow;
                }
            }

            this._context.SaveChanges();
        }

        public void Dispose()
        {
            if (this._context != null)
            {
                this._context.Dispose();
            }
        }
    }
}
