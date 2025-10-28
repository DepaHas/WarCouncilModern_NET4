using System;
using TaleWorlds.SaveSystem;

namespace WarCouncilModern.CouncilSystem
{
    public class WarDecision
    {
        [SaveableField(1)] private string _decisionId;
        [SaveableField(2)] private string _title;
        [SaveableField(3)] private string _description;
        [SaveableField(4)] private DateTime _timestamp;
        [SaveableField(5)] private string _madeByHeroId;

        public WarDecision()
        {
            _decisionId = string.Empty;
            _title = string.Empty;
            _description = string.Empty;
            _timestamp = DateTime.UtcNow;
            _madeByHeroId = string.Empty;
        }

        public WarDecision(string id, string title, string description, string madeByHeroId)
        {
            _decisionId = id ?? string.Empty;
            _title = title ?? string.Empty;
            _description = description ?? string.Empty;
            _timestamp = DateTime.UtcNow;
            _madeByHeroId = madeByHeroId ?? string.Empty;
        }

        public string DecisionId { get { return _decisionId; } set { _decisionId = value ?? string.Empty; } }
        public string Title { get { return _title; } set { _title = value ?? string.Empty; } }
        public string Description { get { return _description; } set { _description = value ?? string.Empty; } }
        public DateTime Timestamp { get { return _timestamp; } } // قراءة فقط لثبات الطابع
        public string MadeByHeroId { get { return _madeByHeroId; } set { _madeByHeroId = value ?? string.Empty; } }

        public override string ToString()
        {
            return string.Format("Decision {0}: {1} by {2} at {3:u}", DecisionId, Title, MadeByHeroId, Timestamp);
        }

        public override bool Equals(object obj)
        {
            var other = obj as WarDecision;
            if (other == null) return false;
            return string.Equals(_decisionId, other._decisionId, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(_decisionId ?? string.Empty);
        }
    }
}