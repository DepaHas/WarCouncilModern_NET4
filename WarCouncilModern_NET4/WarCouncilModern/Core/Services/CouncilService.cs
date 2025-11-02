using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using WarCouncilModern.Core.Council;
using WarCouncilModern.Core.Events;
using WarCouncilModern.Core.Manager;
using WarCouncilModern.Models;
using WarCouncilModern.Models.Entities;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.Core.Services
{
    public class CouncilService : ICouncilService
    {
        private readonly IWarCouncilManager _warCouncilManager;
        private readonly ICouncilMemberSelector _memberSelector;
        private readonly IModLogger _logger;

        public CouncilService(IWarCouncilManager warCouncilManager, ICouncilMemberSelector memberSelector, IModLogger logger)
        {
            _warCouncilManager = warCouncilManager ?? throw new ArgumentNullException(nameof(warCouncilManager));
            _memberSelector = memberSelector ?? throw new ArgumentNullException(nameof(memberSelector));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public WarCouncil? StartCouncilForKingdom(Kingdom kingdom)
        {
            if (kingdom == null)
            {
                _logger.Warn("[CouncilService] Attempted to start a council for a null kingdom.");
                return null;
            }

            var existingCouncil = _warCouncilManager.GetCouncilByFaction(kingdom.StringId);
            if (existingCouncil != null)
            {
                _logger.Info($"[CouncilService] councilId={existingCouncil.SaveId} kingdom='{kingdom.Name}' already exists.");
                return existingCouncil;
            }

            _logger.Info($"[CouncilService] Creating a new council for kingdom '{kingdom.Name}'.");
            var newCouncil = _warCouncilManager.CreateCouncil(kingdom.StringId, $"{kingdom.Name} War Council", kingdom.Leader.StringId);

            if (newCouncil == null)
            {
                _logger.Error($"[CouncilService] Failed to create council for kingdom '{kingdom.Name}'.");
                return null;
            }

            var memberIds = _memberSelector.GetMembersForCouncil(kingdom.StringId, new CouncilContext { Reason = "InitialCreation" });
            _logger.Info($"[CouncilService] councilId={newCouncil.SaveId} Adding {memberIds.Count()} members.");
            foreach (var memberId in memberIds)
            {
                var warHero = new WarHero(memberId, "Unknown"); // Name will be resolved later if needed
                _warCouncilManager.AddMemberToCouncil(new Guid(newCouncil.SaveId), warHero);
            }

            CouncilEvents.RaiseCouncilCreated(newCouncil);

            return newCouncil;
        }
    }
}
