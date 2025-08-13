using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.Interfaces.ExternalService;

namespace ChatApp.Infrastructure.ExternalServices.CacheService
{
    public class PresenceService : IPresenceService
    {
        public Task TrackUserConnection(Guid userId, string connectionId)
        {
            throw new NotImplementedException();
        }

        public Task UnTrackUserConnection(string connectionId)
        {
            throw new NotImplementedException();
        }
    }
}
