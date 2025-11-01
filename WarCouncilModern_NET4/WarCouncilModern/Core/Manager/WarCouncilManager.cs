using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarCouncilModern.Core.Services;
using WarCouncilModern.Core.Settings;
using WarCouncilModern.Core.State;
using WarCouncilModern.CouncilSystem.Behaviors;
using WarCouncilModern.Models;
using WarCouncilModern.Models.Entities;
using WarCouncilModern.Utilities;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.Core.Manager
{
    public interface IWarCouncilManager
    {
        IReadOnlyList<WarCouncil> Councils { get; }
        WarCouncil? CreateCouncil(string kingdomId, string name = "Default Council", string leaderHeroId = "");
        WarCouncil? GetCouncilById(Guid saveId);
        WarCouncil? GetCouncilByFaction(string factionId);
        bool HasActiveCouncilForKingdom(string kingdomId);
        bool RemoveCouncil(Guid saveId);
        bool AddMemberToCouncil(Guid councilId, WarHero hero);
        bool RemoveMemberFromCouncil(Guid councilId, Guid heroSaveId);
        WarDecision? ProposeDecision(Guid councilId, string title, string description, Guid proposedBy);
        Task<bool> ProcessDecisionAsync(Guid councilId, Guid decisionId);
        void RebuildReferencesAfterLoad(IGameApi gameApi);
    }

    public class WarCouncilManager : IWarCouncilManager
    {
        private readonly WarCouncilCampaignBehavior _behavior;
        private readonly object _locker = new object();

        private readonly ICouncilMeetingService _meetingService;
        private readonly IDecisionProcessingService _decisionService;
        private readonly IAdvisorService _advisorService;
        private readonly IModSettings _settings;
        private readonly IModLogger _logger;
        private readonly IModStateTracker _stateTracker;
        private readonly IFeatureRegistry _featureRegistry;

        public IReadOnlyList<WarCouncil> Councils
        {
            get
            {
                lock (_locker) return _behavior.Councils.Values.ToList().AsReadOnly();
            }
        }

        public WarCouncilManager(
            WarCouncilCampaignBehavior behavior,
            ICouncilMeetingService meetingService,
            IDecisionProcessingService decisionService,
            IAdvisorService advisorService,
            IModSettings settings,
            IModLogger logger,
            IModStateTracker stateTracker,
            IFeatureRegistry featureRegistry)
        {
            _behavior = behavior ?? throw new ArgumentNullException(nameof(behavior));
            _meetingService = meetingService ?? throw new ArgumentNullException(nameof(meetingService));
            _decisionService = decisionService ?? throw new ArgumentNullException(nameof(decisionService));
            _advisorService = advisorService ?? throw new ArgumentNullException(nameof(advisorService));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _stateTracker = stateTracker ?? throw new ArgumentNullException(nameof(stateTracker));
            _featureRegistry = featureRegistry ?? throw new ArgumentNullException(nameof(featureRegistry));
        }

        public WarCouncil? CreateCouncil(string kingdomId, string name = "Default Council", string leaderHeroId = "")
        {
            if (string.IsNullOrEmpty(kingdomId))
            {
                throw new ArgumentNullException(nameof(kingdomId));
            }

            lock (_locker)
            {
                if (_behavior.Councils.Values.Any(c => c.KingdomStringId == kingdomId))
                {
                    _logger.Warn($"[WarCouncilManager] Council for kingdom '{kingdomId}' already exists.");
                    return null;
                }

                var council = new WarCouncil(kingdomId, CouncilStructure.Standard)
                {
                    Name = name
                };
                council.AssignLeaderByHeroId(leaderHeroId);

                _behavior.Councils.Add(council.SaveId, council);
                _logger.Info($"[WarCouncilManager] Created council '{council.Name}' for kingdom '{kingdomId}' ({council.SaveId}).");
                _stateTracker.RecordCouncilCreated();
                return council;
            }
        }

        public WarCouncil? GetCouncilById(Guid saveId)
        {
            lock (_locker)
            {
                _behavior.Councils.TryGetValue(saveId.ToString(), out var council);
                return council;
            }
        }

        public WarCouncil? GetCouncilByFaction(string factionId)
        {
            lock (_locker)
            {
                return _behavior.Councils.Values.FirstOrDefault(c => c.KingdomStringId == factionId);
            }
        }

        public bool HasActiveCouncilForKingdom(string kingdomId)
        {
            lock (_locker)
            {
                return _behavior.Councils.Values.Any(c => c.KingdomStringId == kingdomId);
            }
        }

        public bool RemoveCouncil(Guid saveId)
        {
            lock (_locker)
            {
                if (_behavior.Councils.Remove(saveId.ToString()))
                {
                    _logger.Info($"[WarCouncilManager] Removed council '{saveId}'.");
                    return true;
                }
                return false;
            }
        }

        public bool AddMemberToCouncil(Guid councilId, WarHero hero)
        {
            var council = GetCouncilById(councilId);
            if (council == null) return false;
            council.AddMemberById(hero.SaveId.ToString());
            return true;
        }

        public bool RemoveMemberFromCouncil(Guid councilId, Guid heroSaveId)
        {
            var council = GetCouncilById(councilId);
            if (council == null) return false;
            return council.RemoveMemberById(heroSaveId.ToString());
        }

        public WarDecision? ProposeDecision(Guid councilId, string title, string description, Guid proposedBy)
        {
            var council = GetCouncilById(councilId);
            if (council == null) return null;
            var decision = new WarDecision(title, description, proposedBy.ToString());
            council.AddDecision(decision);
            _stateTracker.RecordDecisionProposed();
            return decision;
        }

        public Task<bool> ProcessDecisionAsync(Guid councilId, Guid decisionId)
        {
            return _decisionService.ProcessDecisionAsync(councilId, decisionId);
        }

        public void RebuildReferencesAfterLoad(IGameApi gameApi)
        {
            lock (_locker)
            {
                foreach (var council in _behavior.Councils.Values)
                {
                    council.RebuildMembers(gameApi);
                }
            }
        }
    }
}
