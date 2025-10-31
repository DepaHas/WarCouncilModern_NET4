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
        private readonly IWarDecisionService _decisionService;
        private readonly IModLogger _logger;

        public DevCouncilPanel(ICouncilService councilService, ICouncilUiService councilUiService, IWarDecisionService decisionService, IModLogger logger)
        {
            _councilService = councilService;
            _councilUiService = councilUiService;
            _decisionService = decisionService;
            _logger = logger;
        }

        public void ShowHelp()
        {
            _logger.Info("[DevPanel] Commands: CreateCouncil(kingdomId), ProposeDecision(councilId, title, description, payload), RecordVote(decision, voter, vote), ProcessDecision(council, decision)");
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
            _logger.Info($"[DevPanel] Decision proposal request sent for council {councilId}. Check UI for updates.");
        }


        public void RecordVote(WarDecision decision, Hero voter, bool vote)
        {
            _decisionService.RecordVote(decision, voter, vote);
            _logger.Info($"[DevPanel] Vote recorded for decision {decision.DecisionId} by {voter.StringId}.");
        }

        public void ProcessDecision(WarCouncil council, WarDecision decision)
        {
            _decisionService.ProcessDecision(council, decision);
            _logger.Info($"[DevPanel] Processed decision {decision.DecisionId}. Final status: {decision.Status}");
        }
    }
}
