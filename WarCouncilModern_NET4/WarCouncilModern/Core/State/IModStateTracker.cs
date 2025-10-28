using System;

namespace WarCouncilModern.Core.State
{
    public interface IModStateTracker
    {
        void RecordEvent(string eventType, Guid entityId, object? metadata = null);
    }
}