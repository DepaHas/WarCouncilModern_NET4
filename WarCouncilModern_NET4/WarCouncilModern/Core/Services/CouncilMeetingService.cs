using System;
using WarCouncilModern.Core.Services;
using WarCouncilModern.Models.Entities;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.Core.Services
{
    public class CouncilMeetingService : ICouncilMeetingService
    {
        private readonly IModLogger _logger;

        public CouncilMeetingService(IModLogger logger)
        {
            _logger = logger;
        }

        public void ScheduleMeetingForDecision(WarCouncil council, WarDecision decision)
        {
            _logger.Log("Scheduling a meeting for decision: " + decision.Title);
        }

        public void CancelMeeting(Guid meetingId)
        {
            _logger.Log("Canceling meeting: " + meetingId.ToString());
        }
    }
}
