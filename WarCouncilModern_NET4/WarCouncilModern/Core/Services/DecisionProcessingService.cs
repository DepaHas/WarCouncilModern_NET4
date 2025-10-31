using System.Threading.Tasks;
using WarCouncilModern.Core.Services;
using WarCouncilModern.Models.Entities;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.Core.Services
{
    public class DecisionProcessingService : IDecisionProcessingService
    {
        private readonly IModLogger _logger;

        public DecisionProcessingService(IModLogger logger)
        {
            _logger = logger;
        }

        public Task<bool> ProcessDecisionAsync(WarCouncil council, WarDecision decision)
        {
            _logger.Log("Processing decision: " + decision.Title);
            return Task.FromResult(true);
        }
    }
}
