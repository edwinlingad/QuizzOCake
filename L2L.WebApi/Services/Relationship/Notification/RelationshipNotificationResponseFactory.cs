using L2L.Entities;
using L2L.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Services
{
    public class RelationshipNotificationResponseFactory : BaseService
    {
        public RelationshipNotificationResponseFactory(BaseApiController controller)
            : base(controller)
        {
        }

        public IRelationshipResponse GetResponseObject(RelationshipNotificationTypeEnum type)
        {
            IRelationshipResponse _response = null;
            switch (type)
            {
                case RelationshipNotificationTypeEnum.QuizzmateRequest:
                    _response = new QuizzmateRequestService(_controller);
                    break;
                case RelationshipNotificationTypeEnum.QuizzlingRequest:
                    _response = new QuizzlingRequestService(_controller);
                    break;
                case RelationshipNotificationTypeEnum.AddToClassRequest:
                    break;
                default:
                    break;
            }

            return _response;
        }
    }
}