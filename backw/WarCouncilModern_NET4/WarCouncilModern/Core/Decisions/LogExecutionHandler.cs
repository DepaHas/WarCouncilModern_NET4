using WarCouncilModern.Models.Entities;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.Core.Decisions
{
    public class LogExecutionHandler : IExecutionHandler
    {
        private readonly IModLogger _logger;

        public LogExecutionHandler(IModLogger logger)
        {
            _logger = logger;
        }

        public bool Execute(WarDecision decision, WarCouncil council)
        {
            _logger.Info($"[DecisionExecute] decisionId={decision.DecisionId} councilId={council.SaveId} title='{decision.Title}' executed.");
            return true;
        }
    }
}
