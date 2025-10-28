using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.SaveSystem;
using TaleWorlds.CampaignSystem;
using WarCouncilModern.Models;

namespace WarCouncilModern.Models.Entities

{
    public class WarCouncil
    {
        [SaveableField(1)] private string _kingdomStringId;
        [SaveableField(2)] private CouncilStructure _structure;
        [SaveableField(3)] private string _leaderHeroId;
        [SaveableField(4)] private List<string> _memberHeroIds;
        [SaveableField(5)] private List<WarDecision> _decisions;
        [SaveableField(6)] private List<WarReport> _reports;

        private readonly object _lock = new object();

        public WarCouncil()
        {
            _kingdomStringId = string.Empty;
            _structure = CouncilStructure.RoyalAppointed;
            _leaderHeroId = string.Empty;
            _memberHeroIds = new List<string>();
            _decisions = new List<WarDecision>();
            _reports = new List<WarReport>();
        }

        public WarCouncil(string kingdomId, CouncilStructure structure)
        {
            _kingdomStringId = kingdomId ?? string.Empty;
            _structure = structure;
            _leaderHeroId = string.Empty;
            _memberHeroIds = new List<string>();
            _decisions = new List<WarDecision>();
            _reports = new List<WarReport>();
        }

        public string KingdomStringId { get { lock (_lock) { return _kingdomStringId; } } set { lock (_lock) { _kingdomStringId = value ?? string.Empty; } } }
        public CouncilStructure Structure { get { lock (_lock) { return _structure; } } set { lock (_lock) { _structure = value; } } }

        public string LeaderHeroId { get { lock (_lock) { return _leaderHeroId; } } private set { lock (_lock) { _leaderHeroId = value ?? string.Empty; } } }
        public IReadOnlyList<string> MemberHeroIds { get { lock (_lock) { return _memberHeroIds.ToArray(); } } }
        public IReadOnlyList<WarDecision> Decisions { get { lock (_lock) { return _decisions.ToArray(); } } }
        public IReadOnlyList<WarReport> Reports { get { lock (_lock) { return _reports.ToArray(); } } }

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

        public void AddReport(WarReport r)
        {
            if (r == null) return;
            lock (_lock) _reports.Add(r);
        }

        public TaleWorlds.CampaignSystem.Hero GetLeaderGameHero()
        {
            string id;
            lock (_lock) { id = _leaderHeroId; }
            if (string.IsNullOrEmpty(id)) return null;

            try
            {
                var campaign = Campaign.Current;
                if (campaign != null)
                {
                    try
                    {
                        var objManager = campaign.ObjectManager;
                        if (objManager != null)
                        {
                            var obj = objManager.GetObject<Hero>(id);
                            if (obj != null) return obj;
                        }
                    }
                    catch { }
                }

                foreach (var kingdom in Kingdom.All)
                {
                    if (kingdom == null) continue;
                    var found = kingdom.Heroes != null ? kingdom.Heroes.FirstOrDefault(h => string.Equals(h != null ? h.StringId : null, id, StringComparison.Ordinal)) : null;
                    if (found != null) return found;
                }

                foreach (var clan in Clan.All)
                {
                    if (clan == null) continue;
                    var found = clan.Heroes != null ? clan.Heroes.FirstOrDefault(h => string.Equals(h != null ? h.StringId : null, id, StringComparison.Ordinal)) : null;
                    if (found != null) return found;
                    if (clan.Leader != null && string.Equals(clan.Leader.StringId, id, StringComparison.Ordinal)) return clan.Leader;
                }
            }
            catch { }

            return null;
        }

        public IEnumerable<TaleWorlds.CampaignSystem.Hero> GetMemberGameHeroes()
        {
            List<string> ids;
            lock (_lock) { ids = new List<string>(_memberHeroIds); }
            if (ids.Count == 0) return new TaleWorlds.CampaignSystem.Hero[0];

            var results = new List<TaleWorlds.CampaignSystem.Hero>();

            try
            {
                var campaign = Campaign.Current;
                if (campaign != null)
                {
                    try
                    {
                        var objManager = campaign.ObjectManager;
                        if (objManager != null)
                        {
                            foreach (var id in ids)
                            {
                                if (string.IsNullOrEmpty(id)) continue;
                                var obj = objManager.GetObject<Hero>(id);
                                if (obj != null) results.Add(obj);
                            }
                        }
                    }
                    catch { }
                }

                var remaining = ids.Except(results.Select(h => h.StringId)).ToList();
                if (remaining.Count > 0)
                {
                    foreach (var kingdom in Kingdom.All)
                    {
                        if (kingdom == null) continue;
                        foreach (var h in kingdom.Heroes ?? Enumerable.Empty<Hero>())
                        {
                            if (h == null) continue;
                            if (remaining.Contains(h.StringId))
                            {
                                results.Add(h);
                                remaining.Remove(h.StringId);
                                if (remaining.Count == 0) break;
                            }
                        }
                        if (remaining.Count == 0) break;
                    }
                }

                if (remaining.Count > 0)
                {
                    foreach (var clan in Clan.All)
                    {
                        if (clan == null) continue;
                        foreach (var h in clan.Heroes ?? Enumerable.Empty<Hero>())
                        {
                            if (h == null) continue;
                            if (remaining.Contains(h.StringId))
                            {
                                results.Add(h);
                                remaining.Remove(h.StringId);
                                if (remaining.Count == 0) break;
                            }
                        }
                        if (remaining.Count == 0) break;
                    }
                }
            }
            catch { }

            return results;
        }

        public bool IsValid()
        {
            lock (_lock) return !string.IsNullOrEmpty(_kingdomStringId);
        }

        public override string ToString()
        {
            lock (_lock) return string.Format("WarCouncil: {0} members, {1} decisions, {2} reports", _memberHeroIds.Count, _decisions.Count, _reports.Count);
        }
    }
}