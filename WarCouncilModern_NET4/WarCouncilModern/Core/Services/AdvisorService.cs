using WarCouncilModern.Core.Services;
using WarCouncilModern.Models.Entities;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.Core.Services
{
    public class AdvisorService : IAdvisorService
    {
        private readonly IModLogger _logger;

        public AdvisorService(IModLogger logger)
        {
            _logger = logger;
        }

        public void InitializeForCouncil(WarCouncil council)
        {
            _logger.Info("Initializing advisor service for council: " + council.Name);
        }

        public void OnDecisionProposed(WarCouncil council, WarDecision decision)
        {
            _logger.Info("Advisor service notified of proposed decision: " + decision.Title);
        }
    }
}
