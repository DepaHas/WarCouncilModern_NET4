using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
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
        private readonly IModLogger _logger;

        public CouncilService(IWarCouncilManager warCouncilManager, IModLogger logger)
        {
            _warCouncilManager = warCouncilManager ?? throw new ArgumentNullException(nameof(warCouncilManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public WarCouncil StartCouncilForKingdom(Kingdom kingdom)
        {
            if (kingdom == null)
            {
                _logger.Warn("[CouncilService] Attempted to start a council for a null kingdom.");
                return null;
            }

            var existingCouncil = _warCouncilManager.GetCouncilByFaction(kingdom.StringId);
            if (existingCouncil != null)
            {
                _logger.Info($"[CouncilService] Council for kingdom '{kingdom.Name}' already exists. Returning existing council.");
                return existingCouncil;
            }

            _logger.Info($"[CouncilService] Creating a new council for kingdom '{kingdom.Name}'.");
            var newCouncil = _warCouncilManager.CreateCouncil(kingdom.StringId, $"{kingdom.Name} War Council", kingdom.Leader.StringId);

            if (newCouncil == null)
            {
                _logger.Error($"[CouncilService] Failed to create council for kingdom '{kingdom.Name}'.");
                return null;
            }

            // Add leader and clan leaders as initial members
            var membersToAdd = kingdom.Clans.Where(c => c.Leader != null).Select(c => c.Leader).ToList();
            if (kingdom.Leader != null && !membersToAdd.Contains(kingdom.Leader))
            {
                membersToAdd.Add(kingdom.Leader);
            }

            _logger.Info($"[CouncilService] Adding {membersToAdd.Count} members to the council.");
            foreach (var hero in membersToAdd)
            {
                var warHero = new WarHero(hero.StringId, hero.Name.ToString());
                _warCouncilManager.AddMemberToCouncil(new Guid(newCouncil.SaveId), warHero);
            }

            CouncilEvents.RaiseCouncilCreated(newCouncil);

            return newCouncil;
        }
    }
}
