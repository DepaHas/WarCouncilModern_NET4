using System;
using System.Collections.Generic;
using TaleWorlds.SaveSystem;

namespace WarCouncilModern.Models.Entities
{
    public class WarCamp
    {
        [SaveableField(1)] private string _campId;
        [SaveableField(2)] private string _locationId;
        [SaveableField(3)] private List<string> _assignedHeroIds;
        [SaveableField(4)] private long _createdTicks;

        private readonly object _lock = new object();

        public WarCamp()
        {
            _campId = Guid.NewGuid().ToString();
            _locationId = string.Empty;
            _assignedHeroIds = new List<string>();
            _createdTicks = DateTime.UtcNow.Ticks;
        }

        public WarCamp(string campId, string locationId, IEnumerable<string> heroIds = null) : this()
        {
            _campId = campId ?? Guid.NewGuid().ToString();
            _locationId = locationId ?? string.Empty;
            if (heroIds != null) _assignedHeroIds.AddRange(heroIds);
        }

        public string CampId { get { lock (_lock) return _campId; } set { lock (_lock) _campId = value ?? Guid.NewGuid().ToString(); } }
        public string LocationId { get { lock (_lock) return _locationId; } set { lock (_lock) _locationId = value ?? string.Empty; } }
        public IReadOnlyList<string> AssignedHeroIds { get { lock (_lock) return _assignedHeroIds.ToArray(); } }

        public DateTime CreatedAtUtc { get { return new DateTime(_createdTicks, DateTimeKind.Utc); } set { _createdTicks = value.ToUniversalTime().Ticks; } }

        public void AssignHero(string heroId)
        {
            if (string.IsNullOrEmpty(heroId)) return;
            lock (_lock) { if (!_assignedHeroIds.Contains(heroId)) _assignedHeroIds.Add(heroId); }
        }

        public bool RemoveHero(string heroId)
        {
            if (string.IsNullOrEmpty(heroId)) return false;
            lock (_lock) { return _assignedHeroIds.Remove(heroId); }
        }

        public override string ToString()
        {
            lock (_lock) return $"Camp[{CampId}] @ {LocationId} - {AssignedHeroIds.Count} heroes";
        }
    }
}