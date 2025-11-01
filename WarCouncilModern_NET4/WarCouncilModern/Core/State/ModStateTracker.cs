using System;
using System.Collections.Generic;
using WarCouncilModern.Core.State;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.Core.State
{
    public class ModStateTracker : IModStateTracker
    {
        private readonly List<(DateTime Time, string EventType, Guid Entity, object? Metadata)> _events = new();
        private readonly IModLogger _logger;

        private int _activeCouncils;
        private int _decisionsProposed;
        private int _decisionsProcessed;

        public ModStateTracker(IModLogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void RecordEvent(string eventType, Guid entityId, object? metadata = null)
        {
            _events.Add((DateTime.UtcNow, eventType, entityId, metadata));
            _logger.Info($"[State] {eventType} - {entityId} - {metadata}");

            switch (eventType)
            {
                case "CouncilCreated":
                    _activeCouncils++;
                    break;
                case "CouncilRemoved":
                    _activeCouncils--;
                    break;
                case "DecisionProposed":
                    _decisionsProposed++;
                    break;
                case "DecisionProcessed":
                    _decisionsProcessed++;
                    break;
            }
        }

        public void RecordCouncilCreated()
        {
            RecordEvent("CouncilCreated", Guid.Empty);
        }

        public void RecordDecisionProposed()
        {
            RecordEvent("DecisionProposed", Guid.Empty);
        }

        public IReadOnlyList<(DateTime Time, string EventType, Guid Entity, object? Metadata)> GetEvents() => _events.AsReadOnly();

        public ModStats GetStats()
        {
            return new ModStats
            {
                ActiveCouncils = _activeCouncils,
                DecisionsProposed = _decisionsProposed,
                DecisionsProcessed = _decisionsProcessed
            };
        }
    }
}
