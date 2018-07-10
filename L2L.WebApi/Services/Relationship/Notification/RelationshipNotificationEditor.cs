using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.Enums;

namespace L2L.WebApi.Services
{
    public class RelationshipNotificationEditor
    {
        private RelationshipNotification _entity;

        public RelationshipNotificationEditor(RelationshipNotificationTypeEnum type, int fromId, int toId)
        {
            _entity = new RelationshipNotification
            {
                RNType = type,
                PostedDate = DateTime.UtcNow,
                IsNew = true,
                ToUserId = toId,
                FromUserId = fromId,
            };
        }

        public RelationshipNotification GetEntity()
        {
            return _entity;
        }

        public void AddFriendRequest(int id)
        {
            _entity.FriendRequestId = id;
        }

        public void AddDependentRequest(int id)
        {
            _entity.DependentRequestFromUserId = id;
        }
    }
}