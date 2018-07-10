using L2L.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper.QueryableExtensions;
using L2L.WebApi.Models;
using System.Data.Entity;
using Newtonsoft.Json;
using L2L.Entities;
using L2L.WebApi.Utilities;
using System.IO;
using System.Drawing;

namespace L2L.WebApi.Services
{
    public class QuizzerInfoService : BaseService, IResource
    {
        private const string ImageLocationFolder = "/Content/uploadedImages";

        public QuizzerInfoService(BaseApiController controller)
            : base(controller)
        {
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public object Get(int id)
        {
            try
            {
                var model = _uow.Users.GetAll()
                    .Where(u => u.Id == id)
                    .Include(u => u.Profile)
                    .ProjectTo<QuizzerModel>(new { userId = _currentUser.Id })
                    .FirstOrDefault();

                UpdateModel(model);

                return model;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            throw new NotImplementedException();
        }

        public bool Patch(object modelParam)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<QuizzerModel>(modelParam.ToString());
                if (model == null)
                    return false;

                if (HasUpdatePermission(model) == false)
                    return false;

                Profile entity;
                model.Profile.MapToNew<ProfileModel, Profile>(out entity);

                if (model.IsProfilePixModified)
                {
                    User user;
                    bool isCurrentUser = model.Id == _currentUser.Id;
                    if (isCurrentUser)
                        user = _currentUser;
                    else
                        user = _svcContainer.UserSvc.GetUser(model.Id);
                    SaveImage(user, entity, model.ProfilePixName, model.ProfilePix);

                    if (!isCurrentUser)
                    {
                        _uow.Profiles.Detach(user.Profile);
                        _uow.Users.Detach(user);
                    }
                }

                _uow.Profiles.Update(entity);
                _uow.SaveChanges();

                _svcContainer.UserSvc.ResetStoredUser();

                return true;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return false;
            }
        }

        private void SaveImage(User user, Profile entity, string fileName, string imageData)
        {
            string imageUri;
            ImageUtil.SaveProfilePix(user, fileName, imageData, out imageUri);
            entity.ProfileImageUrl = imageUri;
        }

        public object Post(object model)
        {
            throw new NotImplementedException();
        }

        public object GetManyAlt(int id, int id2, int id3, string str, string str2, string str3)
        {
            throw new NotImplementedException();
        }

        public object GetAlt(string str)
        {
            throw new NotImplementedException();
        }

        private bool HasUpdatePermission(QuizzerModel model)
        {
            if (model.Id == _currentUser.Id)
                return true;
            if (_svcContainer.UserSvc.IsDependent(model.Id))
                return true;

            return false;
        }

        public void UpdateModelList(IEnumerable<QuizzerModel> list)
        {
            foreach (var item in list)
            {
                UpdateModel(item);
            }
        }

        public void UpdateModel(QuizzerModel model)
        {
            SetAge(model);

            model.IsSelf = model.Id == _currentUser.Id;
            model.IsParent = false;
            model.IsDependent = false;

            if (_currentUser.UserType == Entities.Enums.UserTypeEnum.Standard)
            {
                if(model.UserType == Entities.Enums.UserTypeEnum.Child)
                {
                    model.IsDependent = _svcContainer.UserSvc.IsDependent(model.Id);
                }
            }
            else
            {
                if(model.UserType == Entities.Enums.UserTypeEnum.Standard)
                {
                    model.IsParent = _svcContainer.UserSvc.IsParent(model.Id);
                }
            }

            if (model.IsSelf || model.IsParent || model.IsDependent)
                model.IsQuizzmate = true;

            model.Profile.BirthDate = model.Profile.BirthDate.ToLocalTime();
            if (model.IsQuizzmate == false)
            {
                model.UserFullName = "";
                model.Profile.FirstName = "";
                model.Profile.LastName = "";
                model.Profile.BirthDate = DateTime.Now;
            }

            if (model.IsFriendRequestPending)
            {
                model.FriendRequestId = (int)model.FriendRequestPendingId;
                model.RelationshipNotification = _uow.RelationshipNotifications.GetAll()
                    .Where(rn => rn.FriendRequestId == model.FriendRequestPendingId)
                    .ProjectTo<RelationshipNotificationModel>()
                    .FirstOrDefault();
            }

            if (model.IsFriendRequestSent)
            {
                model.FriendRequestId = (int)model.FriendRequestSentId;
                model.RelationshipNotification = _uow.RelationshipNotifications.GetAll()
                    .Where(rn => rn.FriendRequestId == model.FriendRequestSentId)
                    .ProjectTo<RelationshipNotificationModel>()
                    .FirstOrDefault();
            }

            _svcContainer.UserSvc.GetUserPontsIntArray(model);
        }

    }
}