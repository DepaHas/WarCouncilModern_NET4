using WarCouncilModern.CouncilSystem;
using WarCouncilModern.Models.Entities;

namespace WarCouncilModern.Core.Services
{
    public interface IAdvisorService
    {
        void InitializeForCouncil(WarCouncil council);
        void OnDecisionProposed(WarCouncil council, WarDecision decision);
    }
}