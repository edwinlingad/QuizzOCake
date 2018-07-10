using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.Services;

namespace L2L.WebApi.Controllers
{
    // RESOURCE-FACTORY
    public enum ResourceTypeEnum
    {
        QuizzCurrentUserRating,
        QuizzUserRating,
        AssignmentGroup,
        Assignment,
        TestSnapshot,
        QuizzCategories,
        QuizzerInfo,
        BadgeList,
        QuizzmateRequest,
        QuizzlingRequest,
        RelationshipNotification,
        Quizzmates,
        QuizzmateMsgThread,
        QuizzmateMsgThreadMember,
        QuizzmateMsg,
        QuizzConnectMsgThread,
        QuizzClass,
        QuizzClassAnnouncement,
        QuizzClassComment,
        QuizzClassLesson,
        QuizzClassLessonMessage,
        QuizzClassLessonComment,
        QuizzClassJoinRequest,
        QuizzClassMember,
        QuizzClassInviteRequest,
        QuizzClassMemberInvite,
        QuizzClassLessonQuizz,
        QuizzClassQuizz,
        Layout,
        QuizzComment,
        QuizzQuickNote,
        FlashCards,
        ReviewerFromQuestions,
        TestLog
    }

    public interface IResource
    {
        object GetMany(int id, int id2, int id3, int id4, int id5);
        object GetManyAlt(int id, int id2, int id3, string str, string str2, string str3);
        object GetAlt(string str);
        object Get(int id);
        object Post(object modelParam);
        bool Patch(object modelParam);
        bool Delete(int id);
    }

    public class ResourceFactory
    {
        public ResourceFactory(BaseApiController controller)
        {
            _controller = controller;
        }

        public IResource GetResource(ResourceTypeEnum type)
        {
            IResource resource = null;

            if (_resourcesMap.TryGetValue(type, out resource) == true)
                return resource;

            switch (type)
            {
                case ResourceTypeEnum.QuizzCurrentUserRating:
                    resource = new QuizzCurrentUserRatingService(_controller);
                    break;
                case ResourceTypeEnum.QuizzUserRating:
                    resource = new QuizzUserRatingService(_controller);
                    break;
                case ResourceTypeEnum.AssignmentGroup:
                    resource = new AssignmentGroupService(_controller);
                    break;
                case ResourceTypeEnum.Assignment:
                    resource = new AssignmentService(_controller);
                    break;
                case ResourceTypeEnum.TestSnapshot:
                    resource = new TestSnapshotService(_controller);
                    break;
                case ResourceTypeEnum.QuizzCategories:
                    resource = new QuizzCategoryService(_controller);
                    break;
                case ResourceTypeEnum.QuizzerInfo:
                    resource = new QuizzerInfoService(_controller);
                    break;
                case ResourceTypeEnum.BadgeList:
                    resource = new BadgeService(_controller);
                    break;
                case ResourceTypeEnum.QuizzmateRequest:
                    resource = new QuizzmateRequestService(_controller);
                    break;
                case ResourceTypeEnum.QuizzlingRequest:
                    resource = new QuizzlingRequestService(_controller);
                    break;
                case ResourceTypeEnum.RelationshipNotification:
                    resource = new RelationshipNotificationService(_controller);
                    break;
                case ResourceTypeEnum.Quizzmates:
                    resource = new QuizzmatesService(_controller);
                    break;
                case ResourceTypeEnum.QuizzmateMsgThread:
                    resource = new QuizzmateMsgThreadService(_controller);
                    break;
                case ResourceTypeEnum.QuizzmateMsgThreadMember:
                    resource = new QuizzmateMsgThreadMemberService(_controller);
                    break;
                case ResourceTypeEnum.QuizzmateMsg:
                    resource = new QuizzmateMsgService(_controller);
                    break;
                case ResourceTypeEnum.QuizzConnectMsgThread:
                    resource = new QuizzConnectMsgThreadService(_controller);
                    break;
                case ResourceTypeEnum.QuizzClass:
                    resource = new QuizzClassService(_controller);
                    break;
                case ResourceTypeEnum.QuizzClassAnnouncement:
                    resource = new QuizzClassAnnouncementService(_controller);
                    break;
                case ResourceTypeEnum.QuizzClassComment:
                    resource = new QuizzClassCommentService(_controller);
                    break;
                case ResourceTypeEnum.QuizzClassLesson:
                    resource = new QuizzClassLessonService(_controller);
                    break;
                case ResourceTypeEnum.QuizzClassLessonMessage:
                    resource = new QuizzClassLessonMessageService(_controller);
                    break;
                case ResourceTypeEnum.QuizzClassLessonComment:
                    resource = new QuizzClassLessonCommentService(_controller);
                    break;
                case ResourceTypeEnum.QuizzClassJoinRequest:
                    resource = new QuizzClassJoinService(_controller);
                    break;
                case ResourceTypeEnum.QuizzClassMember:
                    resource = new QuizzClassMemberService(_controller);
                    break;
                case ResourceTypeEnum.QuizzClassInviteRequest:
                    resource = new QuizzClassInviteService(_controller);
                    break;
                case ResourceTypeEnum.QuizzClassMemberInvite:
                    resource = new QuizzClassMemberInviteService(_controller);
                    break;
                case ResourceTypeEnum.QuizzClassLessonQuizz:
                    resource = new QuizzClassLessonQuizzService(_controller);
                    break;
                case ResourceTypeEnum.QuizzClassQuizz:
                    resource = new QuizzClassQuizzService(_controller);
                    break;
                case ResourceTypeEnum.Layout:
                    resource = new LayoutService(_controller);
                    break;
                case ResourceTypeEnum.QuizzComment:
                    resource = new QuizzCommentService(_controller);
                    break;
                case ResourceTypeEnum.QuizzQuickNote:
                    resource = new QuickNoteService(_controller);
                    break;
                case ResourceTypeEnum.FlashCards:
                    resource = new TextFlashCardService(_controller);
                    break;
                case ResourceTypeEnum.ReviewerFromQuestions:
                    resource = new ReviewerFromQuestionsService(_controller);
                    break;
                case ResourceTypeEnum.TestLog:
                    resource = new TestLogService(_controller);
                    break;
                default:
                    break;
            }

            if (resource != null)
                _resourcesMap.Add(type, resource);

            return resource;
        }

        private BaseApiController _controller;
        private Dictionary<ResourceTypeEnum, IResource> _resourcesMap = new Dictionary<ResourceTypeEnum, IResource>();
    }
}