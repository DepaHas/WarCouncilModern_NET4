using System;
using WarCouncilModern.Core.Services;
using WarCouncilModern.Models.Entities;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.Core.Services
{
    public class StubAdvisorService : IAdvisorService
    {
        private readonly IModLogger _logger;

        public StubAdvisorService(IModLogger logger)
        {
            _logger = logger;
        }

        public void InitializeForCouncil(WarCouncil council)
        {
            _logger.Info($"[StubAdvisorService] InitializeForCouncil called for {council.SaveId}.");
            // In future: create default advisors, assign roles, compute influence.
        }

        public void OnDecisionProposed(WarCouncil council, WarDecision decision)
        {
            _logger.Debug($"[StubAdvisorService] OnDecisionProposed: {decision.SaveId} in council {council.SaveId}.");
            // In future: provide recommendations, adjust scores.
        }
    }
}