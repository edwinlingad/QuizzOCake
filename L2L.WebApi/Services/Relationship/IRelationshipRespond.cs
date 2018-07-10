using L2L.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.WebApi.Services
{
    public interface IRelationshipResponse
    {
        // return true to delete the RelationshipNotification entity
        // Don't call save changes        
        bool ProccessResponse(RelationshipNotificationModel model);
    }
}
