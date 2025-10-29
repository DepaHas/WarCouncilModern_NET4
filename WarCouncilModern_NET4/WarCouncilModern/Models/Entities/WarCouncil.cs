using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.SaveSystem;

namespace WarCouncilModern.Models.Entities
{

    public class WarCouncil
    {
        [SaveableField(1)] private string _saveId;
        [SaveableField(2)] private string _kingdomStringId;
        [SaveableField(3)] private CouncilStructure _structure;
        [SaveableField(4)] private string _leaderHeroId;
        [SaveableField(5)] private List<string> _memberHeroIds;
        [SaveableField(6)] private List<WarDecision> _decisions;
        [SaveableField(7)] private List<WarReport> _reports;
        [SaveableField(8)] private long _createdTicks;

        private readonly object _lock = new object();

        public WarCouncil()
        {
            _saveId = Guid.NewGuid().ToString();
            _kingdomStringId = string.Empty;
            _structure = CouncilStructure.Undefined;
            _leaderHeroId = string.Empty;
            _memberHeroIds = new List<string>();
            _decisions = new List<WarDecision>();
            _reports = new List<WarReport>();
            _createdTicks = DateTime.UtcNow.Ticks;
        }

        public WarCouncil(string kingdomId, CouncilStructure structure) : this()
        {
            _kingdomStringId = kingdomId ?? string.Empty;
            _structure = structure;
        }

        public string SaveId { get { lock (_lock) return _saveId; } set { lock (_lock) _saveId = value ?? Guid.NewGuid().ToString(); } }
        public string KingdomStringId { get { lock (_lock) return _kingdomStringId; } set { lock (_lock) _kingdomStringId = value ?? string.Empty; } }
        public CouncilStructure Structure { get { lock (_lock) return _structure; } set { lock (_lock) _structure = value; } }
        public string LeaderHeroId { get { lock (_lock) return _leaderHeroId; } private set { lock (_lock) _leaderHeroId = value ?? string.Empty; } }
        public IReadOnlyList<string> MemberHeroIds { get { lock (_lock) return _memberHeroIds.ToArray(); } }
        public IReadOnlyList<WarDecision> Decisions { get { lock (_lock) return _decisions.ToArray(); } }
        public IReadOnlyList<WarReport> Reports { get { lock (_lock) return _reports.ToArray(); } }
        public DateTime CreatedAtUtc { get { return new DateTime(_createdTicks, DateTimeKind.Utc); } set { _createdTicks = value.ToUniversalTime().Ticks; } }

        public void AssignLeaderByHeroId(string heroStringId)
        {
            if (string.IsNullOrEmpty(heroStringId)) return;
            lock (_lock)
            {
                _leaderHeroId = heroStringId;
                if (!_memberHeroIds.Contains(heroStringId)) _memberHeroIds.Add(heroStringId);
            }
        }

        public bool SetLeaderToFirstMember()
        {
            lock (_lock)
            {
                if (_memberHeroIds.Count == 0) { _leaderHeroId = string.Empty; return false; }
                _leaderHeroId = _memberHeroIds[0];
                return true;
            }
        }

        public void AssignMembersByHeroIds(IEnumerable<string> heroIds)
        {
            lock (_lock)
            {
                _memberHeroIds.Clear();
                if (heroIds == null) return;
                foreach (var id in heroIds)
                {
                    if (string.IsNullOrEmpty(id)) continue;
                    if (!_memberHeroIds.Contains(id)) _memberHeroIds.Add(id);
                }
            }
        }

        public void AddMemberById(string heroStringId)
        {
            if (string.IsNullOrEmpty(heroStringId)) return;
            lock (_lock) { if (!_memberHeroIds.Contains(heroStringId)) _memberHeroIds.Add(heroStringId); }
        }

        public bool RemoveMemberById(string heroStringId)
        {
            if (string.IsNullOrEmpty(heroStringId)) return false;
            lock (_lock) { return _memberHeroIds.Remove(heroStringId); }
        }

        public void AddDecision(WarDecision d)
        {
            if (d == null) return;
            lock (_lock) _decisions.Add(d);
        }

        public WarDecision? GetDecisionById(string decisionSaveId)
        {
            if (string.IsNullOrEmpty(decisionSaveId)) return null;
            lock (_lock) return _decisions.FirstOrDefault(x => x.SaveId == decisionSaveId);
        }

        public void AddReport(WarReport r)
        {
            if (r == null) return;
            lock (_lock) _reports.Add(r);
        }

        public bool IsValid()
        {
            lock (_lock) return !string.IsNullOrEmpty(_kingdomStringId);
        }

        public override string ToString()
        {
            lock (_lock) return string.Format("WarCouncil[{0}]: {1} members, {2} decisions, {3} reports", _kingdomStringId, _memberHeroIds.Count, _decisions.Count, _reports.Count);
        }
    }
}