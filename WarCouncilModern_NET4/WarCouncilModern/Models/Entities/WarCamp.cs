using System.Collections.Generic;
using TaleWorlds.SaveSystem;

namespace WarCouncilModern.Models
{
    /// <summary>
    /// تمثيل لمعسكر ميداني بسيط قابل للحفظ.
    /// </summary>
    public class WarCamp
    {
        [SaveableField(1)] private string _campId;
        [SaveableField(2)] private float _x;
        [SaveableField(3)] private float _y;
        [SaveableField(4)] private bool _isActive;
        [SaveableField(5)] private List<string> _assignedHeroIds;

        public WarCamp()
        {
            _campId = string.Empty;
            _x = 0f;
            _y = 0f;
            _isActive = false;
            _assignedHeroIds = new List<string>();
        }

        public WarCamp(string campId, float x, float y)
        {
            _campId = campId ?? string.Empty;
            _x = x;
            _y = y;
            _isActive = true;
            _assignedHeroIds = new List<string>();
        }

        public string CampId { get { return _campId; } set { _campId = value ?? string.Empty; } }
        public float X { get { return _x; } set { _x = value; } }
        public float Y { get { return _y; } set { _y = value; } }
        public bool IsActive { get { return _isActive; } set { _isActive = value; } }

        public IReadOnlyList<string> AssignedHeroIds { get { return _assignedHeroIds.AsReadOnly(); } }

        public void AssignHero(string heroStringId)
        {
            if (string.IsNullOrEmpty(heroStringId)) return;
            if (!_assignedHeroIds.Contains(heroStringId))
                _assignedHeroIds.Add(heroStringId);
        }

        public bool RemoveAssignedHero(string heroStringId)
        {
            if (string.IsNullOrEmpty(heroStringId)) return false;
            return _assignedHeroIds.Remove(heroStringId);
        }

        public void ClearAssignments() { _assignedHeroIds.Clear(); }
    }
}