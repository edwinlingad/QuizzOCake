using L2L.WebApi.Controllers;
using L2L.WebApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Services
{
    public class ServiceContainer : BaseService
    {
        public ServiceContainer(BaseApiController controller)
            : base(controller)
        {
        }

        private UserService _userSvc;
        public UserService UserSvc
        {
            get
            {
                if (_userSvc == null)
                    _userSvc = new UserService(_controller);
                return _userSvc;
            }
        }

        private DependentService _dependentSvc;
        public DependentService DependentSvc
        {
            get
            {
                if (_dependentSvc == null)
                    _dependentSvc = new DependentService(_controller);
                return _dependentSvc;
            }
        } 

        private LayoutService _layoutSvc;
        public LayoutService LayoutSvc
        {
            get     
            {
                if (_layoutSvc == null)
                    _layoutSvc = new LayoutService(_controller);
                return _layoutSvc;
            }
        }

        private QuizzUpvoteService _quizzUpvoteSvc;
        public QuizzUpvoteService QuizzUpvoteSvc
        {
            get
            {
                if (_quizzUpvoteSvc == null)
                    _quizzUpvoteSvc = new QuizzUpvoteService(_controller);
                return _quizzUpvoteSvc;
            }
        }

        #region Quizz
        private QuestionTypeService _questionTypeSvc;
        public QuestionTypeService QuestionTypeSvc
        {
            get
            {
                if (_questionTypeSvc == null)
                    _questionTypeSvc = new QuestionTypeService(_controller);
                return _questionTypeSvc;
            }
        } 
        private TestService _testSvc;
        public TestService TestSvc
        {
            get
            {
                if (_testSvc == null)
                    _testSvc = new TestService(_controller);
                return _testSvc;
            }
        }

        private QuestionService _questionSvc;
        public QuestionService QuestionSvc
        {
            get
            {
                if (_questionSvc == null)
                    _questionSvc = new QuestionService(_controller);
                return _questionSvc;
            }
        }

        private TestSettingService _testSettingSvc;
        public TestSettingService TestSettingSvc
        {
            get
            {
                if (_testSettingSvc == null)
                    _testSettingSvc = new TestSettingService(_controller);
                return _testSettingSvc;
            }
        } 

        private QuizzCategoryService _quizzCategorySvc;
        public QuizzCategoryService QuizzCategorySvc
        {
            get
            {
                if (_quizzCategorySvc == null)
                    _quizzCategorySvc = new QuizzCategoryService(_controller);
                return _quizzCategorySvc;
            }
        }

        private QuizzService _quizzSvc;
        public QuizzService QuizzSvc
        {
            get
            {
                if (_quizzSvc == null)
                    _quizzSvc = new QuizzService(_controller);
                return _quizzSvc;
            }
        }

        private QuizzOverviewService _quizzOverviewSvc;
        public QuizzOverviewService QuizzOverviewSvc
        {
            get
            {
                if (_quizzOverviewSvc == null)
                    _quizzOverviewSvc = new QuizzOverviewService(_controller);
                return _quizzOverviewSvc;
            }
        }

        private QuizzCommentService _quizzCommentSvc;
        public QuizzCommentService QuizzCommentSvc
        {
            get
            {
                if (_quizzCommentSvc == null)
                    _quizzCommentSvc = new QuizzCommentService(_controller);
                return _quizzCommentSvc;
            }
        } 

        #region QandA
        private QAAnswerService _qaAnswerSvc;
        public QAAnswerService QAAnswerSvc
        {
            get
            {
                if (_qaAnswerSvc == null)
                    _qaAnswerSvc = new QAAnswerService(_controller);
                return _qaAnswerSvc;
            }
        }

        private QAQuestionService _qaQuestionSvc;
        public QAQuestionService QAQuestionSvc
        {
            get
            {
                if (_qaQuestionSvc == null)
                    _qaQuestionSvc = new QAQuestionService(_controller);
                return _qaQuestionSvc;
            }
        } 
        #endregion
        #region MultipleChoice
        private MCQuestionService _mcQuestionSvc;
        public MCQuestionService MCQuestionSvc
        {
            get
            {
                if (_mcQuestionSvc == null)
                    _mcQuestionSvc = new MCQuestionService(_controller);
                return _mcQuestionSvc;
            }
        }

        private MCChoiceService _mcChoiceSvc;
        public MCChoiceService MCChoiceSvc
        {
            get
            {
                if (_mcChoiceSvc == null)
                    _mcChoiceSvc = new MCChoiceService(_controller);
                return _mcChoiceSvc;
            }
        } 
        #endregion

        #region MultiChoiceSame
        private MultiChoiceSameChoiceService _multiChoiceSameChoiceSvc;
        public MultiChoiceSameChoiceService MultiChoiceSameChoiceSvc
        {
            get
            {
                if (_multiChoiceSameChoiceSvc == null)
                    _multiChoiceSameChoiceSvc = new MultiChoiceSameChoiceService(_controller);
                return _multiChoiceSameChoiceSvc;
            }
        }

        private MultiChoiceSameChoiceGroupService _multiChoiceSameChoiceGroupSvc;
        public MultiChoiceSameChoiceGroupService MultiChoiceSameChoiceGroupSvc
        {
            get
            {
                if (_multiChoiceSameChoiceGroupSvc == null)
                    _multiChoiceSameChoiceGroupSvc = new MultiChoiceSameChoiceGroupService(_controller);
                return _multiChoiceSameChoiceGroupSvc;
            }
        }

        private MultiChoiceSameQuestionService _multiChoiceSameQuestionSvc;
        public MultiChoiceSameQuestionService MultiChoiceSameQuestionSvc
        {
            get
            {
                if (_multiChoiceSameQuestionSvc == null)
                    _multiChoiceSameQuestionSvc = new MultiChoiceSameQuestionService(_controller);
                return _multiChoiceSameQuestionSvc;
            }
        } 
        #endregion

        #region Reviewer

        private QuickNoteService _quickNoteSvc;
        public QuickNoteService QuickNoteSvc
        {
            get
            {
                if (_quickNoteSvc == null)
                    _quickNoteSvc = new QuickNoteService(_controller);
                return _quickNoteSvc;
            }
        }

        private TextFlashCardService _textFlashCardSvc;
        public TextFlashCardService TextFlashCardSvc
        {
            get
            {
                if (_textFlashCardSvc == null)
                    _textFlashCardSvc = new TextFlashCardService(_controller);
                return _textFlashCardSvc;
            }
        }

        private ReviewerFromQuestionsService _reviewerFromQuestionsSvc;
        public ReviewerFromQuestionsService ReviewerFromQuestionsSvc
        {
            get
            {
                if (_reviewerFromQuestionsSvc == null)
                    _reviewerFromQuestionsSvc = new ReviewerFromQuestionsService(_controller);
                return _reviewerFromQuestionsSvc;
            }
        } 
        #endregion
        #endregion

        #region TestLog
        private TestLogService _testLogSvc;
        public TestLogService TestLogSvc
        {
            get
            {
                if (_testLogSvc == null)
                    _testLogSvc = new TestLogService(_controller);
                return _testLogSvc;
            }
        } 
        #endregion

        #region Notification
        private NotificationService _notificationSvc;
        public NotificationService NotificationSvc
        {
            get
            {
                if (_notificationSvc == null)
                    _notificationSvc = new NotificationService(_controller);
                return _notificationSvc;
            }
        }

        private NotificationTypeService _notificationTypeSvc;
        public NotificationTypeService NotificationTypesSvc
        {
            get
            {
                if (_notificationTypeSvc == null)
                    _notificationTypeSvc = new NotificationTypeService(_controller);
                return _notificationTypeSvc;
            }
        }

        private DeleteNotificationService _deleteNotificationSvc;
        public DeleteNotificationService DeleteNotificationSvc
        {
            get
            {
                if (_deleteNotificationSvc == null)
                    _deleteNotificationSvc = new DeleteNotificationService(_controller);
                return _deleteNotificationSvc;
            }
        } 
        #endregion

        #region Activity 
        private ActivityService _activitySvc;
        public ActivityService ActivitySvc
        {
            get
            {
                if (_activitySvc == null)
                    _activitySvc = new ActivityService(_controller);
                return _activitySvc;
            }
        } 
        #endregion

        #region QuizzRating
        private QuizzCurrentUserRatingService _quizzUserRatingSvc;
        public QuizzCurrentUserRatingService QuizzUserRatingSvc
        {
            get
            {
                if (_quizzUserRatingSvc == null)
                    _quizzUserRatingSvc = new QuizzCurrentUserRatingService(_controller);
                return _quizzUserRatingSvc;
            }
        }
        #endregion

        #region QuizzPoints
        private DailyRewardService _dailyRewardSvc;
        public DailyRewardService DailyRewardSvc
        {
            get
            {
                if (_dailyRewardSvc == null)
                    _dailyRewardSvc = new DailyRewardService(_controller);
                return _dailyRewardSvc;
            }
        }

        private QuizzPointsService _quizzPointSvc;
        public QuizzPointsService QuizzPointsSvc
        {
            get
            {
                if (_quizzPointSvc == null)
                    _quizzPointSvc = new QuizzPointsService(_controller);
                return _quizzPointSvc;
            }
        }
        #endregion

        #region Relationships
        private RelationshipNotificationService _relationshipNotificationSvc;
        public RelationshipNotificationService RelationshipNotificationSvc
        {
            get
            {
                if (_relationshipNotificationSvc == null)
                    _relationshipNotificationSvc = new RelationshipNotificationService(_controller);
                return _relationshipNotificationSvc;
            }
        }

        private NewRelationshipNotificationService _newRelationshipNotificationSvc;
        public NewRelationshipNotificationService NewRelationshipNotificationSvc
        {
            get
            {
                if (_newRelationshipNotificationSvc == null)
                    _newRelationshipNotificationSvc = new NewRelationshipNotificationService(_controller);
                return _newRelationshipNotificationSvc;
            }
        }
        #endregion

        #region Search
        private SearchService _searchSvc;
        public SearchService SearchSvc
        {
            get
            {
                if (_searchSvc == null)
                    _searchSvc = new SearchService(_controller);
                return _searchSvc;
            }
        }
        #endregion

        #region QuizzClass
        private QuizzClassMemberUpdateService _quizzClassMemberUpdateSvc;
        public QuizzClassMemberUpdateService QuizzClassMemberUpdateSvc
        {
            get
            {
                if (_quizzClassMemberUpdateSvc == null)
                    _quizzClassMemberUpdateSvc = new QuizzClassMemberUpdateService(_controller);
                return _quizzClassMemberUpdateSvc;
            }
        }

        private QuizzClassAnnouncementService _quizzClassAnnouncementSvc;
        public QuizzClassAnnouncementService QuizzClassAnnouncementSvc
        {
            get
            {
                if (_quizzClassAnnouncementSvc == null)
                    _quizzClassAnnouncementSvc = new QuizzClassAnnouncementService(_controller);
                return _quizzClassAnnouncementSvc;
            }
        }

        private QuizzClassJoinService _quizzClassJoinSvc;
        public QuizzClassJoinService QuizzClassJoinSvc
        {
            get
            {
                if (_quizzClassJoinSvc == null)
                    _quizzClassJoinSvc = new QuizzClassJoinService(_controller);
                return _quizzClassJoinSvc;
            }
        }

        private QuizzClassInviteService _quizzClassInviteSvc;
        public QuizzClassInviteService QuizzClassInviteSvc
        {
            get
            {
                if (_quizzClassInviteSvc == null)
                    _quizzClassInviteSvc = new QuizzClassInviteService(_controller);
                return _quizzClassInviteSvc;
            }
        }

        private QuizzmateMsgThreadMemberService _quizzmateMsgThreadMemberSvc;
        public QuizzmateMsgThreadMemberService QuizzmateMsgThreadMemberSvc
        {
            get
            {
                if (_quizzmateMsgThreadMemberSvc == null)
                    _quizzmateMsgThreadMemberSvc = new QuizzmateMsgThreadMemberService(_controller);
                return _quizzmateMsgThreadMemberSvc;
            }
        }
        #endregion

        #region QuizzConnect 
        private QuizzConnectMsgThreadService _quizzConnectMsgThreadSvc;
        public QuizzConnectMsgThreadService QuizzConnectMsgThreadSvc
        {
            get
            {
                if (_quizzConnectMsgThreadSvc == null)
                    _quizzConnectMsgThreadSvc = new QuizzConnectMsgThreadService(_controller);
                return _quizzConnectMsgThreadSvc;
            }
        } 
        #endregion

        private EmailService _emailSvc;
        public EmailService EmailSvc
        {
            get
            {
                if (_emailSvc == null)
                    _emailSvc = new EmailService(_controller);
                return _emailSvc;
            }
        } 

        private LoggingService _loggingSvc;
        public LoggingService LoggingSvc
        {
            get
            {
                if (_loggingSvc == null)
                    _loggingSvc = new LoggingService(_controller);
                return _loggingSvc;
            }
        } 
    }
}