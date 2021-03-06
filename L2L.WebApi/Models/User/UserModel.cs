﻿using L2L.Entities;
using L2L.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{
    public class UserPointsBase
    {
        public string DailyNormalPointsQuizzSelfStrList { get; set; }
        public string DailyNormalPointsQuizzOthersStrList { get; set; }
        public string DailySpecialPointsQuizzStrList { get; set; }
        public string DailyPointsAllStrList { get; set; }
        public string BadgeStrList { get; set; }

        // generated
        public int TotalDailyRewardItems { get; set; }

        public int[] DailyNormalPointsQuizzSelfIntList { get; set; }
        public int[] DailyNormalPointsQuizzOthersIntList { get; set; }
        public int[] DailySpecialPointsQuizzIntList { get; set; }

        public int[] DailyPointsAllIntList { get; set; }
        public int[] BadgeIntList { get; set; }
    }

    public class UserModel : UserPointsBase
    {
        public int Id { get; set; }
        public UserTypeEnum UserType { get; set; }
        public string LocalAuthUserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int ProfileId { get; set; }
        public ProfileModel Profile { get; set; }

        public int Points { get; set; }
        public int DailyPoints { get; set; }
        public string DailyPointsDate { get; set; }
        public int TotalBadge { get; set; }

        public DependentPermission DependentPermission { get; set; }

        // generated
        public bool IsAdmin { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsThirdPartyLogin { get; set; }

        public IList<DependentModel> AsUserDependents { get; set; }
    }

    public class DependentModel
    {
        public int Id { get; set; }
        public bool IsPrimary { get; set; }
        public int UserId { get; set; }
        public int ChildId { get; set; }

        public string DependentFullName { get; set; }
    }

    public class DependentUserModel
    {
        public DependentUserModel()
        {
            NotificationsSubscription = new DependentNotificationModel();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public ProfileModel Profile { get; set; }

        public UserTypeEnum UserType { get; set; }
        public bool IsCurrentUserPrimary { get; set; }

        public DependentPermissionModel Permissions { get; set; }
        public DependentNotificationModel NotificationsSubscription { get; set; }

        //public IList<int> ParentIds { get; set; }
    }

    public class DependentPermissionModel
    {
        public int Id { get; set; }
        public bool CanAcceptQuizzmateRequests { get; set; }
        public bool CanUseMessaging { get; set; }
    }

    public class DependentNotificationModel
    {
        public int ChildId { get; set; }

        public bool NwQuizzSubmit { get; set; }
        public bool NwQuizzLive { get; set; }
        public bool NwQuizzReceiveComment { get; set; }

        public bool NwPostComment { get; set; }
        public bool NwPostedCommentFlagged { get; set; }
        public bool NwQuestionFlagged { get; set; }

        public bool NwMessageSent { get; set; }
        public bool NwMessageReceived { get; set; }

        public bool NwQuizzmateRequest { get; set; }
        public bool NwQuizzmateRequestAccept { get; set; }
    }

    public class QuizzerModel : UserPointsBase, IWithAgeModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public UserTypeEnum UserType { get; set; }

        public ProfileModel Profile { get; set; }
        public int Points { get; set; }
        public int DailyPoints { get; set; }
        public string DailyPointsDate { get; set; }
        public int TotalBadge { get; set; }

        // generated by query
        public int NumQuizzes { get; set; }
        public string UserFullName { get; set; }
        public bool IsQuizzmate { get; set; }
        public int? FriendRequestSentId { get; set; }
        public bool IsFriendRequestSent { get; set; }
        public int? FriendRequestPendingId { get; set; }
        public bool IsFriendRequestPending { get; set; }

        // Not used
        public int FriendRequestId { get; set; }
        public bool IsQuizzlingRequestSent { get; set; }
        public bool IsFollowing { get; set; }

        // from code
        public bool IsSelf { get; set; }
        public bool IsParent { get; set; }
        public bool IsDependent { get; set; }
        public RelationshipNotificationModel RelationshipNotification { get; set; }

        // From view
        public bool IsProfilePixModified { get; set; }
        public string ProfilePix { get; set; }
        public string ProfilePixName { get; set; }

        // With Age
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }
    }
}