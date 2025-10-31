using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.SaveSystem;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.Models.Entities
{
    public class WarDecision
    {
        [SaveableField(1)] private string _decisionId;
        [SaveableField(2)] private string _title;
        [SaveableField(3)] private string _description;
        [SaveableField(4)] private string _status;
        [SaveableField(5)] private string _proposedByHeroId;
        [SaveableField(6)] private long _proposedTicks;
        [SaveableField(7)] private Dictionary<string, bool> _votes = new Dictionary<string, bool>();
        [SaveableField(8)] public DecisionExecutionPayload ExecutionPayload { get; set; }

        private readonly object _lock = new object();

        public WarDecision()
        {
            _decisionId = Guid.NewGuid().ToString();
            _title = string.Empty;
            _description = string.Empty;
            _status = "Proposed";
            _proposedByHeroId = string.Empty;
            _proposedTicks = DateTime.UtcNow.Ticks;
        }

        // مُشيد مطابق لاستدعاءات WarCouncilManager(4 args)
        public WarDecision(string decisionId, string title, string proposedByHeroId, string status) : this()
        {
            _decisionId = decisionId ?? Guid.NewGuid().ToString();
            _title = title ?? string.Empty;
            _proposedByHeroId = proposedByHeroId ?? string.Empty;
            _status = status ?? "Proposed";
            _proposedTicks = DateTime.UtcNow.Ticks;
        }

        // القرار يظهر أن الكود يستعمل DecisionId و SaveId أحيانًا؛ احتفظ بكليهما لكن اجعل DecisionId مرجعي
        public string DecisionId { get { return _decisionId; } set { _decisionId = value ?? Guid.NewGuid().ToString(); } }
        public string SaveId { get { return _decisionId; } set { _decisionId = value ?? Guid.NewGuid().ToString(); } }

        public string Title { get { return _title; } set { _title = value ?? string.Empty; } }
        public string Description { get { return _description; } set { _description = value ?? string.Empty; } }
        public string Status { get { return _status; } set { _status = value ?? string.Empty; } }
        public string ProposedByHeroId { get { return _proposedByHeroId; } set { _proposedByHeroId = value ?? string.Empty; } }
        public DateTime ProposedAtUtc { get { return new DateTime(_proposedTicks, DateTimeKind.Utc); } set { _proposedTicks = value.ToUniversalTime().Ticks; } }

        public IReadOnlyDictionary<string, bool> Votes { get { lock(_lock) return _votes; } }

        public void RecordVote(string heroId, bool vote)
        {
            if (string.IsNullOrEmpty(heroId)) return;
            lock (_lock)
            {
                _votes[heroId] = vote;
            }
        }

        public int GetYeaVotes() { lock(_lock) return _votes.Count(v => v.Value); }
        public int GetNayVotes() { lock(_lock) return _votes.Count(v => !v.Value); }

        public void RehydrateVotes(IGameApi gameApi)
        {
            lock (_lock)
            {
                // This is mainly for validation in case hero IDs become invalid
                var validVotes = new Dictionary<string, bool>();
                foreach (var vote in _votes)
                {
                    if (gameApi.FindHeroByStringId(vote.Key) != null)
                    {
                        validVotes[vote.Key] = vote.Value;
                    }
                }
                _votes = validVotes;
            }
        }

        public override string ToString() => $"Decision[{DecisionId}]: {Title} ({Status})";
    }
}