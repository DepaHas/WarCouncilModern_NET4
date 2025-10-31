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
            if (string.IsNullOrEmpty(decision.ExecutionPayload?.TargetFactionId))
            {
                _logger.Warn($"[ChangeFactionRelationHandler] decisionId={decision.DecisionId} missing TargetFactionId in payload.");
                return false;
            }

            var targetFactionId = decision.ExecutionPayload.TargetFactionId;
            var targetFaction = _gameApi.FindFactionById(targetFactionId);
            if (targetFaction == null)
            {
                _logger.Warn($"[ChangeFactionRelationHandler] decisionId={decision.DecisionId} could not find target faction with id={targetFactionId}.");
                return false;
            }

            var sourceKingdom = _gameApi.FindKingdomByStringId(council.KingdomStringId);
            if (sourceKingdom == null)
            {
                _logger.Warn($"[ChangeFactionRelationHandler] councilId={council.SaveId} could not find source kingdom.");
                return false;
            }

            _gameApi.ChangeRelationBetween(sourceKingdom, targetFaction, _relationDelta);
            _logger.Info($"[ChangeFactionRelationHandler] decisionId={decision.DecisionId} applied relation change of {_relationDelta} between {sourceKingdom.StringId} and {targetFactionId}.");
            return true;
        }
    }
}
