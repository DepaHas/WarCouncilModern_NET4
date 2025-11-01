using System;
using TaleWorlds.CampaignSystem;
using WarCouncilModern.Core.Events;
using WarCouncilModern.Core.Decisions;
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
        private readonly IExecutionHandler _executionHandler;
        private readonly IModLogger _logger;

        public WarDecisionService(IWarCouncilManager warCouncilManager, IFeatureRegistry featureRegistry, IExecutionHandler executionHandler, IModLogger logger)
        {
            _warCouncilManager = warCouncilManager ?? throw new ArgumentNullException(nameof(warCouncilManager));
            _featureRegistry = featureRegistry ?? throw new ArgumentNullException(nameof(featureRegistry));
            _executionHandler = executionHandler ?? throw new ArgumentNullException(nameof(executionHandler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public WarDecision ProposeDecision(WarCouncil council, string title, string description, Hero proposer, string executionPayload = null)
        {
            if (council == null || proposer == null || string.IsNullOrWhiteSpace(title))
            {
                _logger.Warn("[WarDecisionService] ProposeDecision called with invalid arguments.");
                return null;
            }

            var decision = _warCouncilManager.ProposeDecision(new Guid(council.SaveId), title, description, new Guid(proposer.StringId));
            if (decision != null)
            {
                if (!string.IsNullOrWhiteSpace(executionPayload))
                {
                    decision.ExecutionPayload = new DecisionExecutionPayload { RawPayload = executionPayload };
                }

                CouncilEvents.RaiseDecisionProposed(council, decision);
                _logger.Info($"[WarDecisionService] councilId={council.SaveId} decisionId={decision.DecisionId} title='{title}' proposed by heroId={proposer.StringId}. Payload attached: {!string.IsNullOrWhiteSpace(executionPayload)}");
            }
            else
            {
                _logger.Error($"[WarDecisionService] councilId={council.SaveId} Failed to propose decision '{title}'.");
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
            var council = _warCouncilManager.FindCouncilByDecisionId(new Guid(decision.DecisionId));
            if (council != null)
            {
                CouncilEvents.RaiseVoteRecorded(council, decision, voter.StringId, vote);
            }
            _logger.Info($"[WarDecisionService] councilId={decision.DecisionId} voter={voter.StringId} vote={(vote ? "Yea" : "Nay")}.");
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
                _logger.Info($"[WarDecisionService] councilId={council.SaveId} decisionId={decision.DecisionId} Auto-processing due to feature flag.");
                ExecuteDecision(council, decision);
                return;
            }

            int yeaVotes = decision.GetYeaVotes();
            int nayVotes = decision.GetNayVotes();

            if (yeaVotes > nayVotes)
            {
                decision.Status = "Approved";
                _logger.Info($"[WarDecisionService] councilId={council.SaveId} decisionId={decision.DecisionId} was approved with {yeaVotes} to {nayVotes}.");

                // Placeholder for execution logic
                ExecuteDecision(council, decision);
            }
            else
            {
                decision.Status = "Rejected";
                _logger.Info($"[WarDecisionService] councilId={council.SaveId} decisionId={decision.DecisionId} was rejected with {yeaVotes} to {nayVotes}.");
            }
            CouncilEvents.RaiseDecisionProcessed(council, decision);
        }

        private void ExecuteDecision(WarCouncil council, WarDecision decision)
        {
            if (_executionHandler.Execute(decision, council))
            {
                decision.Status = "Executed";
                CouncilEvents.RaiseDecisionExecuted(council, decision);
                _logger.Info($"[WarDecisionService] councilId={council.SaveId} decisionId={decision.DecisionId} has been executed.");
            }
            else
            {
                decision.Status = "ExecutionFailed";
                _logger.Warn($"[WarDecisionService] councilId={council.SaveId} decisionId={decision.DecisionId} execution failed.");
            }
        }
    }
}
