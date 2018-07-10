using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.Entities;
using L2L.WebApi.Models;
using AutoMapper;

namespace L2L.WebApi
{
    public static class MapConfig
    {
        public static void RegisterMaps()
        {
            AutoMapper.Mapper.Initialize(Configure);
        }

        private static void Configure(IConfiguration config)
        {
            ConfigureLayoutModels(config);
            ConfigureAccountMaps(config);
            ConfigureUserMaps(config);
            ConfigureQuizzMaps(config);
            ConfigureTestLogMaps(config);
            ConfigureActivityMaps(config);
            ConfigureQuizzRatingMaps(config);
            ConfigureAssignmentMaps(config);
            ConfigureNewNotificationMaps(config);
            ConfigureQuizzPointsMaps(config);
            ConfigureRelationshipMaps(config);
            ConfigureSearchMaps(config);
            ConfigureMessageMaps(config);
            ConfigureQuizzClassMaps(config);
        }

        private static void ConfigureLayoutModels(IConfiguration config)
        {
            config.CreateMap<QuizzModel, LayoutQuizzModel>().ReverseMap();
            config.CreateMap<Quizz, LayoutQuizzModel>().ReverseMap();
            config.CreateMap<TestLogModel, LayoutRecentQuizzModel>().ReverseMap();
            config.CreateMap<TestLog, LayoutRecentQuizzModel>().ReverseMap();
            config.CreateMap<Assignment, LayoutAssignmentModel>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.AssignmentGroup.Quizz.Category))
                .ForMember(dest => dest.QuizzName, opt => opt.MapFrom(src => src.AssignmentGroup.Quizz.Title));
            config.CreateMap<AssignmentGroup, LayoutAssignmentGroupModel>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Quizz.Category))
                .ForMember(dest => dest.QuizzName, opt => opt.MapFrom(src => src.Quizz.Title));
        }

        private static void ConfigureAccountMaps(IConfiguration config)
        {
            config.CreateMap<L2L.Entities.Profile, RegisterStandardModel>().ReverseMap();
            config.CreateMap<L2L.Entities.Profile, RegisterDependentModel>().ReverseMap();
            config.CreateMap<L2L.Entities.Profile, RegisterUserModelBase>().ReverseMap();
            config.CreateMap<L2L.Entities.Profile, RegisterExternalBindingModel>().ReverseMap();
        }

        private static void ConfigureUserMaps(IConfiguration config)
        {
            var parentId = 0;
            var userId = 0;
            config.CreateMap<User, User>();
            config.CreateMap<User, UserModel>().ReverseMap();
            config.CreateMap<L2L.Entities.Profile, ProfileModel>().ReverseMap();
            config.CreateMap<Dependent, DependentModel>()
                .ForMember(dest => dest.DependentFullName, opt => opt.MapFrom(src => src.Child.Profile.FirstName + " " + src.Child.Profile.LastName));
            config.CreateMap<DependentModel, Dependent>();
            config.CreateMap<DependentPermission, DependentPermissionModel>().ReverseMap();
            config.CreateMap<Dependent, DependentNotificationModel>().ReverseMap();

            config.CreateMap<User, DependentUserModel>()
                .ForMember(dest => dest.Profile, opt => opt.MapFrom(u => u.Profile))
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(u => u.DependentPermission))
                .ForMember(dest => dest.NotificationsSubscription, opt => opt.MapFrom(u => u.AsChildDependsOn.Where(d => d.ChildId == u.Id && d.UserId == parentId).FirstOrDefault()))
                .ForMember(dest => dest.IsCurrentUserPrimary, opt => opt.MapFrom(u => u.AsChildDependsOn.Where(d => d.ChildId == u.Id && d.UserId == parentId).FirstOrDefault().IsPrimary));

            config.CreateMap<User, QuizzerModel>()
                .ForMember(dest => dest.NumQuizzes, opt => opt.MapFrom(src => src.Quizzes.Where(q => q.IsLive == true).Count()))
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.Profile.FirstName + " " + src.Profile.LastName))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.Profile.BirthDate))
                .ForMember(dest => dest.IsQuizzmate, opt => opt.MapFrom(src =>
                        src.FriendsAsUser1.Select(f => f.User2Id).Contains(userId) ||
                        src.FriendsAsUser2.Select(f => f.User1Id).Contains(userId)))
                .ForMember(dest => dest.FriendRequestSentId, opt => opt.MapFrom(src =>
                        src.FriendRequestsTo.Where(dr => dr.IsDeleted == false && dr.RequestFromId == userId).Select(dr => dr.Id).FirstOrDefault()))
                .ForMember(dest => dest.IsFriendRequestSent, opt => opt.MapFrom(src =>
                        src.FriendRequestsTo.Where(dr => dr.IsDeleted == false).Select(dr => dr.RequestFromId).Contains(userId)))
                .ForMember(dest => dest.FriendRequestPendingId, opt => opt.MapFrom(src =>
                        src.FriendRequestsFrom.Where(dr => dr.IsDeleted == false && dr.RequestToId == userId).Select(dr => dr.Id).FirstOrDefault()))
                .ForMember(dest => dest.IsFriendRequestPending, opt => opt.MapFrom(src =>
                        src.FriendRequestsFrom.Where(dr => dr.IsDeleted == false).Select(dr => dr.RequestToId).Contains(userId)))
                ;
        }

        private static void ConfigureQuizzMaps(IConfiguration config)
        {
            var userId = 0;
            config.CreateMap<QuizzCategory, QuizzCategoryModel>()
                .ForMember(dest => dest.QuizzCategoryType, opt => opt.MapFrom(src => src.CategoryValue));
            config.CreateMap<QuizzCategoryModel, QuizzCategory>()
                .ForMember(dest => dest.CategoryValue, opt => opt.MapFrom(src => src.QuizzCategoryType));
            config.CreateMap<Quizz, QuizzModel>()
                .ForMember(dest => dest.TestId, opt => opt.MapFrom(src => src.Tests.FirstOrDefault().Id))
                .ForMember(dest => dest.DefaultTestSetting, opt => opt.MapFrom(src => src.Tests.FirstOrDefault().DefaultSetting))
                .ReverseMap();
            config.CreateMap<Quizz, QuizzOverviewModel>()
                .ForMember(dest => dest.NumQuestions, opt => opt.MapFrom(src => src.Tests.FirstOrDefault().Questions.Count))
                .ForMember(dest => dest.NumQuickNotes, opt => opt.MapFrom(src => src.Reviewers.FirstOrDefault().QuickNotes.Count))
                .ForMember(dest => dest.NumLikes, opt => opt.MapFrom(src => src.QuizRating.QuizUpvotes.Count))
                .ForMember(dest => dest.NumTaken, opt => opt.MapFrom(src => src.TestLogs.Count))
                .ForMember(dest => dest.OwnerUserName, opt => opt.MapFrom(src => src.Owner.UserName))
                .ForMember(dest => dest.OwnerFullName, opt => opt.MapFrom(src => src.Owner.Profile.FirstName + " " + src.Owner.Profile.LastName))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.Owner.Profile.BirthDate))
                .ForMember(dest => dest.IsQuizzmate, opt => opt.MapFrom(src =>
                        src.Owner.FriendsAsUser1.Select(f => f.User2Id).Contains(userId) ||
                        src.Owner.FriendsAsUser2.Select(f => f.User1Id).Contains(userId)))
                .ForMember(dest => dest.OwnerPoints, opt => opt.MapFrom(src => src.Owner.Points))
                .ForMember(dest => dest.TotalBadge, opt => opt.MapFrom(src => src.Owner.TotalBadge))
                .ForMember(dest => dest.ReviewerId, opt => opt.MapFrom(src => src.Reviewers.FirstOrDefault().Id))
                .ForMember(dest => dest.TestId, opt => opt.MapFrom(src => src.Tests.FirstOrDefault().Id))
                .ForMember(dest => dest.IsBookmarked, opt => opt.UseValue(false))
                .ForMember(dest => dest.IsLiked, opt => opt.MapFrom(src => src.QuizRating.QuizUpvotes.Where(r => r.UserId == userId && r.UpVote == 1).Count() == 1))
                .ForMember(dest => dest.QuizzRatingId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NumComments, opt => opt.MapFrom(src => src.Comments.Where(c => c.IsDeleted == false).Count()));

            config.CreateMap<Quizz, AssignmentQuizzModel>()
                .ForMember(dest => dest.NumQuestions, opt => opt.MapFrom(src => src.Tests.FirstOrDefault().Questions.Count))
                .ForMember(dest => dest.NumQuickNotes, opt => opt.MapFrom(src => src.Reviewers.FirstOrDefault().QuickNotes.Count))
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.UserName))
                .ForMember(dest => dest.ReviewerId, opt => opt.MapFrom(src => src.Reviewers.FirstOrDefault().Id))
                .ForMember(dest => dest.TestId, opt => opt.MapFrom(src => src.Tests.FirstOrDefault().Id));


            config.CreateMap<Test, TestModel>().ReverseMap();
            config.CreateMap<TestSnapshot, LayoutTestSnapshot>().ReverseMap();
            config.CreateMap<Question, QuestionModel>().ReverseMap();
            config.CreateMap<TestSetting, TestSettingModel>().ReverseMap();

            ConfigureQandAMaps(config);
            ConfigureMultipleChoiceMaps(config);
            ConfigureMultiChoiceSameMaps(config);
            ConfigureTakeTestMaps(config);
            ConfigureReviewerMaps(config);

            ConfigureQuizzCommentMaps(config);
        }

        private static void ConfigureQandAMaps(IConfiguration config)
        {
            config.CreateMap<QandAQuestion, QAQuestionModel>().ReverseMap();
            config.CreateMap<QandAAnswer, QAAnswerModel>().ReverseMap();
        }

        private static void ConfigureMultipleChoiceMaps(IConfiguration config)
        {
            config.CreateMap<MultipleChoiceQuestion, MCQuestionModel>().ReverseMap();
            config.CreateMap<MultipleChoiceChoice, MChoiceModel>().ReverseMap();
        }

        private static void ConfigureMultiChoiceSameMaps(IConfiguration config)
        {
            config.CreateMap<MultiChoiceSameQuestion, MultiChoiceSameQuestionModel>().ReverseMap();
            config.CreateMap<MultiChoiceSameChoiceGroup, MultiChoiceSameChoiceGroupModel>().ReverseMap();
            config.CreateMap<MultiChoiceSameChoice, MultiChoiceSameChoiceModel>().ReverseMap();
        }

        private static void ConfigureTakeTestMaps(IConfiguration config)
        {
            config.CreateMap<Test, TakeTestModel>().ReverseMap();
        }

        private static void ConfigureReviewerMaps(IConfiguration config)
        {
            config.CreateMap<TextFlashCard, TextFlashCardModel>().ReverseMap();
            config.CreateMap<QuickNote, QuickNoteModel>().ReverseMap();
            config.CreateMap<QuickNote, QuickNoteSimpleModel>().ReverseMap();
        }

        private static void ConfigureQuizzCommentMaps(IConfiguration config)
        {
            int userId = 0;
            config.CreateMap<QuizzComment, QuizzCommentModel>()
                .ForMember(dest => dest.AuthorUserName, opt => opt.MapFrom(src => src.Author.UserName))
                .ForMember(dest => dest.AuthorFullName, opt => opt.MapFrom(src => src.Author.Profile.FirstName + " " + src.Author.Profile.LastName))
                .ForMember(dest => dest.AuthorProfilePixUrl, opt => opt.MapFrom(src => src.Author.Profile.ProfileImageUrl))
                .ForMember(dest => dest.NumLikes, opt => opt.MapFrom(src => src.Likes.Where(l => l.UpVote == 1).Count()))
                .ForMember(dest => dest.IsLiked, opt => opt.MapFrom(src => src.Likes.Where(l => l.AuthorId == userId && l.UpVote == 1).Count() == 1))
                .ForMember(dest => dest.NumFlags, opt => opt.MapFrom(src => src.Flags.Where(f => f.UpVote == 1).Count()))
                .ForMember(dest => dest.IsFlagged, opt => opt.MapFrom(src => src.Flags.Where(f => f.AuthorId == userId).Count() == 1))
                .ForMember(dest => dest.IsQuizzmate, opt => opt.MapFrom(src =>
                        src.Author.FriendsAsUser1.Select(f => f.User2Id).Contains(userId) ||
                        src.Author.FriendsAsUser2.Select(f => f.User1Id).Contains(userId)))
                .ReverseMap();
        }

        private static void ConfigureTestLogMaps(IConfiguration config)
        {
            int userId = 0;
            config.CreateMap<TestLog, TestLogModel>()
                .ForMember(dest => dest.IsAuthor, opt => opt.MapFrom(src => src.Quizz.OwnerId == userId))
                .ForMember(dest => dest.AuthorUserName, opt => opt.MapFrom(src => src.Quizz.Owner.UserName))
                .ForMember(dest => dest.AuthorFullName, opt => opt.MapFrom(src => src.Quizz.Owner.Profile.FirstName + " " + src.Quizz.Owner.Profile.LastName))
                .ForMember(dest => dest.IsQuizzmate, opt => opt.MapFrom(src =>
                        src.Quizz.Owner.FriendsAsUser1.Select(f => f.User2Id).Contains(userId) ||
                        src.Quizz.Owner.FriendsAsUser2.Select(f => f.User1Id).Contains(userId)))
                .ReverseMap();
            config.CreateMap<TestLog, TestLogSummary>().ReverseMap();
        }

        private static void ConfigureActivityMaps(IConfiguration config)
        {
            config.CreateMap<Activity, ActivityModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Owner.UserName))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Owner.Profile.FirstName + " " + src.Owner.Profile.LastName))
                .ForMember(dest => dest.QuizzTitle, opt => opt.MapFrom(src => src.Quizz.Title))
                .ForMember(dest => dest.QuizzComment, opt => opt.MapFrom(src => src.QuizzComment.Comment))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.TestLog.Score))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.TestLog.Total));

        }

        private static void ConfigureQuizzRatingMaps(IConfiguration config)
        {
            int userId = 0;
            config.CreateMap<QuizzRating, QuizzCurrentUserRatingModel>()
                .ForMember(dest => dest.QuizzRatingId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.QuizUserRatings.Where(qr => qr.UserId == userId)
                            .Select(qr => qr.Rating).DefaultIfEmpty(0).FirstOrDefault()));

            config.CreateMap<QuizzRating, QuizzUserRatingModel>()
                .ForMember(dest => dest.QuizzRatingId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NumRatings, opt => opt.MapFrom(src => src.QuizUserRatings.Count()))
                .ForMember(dest => dest.RatingAvg, opt => opt.MapFrom(src => src.QuizUserRatings.Average(x => x.Rating)));
        }

        private static void ConfigureAssignmentMaps(IConfiguration config)
        {
            config.CreateMap<Assignment, AssignmentModel>().ReverseMap();
            config.CreateMap<Assignment, AssignmentInfo>()
                .ForMember(dest => dest.DependentFullName, opt => opt.MapFrom(src => src.Dependent.Profile.FirstName + " " + src.Dependent.Profile.LastName));
            config.CreateMap<AssignmentInfo, Assignment>();
            config.CreateMap<AssignmentGroup, AssignmentGroupModel>()
                .ForMember(dest => dest.AssignedByFullName, opt => opt.MapFrom(src => src.AssignedBy.Profile.FirstName + " " + src.AssignedBy.Profile.LastName));
            config.CreateMap<AssignmentGroupModel, AssignmentGroup>();
        }

        private static void ConfigureNewNotificationMaps(IConfiguration config)
        {
            config.CreateMap<NewNotification, NewNotificationModel>()
                .ForMember(dest => dest.FromUserName, opt => opt.MapFrom(src => src.FromUser.UserName))
                .ForMember(dest => dest.QuizzOwnerUserName, opt => opt.MapFrom(src => src.Quizz.Owner.UserName))
                .ForMember(dest => dest.QuizzCommentOwnerUserName, opt => opt.MapFrom(src => src.QuizzComment.Author.UserName))
                .ForMember(dest => dest.QuizzTitle, opt => opt.MapFrom(src => src.Quizz.Title))
                .ForMember(dest => dest.AssignmentScore, opt => opt.MapFrom(src => src.Assignment.CompletedScore))
                .ForMember(dest => dest.QuizzCommentValue, opt => opt.MapFrom(src => src.QuizzComment.Comment))
                .ForMember(dest => dest.ToQuizzmateUserId, opt => opt.MapFrom(src => src.FriendRequest.RequestToId))
                .ForMember(dest => dest.ToQuizzmateUserName, opt => opt.MapFrom(src => src.FriendRequest.RequestTo.UserName))
                .ForMember(dest => dest.ToQuizzmateUserFullName, opt => opt.MapFrom(src => src.FriendRequest.RequestTo.Profile.FirstName + " " + src.FriendRequest.RequestTo.Profile.LastName))
                .ForMember(dest => dest.FromQuizzmateUserId, opt => opt.MapFrom(src => src.FriendRequest.RequestFromId))
                .ForMember(dest => dest.FromQuizzmateUserName, opt => opt.MapFrom(src => src.FriendRequest.RequestFrom.UserName))
                .ForMember(dest => dest.FromQuizzmateUserFullName, opt => opt.MapFrom(src => src.FriendRequest.RequestFrom.Profile.FirstName + " " + src.FriendRequest.RequestFrom.Profile.LastName))
                .ForMember(dest => dest.QuizzmateRequestMessage, opt => opt.MapFrom(src => src.FriendRequest.Message))
                .ForMember(dest => dest.ToUnQuizzmateUserName, opt => opt.MapFrom(src => src.ToUnQuizzmate.UserName))
                .ForMember(dest => dest.ToUnQUizzmateUserFullName, opt => opt.MapFrom(src => src.ToUnQuizzmate.Profile.FirstName + " " + src.ToUnQuizzmate.Profile.LastName))
                ;
            config.CreateMap<NewNotificationModel, NewNotification>();
        }

        private static void ConfigureQuizzPointsMaps(IConfiguration config)
        {
            config.CreateMap<DailyRewardPerUser, DailyRewardModel>()
                .ForMember(dest => dest.DailyRewardPerUserId, opt => opt.MapFrom(src => src.Id));
        }

        private static void ConfigureRelationshipMaps(IConfiguration config)
        {
            ConfigureFriendMaps(config);
            ConfigureDependentMaps(config);
            ConfigureRelationshipNotificationMaps(config);

        }

        private static void ConfigureFriendMaps(IConfiguration config)
        {
            config.CreateMap<FriendRequest, FriendRequestModel>().ReverseMap();
        }

        private static void ConfigureDependentMaps(IConfiguration config)
        {
            config.CreateMap<DependentRequestFromUser, DependentRequestFromUserModel>().ReverseMap();
        }

        private static void ConfigureRelationshipNotificationMaps(IConfiguration config)
        {
            config.CreateMap<RelationshipNotification, RelationshipNotificationModel>()
                .ForMember(dest => dest.ToUserName, opt => opt.MapFrom(src => src.ToUser.UserName))
                .ForMember(dest => dest.ToUserFullName, opt => opt.MapFrom(src => src.ToUser.Profile.FirstName + " " + src.ToUser.Profile.LastName))
                .ForMember(dest => dest.FromUserName, opt => opt.MapFrom(src => src.FromUser.UserName))
                .ForMember(dest => dest.FromUserFullName, opt => opt.MapFrom(src => src.FromUser.Profile.FirstName + " " + src.FromUser.Profile.LastName))
                .ForMember(dest => dest.QuizzmateRequestMessage, opt => opt.MapFrom(src => src.FriendRequest.Message))
                .ReverseMap();
        }

        private static void ConfigureSearchMaps(IConfiguration config)
        {
            var userId = 0;
            config.CreateMap<User, SearchModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.Profile.BirthDate))
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.Profile.FirstName + " " + src.Profile.LastName))
                .ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom(src => src.Profile.ProfileImageUrl))
                .ForMember(dest => dest.IsQuizzmate, opt => opt.MapFrom(src =>
                        src.FriendsAsUser1.Select(f => f.User2Id).Contains(userId) ||
                        src.FriendsAsUser2.Select(f => f.User1Id).Contains(userId)))
                .ReverseMap();

            config.CreateMap<Quizz, SearchModel>()
                .ForMember(dest => dest.QuizzId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.Owner.Profile.BirthDate))
                .ForMember(dest => dest.QuizzCategory, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.QuizzName, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.QuizzDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.QuizzAuthorId, opt => opt.MapFrom(src => src.OwnerId))
                .ForMember(dest => dest.QuizzAuthorUserName, opt => opt.MapFrom(src => src.Owner.UserName))
                .ForMember(dest => dest.QuizzAuthorFullName, opt => opt.MapFrom(src => src.Owner.Profile.FirstName + " " + src.Owner.Profile.LastName))
                .ForMember(dest => dest.IsQuizzmate, opt => opt.MapFrom(src =>
                        src.Owner.FriendsAsUser1.Select(f => f.User2Id).Contains(userId) ||
                        src.Owner.FriendsAsUser2.Select(f => f.User1Id).Contains(userId)))
                .ReverseMap();

            config.CreateMap<QuizzClass, SearchModel>()
                .ForMember(dest => dest.QuizzClassId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.TeacherId))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.Teacher.Profile.BirthDate))
                .ForMember(dest => dest.IsMember, opt => opt.MapFrom(src => src.Students.Where(s =>
                        s.StudentId == userId).FirstOrDefault() != null))
                .ForMember(dest => dest.IsRequestSent, opt => opt.MapFrom(src =>
                        src.JoinRequests.Where(jr => jr.UserId == userId && jr.IsDeleted == false).FirstOrDefault() != null))
                .ForMember(dest => dest.IsInviteSent, opt => opt.MapFrom(src =>
                        src.InviteRequests.Where(ir => ir.UserId == userId && ir.IsDeleted == false).FirstOrDefault() != null))
                .ForMember(dest => dest.TeacherUserName, opt => opt.MapFrom(src => src.Teacher.UserName))
                .ForMember(dest => dest.TeacherFullName, opt => opt.MapFrom(src => src.Teacher.Profile.FirstName + " " + src.Teacher.Profile.LastName))
                .ForMember(dest => dest.QcTags, opt => opt.MapFrom(src => src.Tags))
                .ForMember(dest => dest.IsQuizzmate, opt => opt.MapFrom(src =>
                        src.Teacher.FriendsAsUser1.Select(f => f.User2Id).Contains(userId) ||
                        src.Teacher.FriendsAsUser2.Select(f => f.User1Id).Contains(userId)));
        }

        private static void ConfigureMessageMaps(IConfiguration config)
        {
            config.CreateMap<QuizzmateMsgThread, QuizzmateMsgThreadModel>().ReverseMap();
            config.CreateMap<QuizzmateMsgThreadMember, QuizzmateMsgThreadMemberModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.Profile.FirstName + " " + src.User.Profile.LastName))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.User.Profile.BirthDate))
                .ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom(src => src.User.Profile.ProfileImageUrl))
                .ReverseMap();
            config.CreateMap<QuizzmateMsg1, QuizzmateMsgModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Author.UserName))
                .ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom(src => src.Author.Profile.ProfileImageUrl));
            config.CreateMap<QuizzmateMsg1, QuizzConnectMsgThread>()
                .ForMember(dest => dest.LastSenderUserName, opt => opt.MapFrom(src => src.Author.UserName))
                .ForMember(dest => dest.LastSenderProfileUrl, opt => opt.MapFrom(src => src.Author.Profile.ProfileImageUrl))
                .ForMember(dest => dest.LastSenderMsg, opt => opt.MapFrom(src => src.Message))
                .ForMember(dest => dest.LastMsgPostedDate, opt => opt.MapFrom(src => src.PostedDate));
        }

        private static void ConfigureQuizzClassMaps(IConfiguration config)
        {
            var userId = 0;
            int? depId = null;
            config.CreateMap<QuizzClass, QuizzClassModel>()
                .ForMember(dest => dest.IsTeacherQuizzmate, opt => opt.MapFrom(src =>
                        src.Teacher.FriendsAsUser1.Select(f => f.User2Id).Contains(userId) ||
                        src.Teacher.FriendsAsUser2.Select(f => f.User1Id).Contains(userId)))
                .ForMember(dest => dest.TeacherUserName, opt => opt.MapFrom(src => src.Teacher.UserName))
                .ForMember(dest => dest.TeacherFullName, opt => opt.MapFrom(src => src.Teacher.Profile.FirstName + " " + src.Teacher.Profile.LastName))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.Teacher.Profile.BirthDate))
                .ForMember(dest => dest.NumStudents, opt => opt.MapFrom(src => src.Students.Count()))
                .ForMember(dest => dest.NumMaterials, opt => opt.MapFrom(src => src.Materials.Count()))
                .ForMember(dest => dest.NumStudentJoinRequests, opt => opt.MapFrom(src =>
                        src.JoinRequests.Where(qcj => qcj.IsDeleted == false).Count()))
                .ForMember(dest => dest.NumNewInviteAccepts, opt => opt.MapFrom(src =>
                        src.Students.Where(s => s.IsNewInviteAccepted == true).Count()))
                .ForMember(dest => dest.IsRequestSent, opt => opt.MapFrom(src =>
                        src.JoinRequests.Where(jr => jr.UserId == userId && jr.IsDeleted == false).FirstOrDefault() != null))
                .ForMember(dest => dest.IsInviteSent, opt => opt.MapFrom(src =>
                        src.InviteRequests.Where(ir => ir.UserId == userId && ir.IsDeleted == false).FirstOrDefault() != null))
                .ForMember(dest => dest.IsMember, opt => opt.MapFrom(src => src.Students.Where(s =>
                        s.StudentId == userId).FirstOrDefault() != null))
                .ForMember(dest => dest.QuizzClassMemberId, opt => opt.MapFrom(src => src.Students
                        .Where(s => s.StudentId == userId && s.DependentId == depId).Select(s => s.Id).DefaultIfEmpty(0).FirstOrDefault()))
                .ForMember(dest => dest.NumComments, opt => opt.MapFrom(src => src.ClassComments.Where(c => c.IsDeleted == false).Count()))
                .ReverseMap();

            config.CreateMap<QuizzClassAnnouncement, QuizzClassAnnouncementModel>()
                .ReverseMap();

            config.CreateMap<QuizzClassComment, QuizzClassCommentModel>()
                .ForMember(dest => dest.AuthorUserName, opt => opt.MapFrom(src => src.Author.UserName))
                .ForMember(dest => dest.AuthorFullName, opt => opt.MapFrom(src => src.Author.Profile.FirstName + " " + src.Author.Profile.LastName))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.Author.Profile.BirthDate))
                .ForMember(dest => dest.AuthorProfilePixUrl, opt => opt.MapFrom(src => src.Author.Profile.ProfileImageUrl))
                .ForMember(dest => dest.IsQuizzmate, opt => opt.MapFrom(src =>
                        src.Author.FriendsAsUser1.Select(f => f.User2Id).Contains(userId) ||
                        src.Author.FriendsAsUser2.Select(f => f.User1Id).Contains(userId)))
                .ReverseMap();

            config.CreateMap<QuizzClassQuizz, QuizzClassQuizzModel>()
                .ReverseMap();

            // Quizz Class Lesson
            config.CreateMap<QuizzClassLesson, QuizzClassLessonModel>()
                .ForMember(dest => dest.QuizzClassName, opt => opt.MapFrom(src => src.QuizzClass.ClassName))
                .ForMember(dest => dest.IsTeacherQuizzmate, opt => opt.MapFrom(src =>
                        src.QuizzClass.Teacher.FriendsAsUser1.Select(f => f.User2Id).Contains(userId) ||
                        src.QuizzClass.Teacher.FriendsAsUser2.Select(f => f.User1Id).Contains(userId)))
                .ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.QuizzClass.TeacherId))
                .ForMember(dest => dest.TeacherUserName, opt => opt.MapFrom(src => src.QuizzClass.Teacher.UserName))
                .ForMember(dest => dest.TeacherFullName, opt => opt.MapFrom(src => src.QuizzClass.Teacher.Profile.FirstName + " " + src.QuizzClass.Teacher.Profile.LastName))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.QuizzClass.Teacher.Profile.BirthDate))
                .ForMember(dest => dest.NumComments, opt => opt.MapFrom(src => src.Comments.Where(c => c.IsDeleted == false)
                            .Count()))
                .ReverseMap();

            config.CreateMap<QuizzClassLessonMessage, QuizzClassLessonMessageModel>()
                .ForMember(dest => dest.QuizzClassId, opt => opt.MapFrom(src => src.QuizzClassLesson.QuizzClassId))
                .ForMember(dest => dest.QuizzClassLessonIdx, opt => opt.MapFrom(src => src.QuizzClassLesson.QuizzClassLesssonIdx))
                .ReverseMap();
            config.CreateMap<QuizzClassLessonComment, QuizzClassLessonCommentModel>()
                .ForMember(dest => dest.QuizzClassId, opt => opt.MapFrom(src => src.QuizzClassLesson.QuizzClassId))
                .ForMember(dest => dest.QuizzClassLessonIdx, opt => opt.MapFrom(src => src.QuizzClassLesson.QuizzClassLesssonIdx))
                .ForMember(dest => dest.AuthorUserName, opt => opt.MapFrom(src => src.Author.UserName))
                .ForMember(dest => dest.AuthorFullName, opt => opt.MapFrom(src => src.Author.Profile.FirstName + " " + src.Author.Profile.LastName))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.Author.Profile.BirthDate))
                .ForMember(dest => dest.AuthorProfilePixUrl, opt => opt.MapFrom(src => src.Author.Profile.ProfileImageUrl))
                .ForMember(dest => dest.IsQuizzmate, opt => opt.MapFrom(src =>
                        src.Author.FriendsAsUser1.Select(f => f.User2Id).Contains(userId) ||
                        src.Author.FriendsAsUser2.Select(f => f.User1Id).Contains(userId)))
                .ForMember(dest => dest.IsTeacher, opt => opt.MapFrom(src => src.AuthorId == src.QuizzClassLesson.QuizzClass.TeacherId))
                .ForMember(dest => dest.IsAuthor, opt => opt.MapFrom(src => src.AuthorId == userId))
                .ReverseMap();
            config.CreateMap<QuizzClassLessonQuizz, QuizzClassLessonQuizzModel>()
                .ReverseMap();

            //Quizz Class Member
            config.CreateMap<QuizzClassJoinRequest, QuizzClassJoinRequestModel>()
                .ForMember(dest => dest.UserUserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.Profile.FirstName + " " + src.User.Profile.LastName))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.User.Profile.BirthDate))
                .ForMember(dest => dest.UserProfilePixUrl, opt => opt.MapFrom(src => src.User.Profile.ProfileImageUrl))
                .ForMember(dest => dest.IsQuizzmate, opt => opt.MapFrom(src =>
                        src.User.FriendsAsUser1.Select(f => f.User2Id).Contains(userId) ||
                        src.User.FriendsAsUser2.Select(f => f.User1Id).Contains(userId)))
                .ReverseMap();

            config.CreateMap<QuizzClassInviteRequest, QuizzClassInviteRequestModel>()
                .ForMember(dest => dest.UserUserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.Profile.FirstName + " " + src.User.Profile.LastName))
                .ForMember(dest => dest.UserBirthdate, opt => opt.MapFrom(src => src.User.Profile.BirthDate))
                .ForMember(dest => dest.UserProfilePixUrl, opt => opt.MapFrom(src => src.User.Profile.ProfileImageUrl))
                .ForMember(dest => dest.IsQuizzmate, opt => opt.MapFrom(src =>
                        src.User.FriendsAsUser1.Select(f => f.User2Id).Contains(userId) ||
                        src.User.FriendsAsUser2.Select(f => f.User1Id).Contains(userId)))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.QuizzClass.ClassName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.QuizzClass.Description))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.QuizzClass.Tags))
                .ForMember(dest => dest.TeacherUserName, opt => opt.MapFrom(src => src.QuizzClass.Teacher.UserName))
                .ForMember(dest => dest.TeacherFullName, opt => opt.MapFrom(src => src.QuizzClass.Teacher.Profile.FirstName + " " + src.QuizzClass.Teacher.Profile.LastName))
                .ForMember(dest => dest.TeacherBirthdate, opt => opt.MapFrom(src => src.QuizzClass.Teacher.Profile.BirthDate))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.QuizzClass.ImageUrl))
                .ReverseMap();

            config.CreateMap<QuizzClassMember, QuizzClassMemberModel>()
                .ForMember(dest => dest.StudentUserName, opt => opt.MapFrom(src => src.Student.UserName))
                .ForMember(dest => dest.StudentFullName, opt => opt.MapFrom(src => src.Student.Profile.FirstName + " " + src.Student.Profile.LastName))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.Student.Profile.BirthDate))
                .ForMember(dest => dest.StudentProfilePixUrl, opt => opt.MapFrom(src => src.Student.Profile.ProfileImageUrl))
                .ForMember(dest => dest.IsQuizzmate, opt => opt.MapFrom(src =>
                        src.Student.FriendsAsUser1.Select(f => f.User2Id).Contains(userId) ||
                        src.Student.FriendsAsUser2.Select(f => f.User1Id).Contains(userId)))
                .ReverseMap();
        }
    }
}