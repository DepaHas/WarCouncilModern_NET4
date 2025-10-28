using System;
using WarCouncilModern.CouncilSystem;
using WarCouncilModern.Models.Entities;

namespace WarCouncilModern.Core.Services
{
    public interface ICouncilMeetingService
    {
        void ScheduleMeetingForDecision(WarCouncil council, WarDecision decision);
        void CancelMeeting(Guid meetingId);
    }
}