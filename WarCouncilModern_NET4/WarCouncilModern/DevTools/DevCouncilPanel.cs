using System;
using TaleWorlds.CampaignSystem;
using WarCouncilModern.Core.Council;
using WarCouncilModern.Core.Services;
using WarCouncilModern.Models.Entities;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.DevTools
{
    public class DevCouncilPanel
    {
        private readonly ICouncilService _councilService;
        private readonly IWarDecisionService _decisionService;
        private readonly IModLogger _logger;

        public DevCouncilPanel(ICouncilService councilService, IWarDecisionService decisionService, IModLogger logger)
        {
            _councilService = councilService;
            _decisionService = decisionService;
            _logger = logger;
        }

        public void ShowHelp()
        {
            _logger.Info("[DevPanel] Commands: CreateCouncil(kingdomId), ProposeDecision(council, title, description, proposer), RecordVote(decision, voter, vote), ProcessDecision(council, decision)");
        }

        public WarCouncil CreateCouncil(Kingdom kingdom)
        {
            var council = _councilService.StartCouncilForKingdom(kingdom);
            _logger.Info($"[DevPanel] Created council {council.SaveId} for kingdom {kingdom.StringId}");
            return council;
        }

        public WarDecision ProposeDecision(WarCouncil council, string title, string description, Hero proposer, string targetFactionId = null)
        {
            var decision = _decisionService.ProposeDecision(council, title, description, proposer);
            if (decision != null && targetFactionId != null)
            {
                decision.ExecutionPayload = new DecisionExecutionPayload { TargetFactionId = targetFactionId };
            }
            _logger.Info($"[DevPanel] Proposed decision {decision?.DecisionId} in council {council.SaveId}");
            return decision;
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
