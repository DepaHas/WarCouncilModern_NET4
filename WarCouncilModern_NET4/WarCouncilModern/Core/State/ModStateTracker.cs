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

        public ModStateTracker(IModLogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void RecordEvent(string eventType, Guid entityId, object? metadata = null)
        {
            _events.Add((DateTime.UtcNow, eventType, entityId, metadata));
            _logger.Info($"[State] {eventType} - {entityId} - {metadata}");
        }

        public IReadOnlyList<(DateTime Time, string EventType, Guid Entity, object? Metadata)> GetEvents() => _events.AsReadOnly();
    }
}