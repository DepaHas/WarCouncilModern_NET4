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

        // ... (rest of the file is unchanged)
    }
}
