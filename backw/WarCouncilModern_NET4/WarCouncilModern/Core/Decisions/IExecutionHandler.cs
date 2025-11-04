using WarCouncilModern.Models.Entities;

namespace WarCouncilModern.Core.Decisions
{
    public interface IExecutionHandler
    {
        /// <summary>
        /// Executes the effect of a decision. Returns true if successful, false otherwise.
        /// </summary>
        bool Execute(WarDecision decision, WarCouncil council);
    }
}
