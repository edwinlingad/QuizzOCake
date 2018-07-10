using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Entities.Enums;

namespace L2L.Entities
{
    public class User
    {
        public User()
        { 
            Quizzes = new HashSet<Quizz>();
            QuizLogs = new HashSet<TestLog>();

            UserRatings = new HashSet<QuizzUserRating>();
            QuizUpvotes = new HashSet<QuizzUpvote>();

            InitializeFriendProperties();
            InitializeGroupProperties();
            InitializeDependentProperties();
        }

        private void InitializeFriendProperties()
        {
            FriendsAsUser1 = new HashSet<FriendRelationship>();
            FriendsAsUser2 = new HashSet<FriendRelationship>();
            FriendRequestsTo = new HashSet<FriendRequest>();
            FriendRequestsFrom = new HashSet<FriendRequest>();
        }

        private void InitializeGroupProperties()
        {
            UserGroups = new HashSet<UserGroup>();
            PublicGroupsCreated = new HashSet<PublicGroup>();
            PublicGroupsMemberOf = new HashSet<PublicGroupMember>();
            PublicGroupRequests = new HashSet<PublicGroupMembershipRequest>();
        }

        private void InitializeDependentProperties()
        {
            AsUserDependents = new HashSet<Dependent>();
            AsChildDependsOn = new HashSet<Dependent>();
            AsUserDependentRequestsTo = new HashSet<DependentRequestFromUser>();
            AsUserDependentRequestsFrom = new HashSet<DependentRequestFromUser>();
            AsChildDependentRequestsTo = new HashSet<DependentRequestFromChild>();
            AsChildDependentRequestsFrom = new HashSet<DependentRequestFromChild>();
        }

        public int Id { get; set; }
        public UserTypeEnum UserType { get; set; }
        public string LocalAuthUserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int Points { get; set; }

        public int DailyPoints { get; set; }
        public string DailyPointsDate { get; set; }

        public string DailyNormalPointsQuizzSelfStrList { get; set; }
        public string DailyNormalPointsQuizzOthersStrList { get; set; }
        public string DailySpecialPointsQuizzStrList { get; set; }

        public string DailyPointsAllStrList { get; set; }

        public int TotalBadge { get; set; }
        public string BadgeStrList { get; set; }

        // Foreign Keys

        // Navigation Properties
        public virtual DependentPermission DependentPermission { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual ICollection<Quizz> Quizzes { get; set; }
        public virtual ICollection<TestLog> QuizLogs { get; set; }
        public virtual IList<Question> QuestionsCreated { get; set; }

        public virtual ICollection<QuizzUserRating> UserRatings { get; set; }
        public virtual ICollection<QuizzUpvote> QuizUpvotes { get; set; }

        // Unfortunately the relationships are like these because of how the tables
        public virtual ICollection<FriendRelationship> FriendsAsUser1 { get; set; }
        public virtual ICollection<FriendRelationship> FriendsAsUser2 { get; set; }
        public virtual ICollection<FriendRequest> FriendRequestsTo { get; set; }
        public virtual ICollection<FriendRequest> FriendRequestsFrom { get; set; }

        public virtual ICollection<UserGroup> UserGroups { get; set; }
        public virtual ICollection<PublicGroup> PublicGroupsCreated { get; set; }
        public virtual ICollection<PublicGroupMember> PublicGroupsMemberOf { get; set; }
        public virtual ICollection<PublicGroupMembershipRequest> PublicGroupRequests { get; set; }
       
        public virtual ICollection<Dependent> AsUserDependents { get; set; }
        public virtual ICollection<Dependent> AsChildDependsOn { get; set; }
        public virtual ICollection<DependentRequestFromUser> AsUserDependentRequestsTo { get; set; }
        public virtual ICollection<DependentRequestFromUser> AsUserDependentRequestsFrom { get; set; }
        public virtual ICollection<DependentRequestFromChild> AsChildDependentRequestsTo { get; set; }
        public virtual ICollection<DependentRequestFromChild> AsChildDependentRequestsFrom { get; set; }
    }
}
