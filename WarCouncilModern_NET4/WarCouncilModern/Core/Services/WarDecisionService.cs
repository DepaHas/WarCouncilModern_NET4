using System;
using TaleWorlds.CampaignSystem;
using WarCouncilModern.Core.Events;
using WarCouncilModern.Core.Manager;
using WarCouncilModern.Core.State;
using WarCouncilModern.Models.Entities;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.Core.Services
{
    public class WarDecisionService : IWarDecisionService
    {
        private readonly IWarCouncilManager _warCouncilManager;
        private readonly IFeatureRegistry _featureRegistry;
        private readonly IModLogger _logger;

        public WarDecisionService(IWarCouncilManager warCouncilManager, IFeatureRegistry featureRegistry, IModLogger logger)
        {
            _warCouncilManager = warCouncilManager ?? throw new ArgumentNullException(nameof(warCouncilManager));
            _featureRegistry = featureRegistry ?? throw new ArgumentNullException(nameof(featureRegistry));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public WarDecision ProposeDecision(WarCouncil council, string title, string description, Hero proposer)
        {
            if (council == null || proposer == null || string.IsNullOrWhiteSpace(title))
            {
                _logger.Warn("[WarDecisionService] ProposeDecision called with invalid arguments.");
                return null;
            }

            var decision = _warCouncilManager.ProposeDecision(new Guid(council.SaveId), title, description, new Guid(proposer.StringId));

            if (decision != null)
            {
                CouncilEvents.RaiseDecisionProposed(council, decision);
                _logger.Info($"[WarDecisionService] Decision '{title}' proposed in council for {council.KingdomStringId}.");
            }
            else
            {
                _logger.Error($"[WarDecisionService] Failed to propose decision '{title}' in council for {council.KingdomStringId}.");
            }

            return decision;
        }

        public void RecordVote(WarDecision decision, Hero voter, bool vote)
        {
            if (decision == null || voter == null)
            {
                _logger.Warn("[WarDecisionService] RecordVote called with invalid arguments.");
                return;
            }

            decision.RecordVote(voter.StringId, vote);
            _logger.Info($"[WarDecisionService] Vote recorded for '{voter.Name}' on decision '{decision.Title}'. Vote: {(vote ? "Yea" : "Nay")}");
        }

        public void ProcessDecision(WarCouncil council, WarDecision decision)
        {
            if (council == null || decision == null)
            {
                _logger.Warn("[WarDecisionService] ProcessDecision called with invalid arguments.");
                return;
            }

            if (_featureRegistry.IsEnabled(FeatureKeys.AutoDecisionProcessing))
            {
                _logger.Info($"[WarDecisionService] Auto-processing decision '{decision.Title}' due to feature flag.");
                ExecuteDecision(council, decision);
                return;
            }

            int yeaVotes = decision.GetYeaVotes();
            int nayVotes = decision.GetNayVotes();

            if (yeaVotes > nayVotes)
            {
                decision.Status = "Approved";
                _logger.Info($"[WarDecisionService] Decision '{decision.Title}' was approved with {yeaVotes} to {nayVotes}.");

                // Placeholder for execution logic
                ExecuteDecision(council, decision);
            }
            else
            {
                decision.Status = "Rejected";
                _logger.Info($"[WarDecisionService] Decision '{decision.Title}' was rejected with {yeaVotes} to {nayVotes}.");
            }
        }

        private void ExecuteDecision(WarCouncil council, WarDecision decision)
        {
            // This is a placeholder for where the actual game-changing logic would go.
            // For now, we'll just update the status and raise an event.
            decision.Status = "Executed";
            CouncilEvents.RaiseDecisionExecuted(council, decision);
            _logger.Info($"[WarDecisionService] Decision '{decision.Title}' has been executed for council {council.KingdomStringId}.");
        }
    }
}
