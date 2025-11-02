using WarCouncilModern.Models.Entities;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.Core.Decisions
{
    public class ChangeFactionRelationHandler : IExecutionHandler
    {
        private readonly IGameApi _gameApi;
        private readonly IModLogger _logger;
        private readonly int _relationDelta;

        public ChangeFactionRelationHandler(IGameApi gameApi, IModLogger logger, int relationDelta = -20)
        {
            _gameApi = gameApi;
            _logger = logger;
            _relationDelta = relationDelta;
        }

        public bool Execute(WarDecision decision, WarCouncil council)
        {
            var payload = decision.ExecutionPayload;
            var councilId = council.SaveId;
            var decisionId = decision.DecisionId;
            var proposerId = decision.ProposedByHeroId;

            if (payload == null || string.IsNullOrEmpty(payload.TargetFactionId))
            {
                _logger.Warn($"[ChangeFactionRelationHandler] councilId={councilId} decisionId={decisionId} proposerId={proposerId} Execution failed: Missing or invalid payload.");
                return false;
            }

            var targetFactionId = payload.TargetFactionId;
            var targetFaction = _gameApi.FindFactionById(targetFactionId!);
            if (targetFaction == null)
            {
                _logger.Warn($"[ChangeFactionRelationHandler] councilId={councilId} decisionId={decisionId} proposerId={proposerId} Execution failed: Target faction not found (targetFactionId='{targetFactionId}').");
                return false;
            }

            var sourceKingdom = _gameApi.FindKingdomByStringId(council.KingdomStringId);
            if (sourceKingdom == null)
            {
                _logger.Warn($"[ChangeFactionRelationHandler] councilId={councilId} decisionId={decisionId} proposerId={proposerId} Execution failed: Source kingdom not found.");
                return false;
            }

            _gameApi.ChangeRelationBetween(sourceKingdom, targetFaction, _relationDelta);
            _logger.Info($"[ChangeFactionRelationHandler] councilId={councilId} decisionId={decisionId} proposerId={proposerId} Execution success. Payload: {{ targetFactionId='{targetFactionId}', relationDelta={_relationDelta} }}. Applied relation change between {sourceKingdom.StringId} and {targetFactionId}.");
            return true;
        }
    }
}
