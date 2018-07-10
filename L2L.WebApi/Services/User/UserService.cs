using L2L.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.Entities;
using L2L.WebApi.Models;
using L2L.WebApi.Controllers;
using L2L.WebApi.Interfaces;
using L2L.WebApi.Utilities;
using L2L.WebApi.Enums;
using System.Data.Entity;
using L2L.Entities.Enums;

namespace L2L.WebApi.Services
{
    public class UserService : BaseService
    {
        new private User _currentUser;
        private UserModel _currentUserModel;

        public UserService(BaseApiController controller)
            : base(controller)
        {
        }

        public bool GetMainUserId(string emailOrUserName, out string mainUserId, out string subUserId)
        {
            mainUserId = subUserId = "";
            try
            {
                if (emailOrUserName.Contains("@"))
                {
                    mainUserId = subUserId = _uow.Users.GetAll()
                        .Where(u => u.Email == emailOrUserName && u.UserType == UserTypeEnum.Standard)
                        .Select(u => u.LocalAuthUserId)
                        .FirstOrDefault();

                    if (String.IsNullOrEmpty(mainUserId))
                        return false;
                }
                else
                {
                    var user = _uow.Users.GetAll()
                        .Where(u => u.UserName == emailOrUserName)
                        .Include(u => u.AsChildDependsOn.Select(d => d.User))
                        .FirstOrDefault();

                    if (user.UserType == UserTypeEnum.Child)
                    {
                        var primaryParent = user.AsChildDependsOn
                            .Where(d => d.IsPrimary == true)
                            .Select(d => d.User)
                            .FirstOrDefault();
                        mainUserId = primaryParent.LocalAuthUserId;
                        subUserId = user.LocalAuthUserId;
                    }
                    else
                    {
                        mainUserId = subUserId = user.LocalAuthUserId;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        public bool IsEmailUsed(string email)
        {
            var count = _uow.Users.GetAll()
                .Where(u => u.Email == email)
                .Count();

            return count != 0;
        }

        public bool IsUserNameUsed(string userName)
        {
            var count = _uow.Users.GetAll()
                .Where(u => u.UserName == userName)
                .Count();

            return count != 0;
        }

        public UserModel CreateNewStandardUser(string localAuthId, string userName, string email, RegisterUserModelBase model = null)
        {
            var userModel = CreateNewUser(localAuthId, userName, email, UserTypeEnum.Standard, model);
            return userModel;
        }

        public UserModel CreateNewDependentUser(string localAuthId, string userName, string email, RegisterDependentModel model)
        {
            var childUser = CreateNewUser(localAuthId, userName, email, UserTypeEnum.Child, model);
            return childUser;
        }

        private UserModel CreateNewUser(string localAuthId, string userName, string email, UserTypeEnum userType, RegisterUserModelBase model = null)
        {
            try
            {
                User user = new User();

                user.LocalAuthUserId = localAuthId;
                user.UserType = userType;
                user.UserName = userName;
                user.Email = email;
                user.BadgeStrList = "1,";

                if (model != null)
                {
                    user.Profile = new Profile();
                    MappingUtil.Map(model, user.Profile);
                    //user.Profile.ProfileImageUrl = "Content/images/Icons/default-profile-1.0.jpg";
                    user.Profile.ProfileImageUrl = "Content/images/" + localAuthId + "/default-profile-1.0.jpg";
                }

                Utilities.ImageUtil.CreateDefaultProfilePix(localAuthId);

                if (userType == UserTypeEnum.Child)
                {
                    AddDependentRelationshipToCurrentUser(user);
                    AddQuizzmateRelationshipToCurrentUser(user);
                }
                _uow.Users.Add(user);
                _uow.SaveChanges();

                var userModel = MappingUtil.Map<User, UserModel>(user);

                return userModel;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        private void AddDependentRelationshipToCurrentUser(User child)
        {
            var user = GetCurrentUser();
            var dependentEntity = new Dependent();
            child.DependentPermission = new DependentPermission()
            {
                CanAcceptQuizzmateRequests = true,
                CanUseMessaging = true
            };

            dependentEntity.Child = child;
            dependentEntity.UserId = user.Id;
            dependentEntity.IsPrimary = true;

            _uow.Dependents.Add(dependentEntity);
        }

        private void AddQuizzmateRelationshipToCurrentUser(User child)
        {
            var user = GetCurrentUser();

            var friendRelationship = new FriendRelationship
            {
                User1Id = user.Id,
                User2Id = child.Id
            };

            _uow.FriendRelationships.Add(friendRelationship);
        }

        public User GetCurrentUser()
        {
            CheckIfUserChanged();
            if (_currentUser == null)
            {
                _currentUser = _sessionHelper.Retrieve(SessionIdxEnum.CurrentUserEntity) as User;
                if (_currentUser == null)
                {
                    string localId = UserInfoUtil.GetLocalId();
                    if (localId != null)
                    {
                        var userTmp = _uow.Users.GetAll()
                            .Where(u => u.LocalAuthUserId == localId)
                            .Include(u => u.Profile)
                            .Include(u => u.AsUserDependents.Select(d => d.Child.Profile))
                            .Include(u => u.AsChildDependsOn.Select(p => p.User.Profile))
                            .FirstOrDefault();

                        if (userTmp != null)
                        {
                            userTmp.MapToNew<User, User>(out _currentUser);
                            _uow.Users.Detach(userTmp);
                        }
                    }
                    else
                    {
                        CreateDefaultUser();
                    }
                    _sessionHelper.Store(SessionIdxEnum.CurrentUserEntity, _currentUser);
                }
                else
                {
                    _currentUserModel = MappingUtil.Map<User, UserModel>(_currentUser);
                }
            }
            return _currentUser;
        }

        private void CreateDefaultUser()
        {
            _currentUser = new User()
            {
                Id = 0,
                UserType = UserTypeEnum.Standard,
                Profile = new Profile
                {
                    Id = 0,
                    FirstName = "Guest",
                    BirthDate = new DateTime(1950, 01, 01),
                    ProfileImageUrl = "Content/images/Icons/default-profile-1.0.jpg"
                },
            };
        }

        public UserModel GetCurrentUserModel()
        {
            UserModel userModel = null;
            CheckIfUserChanged();
            if (_currentUser == null)
            {
                GetCurrentUser();
            }

            _currentUser.MapToNew(out userModel);
            GetUserPontsIntArray(userModel);

            userModel.IsAdmin = HttpContext.Current.User.IsInRole("admin");
            return userModel;
        }

        public void GetUserPontsIntArray(UserPointsBase userPointsBase)
        {
            userPointsBase.TotalDailyRewardItems = QuizzPointsService.MaxDailySpecialQuizzTake;
            userPointsBase.DailyNormalPointsQuizzSelfIntList = HelperUtil.GetIntArrayFromString(userPointsBase.DailyNormalPointsQuizzSelfStrList);
            userPointsBase.DailyNormalPointsQuizzOthersIntList = HelperUtil.GetIntArrayFromString(userPointsBase.DailyNormalPointsQuizzOthersStrList);
            userPointsBase.DailySpecialPointsQuizzIntList = HelperUtil.GetIntArrayFromString(userPointsBase.DailySpecialPointsQuizzStrList);
            userPointsBase.DailyPointsAllIntList = HelperUtil.GetIntArrayFromString(userPointsBase.DailyPointsAllStrList, QuizzPointsService.MaxDailyQuizzTypeCount);
            userPointsBase.BadgeIntList = HelperUtil.GetIntArrayFromString(userPointsBase.BadgeStrList, BadgeService.MaxBadgeTypeCount);
        }

        private void CheckIfUserChanged()
        {
            string localId = _sessionHelper.Retrieve(SessionIdxEnum.LocalId) as string;
            if (localId == null || localId != UserInfoUtil.GetLocalId())
            {
                localId = UserInfoUtil.GetLocalId();
                _sessionHelper.Store(SessionIdxEnum.LocalId, localId);
                ResetSessionServiceLocalData();
            }
        }

        public void ResetStoredUser()
        {
            _sessionHelper.Store(SessionIdxEnum.LocalId, null);
        }

        public User GetUser(int id)
        {
            var user = _uow.Users.GetAll()
                .Where(u => u.Id == id)
                .Include(u => u.Profile)
                .FirstOrDefault();

            return user;
        }

        private void ResetSessionServiceLocalData()
        {
            _currentUser = null;
            _sessionHelper.Store(SessionIdxEnum.CurrentUserEntity, null);

            _currentUserModel = null;
            _sessionHelper.Store(SessionIdxEnum.CurrentUserModel, null);
        }

        public void SetClientToday(string clientToday)
        {
            _sessionHelper.Store(SessionIdxEnum.ClientToday, clientToday);
            _svcContainer.DailyRewardSvc.CheckResetDailyReward(clientToday);
        }

        public string GetClientToday()
        {
            return _sessionHelper.Retrieve(SessionIdxEnum.ClientToday) as string;
        }

        public void UpdateCurrentUserInSession()
        {
            _sessionHelper.Store(SessionIdxEnum.CurrentUserEntity, _currentUser);
        }

        public bool IsParent(int depId)
        {
            foreach (var item in _currentUser.AsChildDependsOn)
            {
                if (item.UserId == depId)
                    return true;
            }

            return false;
        }

        public bool IsDependent(int depId)
        {
            foreach (var item in _currentUser.AsUserDependents)
            {
                if (item.ChildId == depId)
                    return true;
            }

            return false;
        }

        public bool IsQuizzmate(int userId)
        {
            var isQuizzmate = _uow.Users.GetAll()
                .Where(u => u.Id == _currentUser.Id)
                .Select(u => u.FriendsAsUser1.Select(f => f.User1Id).Contains(userId) ||
                            u.FriendsAsUser2.Select(f => f.User2Id).Contains(userId))
                .FirstOrDefault();

            return isQuizzmate;
        }
    }
}