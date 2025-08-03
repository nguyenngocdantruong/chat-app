using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Interfaces.ExternalService
{
    public interface IPresenceService
    {
        // Tracker user
        Task TrackUserConnection(Guid userId, string connectionId);
        Task UnTrackUserConnection(string connectionId);
    }
}
