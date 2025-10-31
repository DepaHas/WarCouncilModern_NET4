using System;

namespace WarCouncilModern.Core.State
{
    public class ModStats
    {
        public int ActiveCouncils { get; set; }
        public int DecisionsProposed { get; set; }
        public int DecisionsProcessed { get; set; }
    }

    public interface IModStateTracker
    {
        void RecordEvent(string eventType, Guid entityId, object? metadata = null);
        ModStats GetStats();
    }
}