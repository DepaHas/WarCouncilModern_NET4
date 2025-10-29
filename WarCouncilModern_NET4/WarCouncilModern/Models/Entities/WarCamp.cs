#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.SaveSystem;

namespace WarCouncilModern.Models.Entities
{
    public class WarCamp
    {
        [SaveableField(1)] private string _campId;
        [SaveableField(2)] private string _locationName;
        [SaveableField(3)] private List<string> _assignedHeroIds;
        [SaveableField(4)] private DateTime _createdAtUtcTicks;

        public WarCamp()
        {
            _campId = Guid.NewGuid().ToString();
            _locationName = string.Empty;
            _assignedHeroIds = new List<string>();
            _createdAtUtcTicks = DateTime.UtcNow;
        }

        public WarCamp(string campId, string locationName, IEnumerable<string>? assignedHeroIds = null)
            : this()
        {
            _campId = string.IsNullOrEmpty(campId) ? Guid.NewGuid().ToString() : campId;
            _locationName = locationName ?? string.Empty;
            // امنع تمرير null مباشرة إلى List ctor
            _assignedHeroIds = new List<string>(assignedHeroIds ?? Enumerable.Empty<string>());
            _createdAtUtcTicks = DateTime.UtcNow;
        }

        public string CampId
        {
            get => _campId;
            set => _campId = value ?? Guid.NewGuid().ToString();
        }

        public string LocationName
        {
            get => _locationName;
            set => _locationName = value ?? string.Empty;
        }

        // إرجاع نسخة للقائمة لحماية الحالة الداخلية
        public IReadOnlyList<string> AssignedHeroIds => _assignedHeroIds.AsReadOnly();

        public DateTime CreatedAtUtc
        {
            get => _createdAtUtcTicks;
            set => _createdAtUtcTicks = value.ToUniversalTime();
        }

        public void AddMemberById(string heroId)
        {
            if (string.IsNullOrEmpty(heroId)) return;
            if (!_assignedHeroIds.Contains(heroId)) _assignedHeroIds.Add(heroId);
        }

        public void RemoveMemberById(string heroId)
        {
            if (string.IsNullOrEmpty(heroId)) return;
            _assignedHeroIds.Remove(heroId);
        }

        public override string ToString() =>
            $"WarCamp[{CampId}] at {LocationName} Members:{_assignedHeroIds.Count}";
    }
}