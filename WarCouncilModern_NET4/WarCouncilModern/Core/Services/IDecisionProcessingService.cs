using System.Threading.Tasks;
using WarCouncilModern.CouncilSystem;
using WarCouncilModern.Models.Entities;

namespace WarCouncilModern.Core.Services
{
    public interface IDecisionProcessingService
    {
        Task<bool> ProcessDecisionAsync(WarCouncil council, WarDecision decision);
    }
}