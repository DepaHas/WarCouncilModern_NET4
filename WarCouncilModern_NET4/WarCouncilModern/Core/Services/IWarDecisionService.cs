using TaleWorlds.CampaignSystem;
using WarCouncilModern.Models.Entities;

namespace WarCouncilModern.Core.Services
{
    /// <summary>
    /// Manages the lifecycle of decisions within a war council.
    /// </summary>
    public interface IWarDecisionService
    {
        /// <summary>
        /// Proposes a new decision for a council.
        /// </summary>
        /// <param name="council">The council where the decision is proposed.</param>
        /// <param name="title">The title of the decision.</param>
        /// <param name="description">The description of the decision.</param>
        /// <param name="proposer">The hero proposing the decision.</param>
        /// <param name="executionPayload">The data required to execute the decision.</param>
        /// <returns>The newly created WarDecision.</returns>
        WarDecision ProposeDecision(WarCouncil council, string title, string description, Hero proposer, string executionPayload = null);

        /// <summary>
        /// Records a vote from a hero on a specific decision.
        /// </summary>
        /// <param name="decision">The decision being voted on.</param>
        /// <param name="voter">The hero casting the vote.</param>
        /// <param name="vote">The vote (true for 'yea', false for 'nay').</param>
        void RecordVote(WarDecision decision, Hero voter, bool vote);

        /// <summary>
        /// Processes the final outcome of a decision based on votes.
        /// </summary>
        /// <param name="council">The council associated with the decision.</param>
        /// <param name="decision">The decision to process.</param>
        void ProcessDecision(WarCouncil council, WarDecision decision);
    }
}
