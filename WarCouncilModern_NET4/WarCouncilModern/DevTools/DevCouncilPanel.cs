using System;
using TaleWorlds.CampaignSystem;
using WarCouncilModern.Core.Council;
using WarCouncilModern.Core.Services;
using WarCouncilModern.Models.Entities;
using WarCouncilModern.UI.Services;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.DevTools
{
    public class DevCouncilPanel
    {
        private readonly ICouncilService _councilService;
        private readonly ICouncilUiService _councilUiService;
        private readonly IModLogger _logger;

        public DevCouncilPanel(ICouncilService councilService, ICouncilUiService councilUiService, IModLogger logger)
        {
            _councilService = councilService;
            _councilUiService = councilUiService;
            _logger = logger;
        }

        public void ShowHelp()
        {
            _logger.Info("[DevPanel] Commands: CreateCouncil(kingdom), ProposeDecision(councilId, title, desc, payload), CastVote(councilId, decisionId, vote), TallyDecision(councilId, decisionId)");
        }

        public WarCouncil CreateCouncil(Kingdom kingdom)
        {
            var council = _councilService.StartCouncilForKingdom(kingdom);
            _logger.Info($"[DevPanel] Created council {council.SaveId} for kingdom {kingdom.StringId}");
            return council;
        }

        public async void ProposeDecision(Guid councilId, string title, string description, string payload)
        {
            _logger.Info($"[DevPanel] Requesting to propose decision '{title}' in council {councilId}...");
            await _councilUiService.ProposeDecisionAsync(councilId, title, description, payload);
            _logger.Info($"[DevPanel] Decision proposal request sent for council {councilId}.");
        }

        public async void CastVote(Guid councilId, Guid decisionId, bool vote)
        {
            _logger.Info($"[DevPanel] Casting vote '{vote}' on decision {decisionId} in council {councilId}...");
            await _councilUiService.CastVoteAsync(councilId, decisionId, vote);
            _logger.Info($"[DevPanel] Vote cast request sent for decision {decisionId}.");
        }

        public async void TallyDecision(Guid councilId, Guid decisionId)
        {
            _logger.Info($"[DevPanel] Requesting tally for decision {decisionId} in council {councilId}...");
            await _councilUiService.RequestTallyAndExecuteAsync(councilId, decisionId);
            _logger.Info($"[DevPanel] Tally request sent for decision {decisionId}.");
        }
    }
}
