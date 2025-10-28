using System;
using WarCouncilModern.Core.Services;
using WarCouncilModern.CouncilSystem;
using WarCouncilModern.Models.Entities;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.Core.Services
{
    public class StubCouncilMeetingService : ICouncilMeetingService
    {
        private readonly IModLogger _logger;

        public StubCouncilMeetingService(IModLogger logger)
        {
            _logger = logger;
        }

        public void ScheduleMeetingForDecision(WarCouncil council, WarDecision decision)
        {
            // Minimal stub: log scheduling action
            _logger.Info($"[StubCouncilMeetingService] ScheduleMeeting requested for decision {decision.SaveId} in council {council.SaveId}.");
            // In future: create Meeting entity, link attendees, schedule time.
        }

        public void CancelMeeting(Guid meetingId)
        {
            _logger.Info($"[StubCouncilMeetingService] CancelMeeting requested for meeting {meetingId}.");
        }
    }
}