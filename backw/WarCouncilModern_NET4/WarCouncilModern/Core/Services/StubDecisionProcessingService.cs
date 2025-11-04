using System.Threading.Tasks;
using WarCouncilModern.Core.Services;
using WarCouncilModern.Models.Entities;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.Core.Services
{
    public class StubDecisionProcessingService : IDecisionProcessingService
    {
        private readonly IModLogger _logger;

        public StubDecisionProcessingService(IModLogger logger)
        {
            _logger = logger;
        }

        public Task<bool> ProcessDecisionAsync(WarCouncil council, WarDecision decision)
        {
            _logger.Info($"[StubDecisionProcessingService] Processing decision {decision.SaveId} (stub).");
            // Simple deterministic stub: accept all decisions
            decision.Status = "Executed";
            return Task.FromResult(true);
        }
    }
}