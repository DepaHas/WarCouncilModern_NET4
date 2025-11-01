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
        WarCouncil CreateCouncil(string kingdomId, string name = "Default Council", string leaderHeroId = "");
        WarCouncil? GetCouncilById(Guid saveId);
        WarCouncil? GetCouncilByFaction(string factionId);
        WarCouncil? FindCouncilById(Guid councilId);
        WarCouncil? FindCouncilByDecisionId(Guid decisionId);
        bool HasActiveCouncilForKingdom(string kingdomId);
        bool RemoveCouncil(Guid saveId);
        bool AddMemberToCouncil(Guid councilId, WarHero hero);
        bool RemoveMemberFromCouncil(Guid councilId, Guid heroSaveId);
        WarDecision ProposeDecision(Guid councilId, string title, string description, Guid proposedBy);
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

        public WarCouncil CreateCouncil(string kingdomId, string name = "Default Council", string leaderHeroId = "")
        {
            var council = new WarCouncil(kingdomId) { Name = name, LeaderHeroId = leaderHeroId };
            _behavior.AddCouncil(council);
            return council;
        }

        public WarCouncil? GetCouncilById(Guid saveId)
        {
            return _behavior.GetCouncilById(saveId.ToString());
        }

        public WarCouncil? GetCouncilByFaction(string factionId)
        {
            return Councils.FirstOrDefault(c => c.KingdomId == factionId);
        }

        public bool HasActiveCouncilForKingdom(string kingdomId)
        {
            return Councils.Any(c => c.KingdomId == kingdomId);
        }

        public bool RemoveCouncil(Guid saveId)
        {
            return _behavior.RemoveCouncil(saveId.ToString());
        }

        public bool AddMemberToCouncil(Guid councilId, WarHero hero)
        {
            var council = GetCouncilById(councilId);
            if (council == null) return false;
            council.AddMember(hero);
            return true;
        }

        public bool RemoveMemberFromCouncil(Guid councilId, Guid heroSaveId)
        {
            var council = GetCouncilById(councilId);
            if (council == null) return false;
            council.RemoveMember(heroSaveId.ToString());
            return true;
        }

        public WarDecision ProposeDecision(Guid councilId, string title, string description, Guid proposedBy)
        {
            var council = GetCouncilById(councilId);
            if (council == null) return null;
            var decision = new WarDecision(Guid.NewGuid().ToString(), title, proposedBy.ToString(), "Proposed") { Description = description };
            council.AddDecision(decision);
            return decision;
        }

        public async Task<bool> ProcessDecisionAsync(Guid councilId, Guid decisionId)
        {
            var council = GetCouncilById(councilId);
            var decision = council?.Decisions.FirstOrDefault(d => new Guid(d.DecisionId) == decisionId);
            if (council == null || decision == null) return false;

            await _decisionService.ProcessDecisionAsync(council, decision);
            return true;
        }

        public void RebuildReferencesAfterLoad(IGameApi gameApi)
        {
            foreach (var council in Councils)
            {
                council.RehydrateReferences(gameApi);
            }
        }

        public WarCouncil? FindCouncilById(Guid councilId)
        {
            return Councils.FirstOrDefault(c => new Guid(c.SaveId) == councilId);
        }

        public WarCouncil? FindCouncilByDecisionId(Guid decisionId)
        {
            return Councils.FirstOrDefault(c => c.Decisions.Any(d => new Guid(d.DecisionId) == decisionId));
        }
    }
}
