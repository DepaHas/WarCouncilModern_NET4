using TaleWorlds.CampaignSystem;
using WarCouncilModern.Models.Entities;

namespace WarCouncilModern.Core.Services
{
    /// <summary>
    /// Manages the game logic for war councils, such as creation and member selection.
    /// </summary>
    public interface ICouncilService
    {
        /// <summary>
        /// Starts a new council for a specific kingdom if one does not already exist.
        /// </summary>
        /// <param name="kingdom">The kingdom to create the council for.</param>
        /// <returns>The newly created or existing WarCouncil instance.</returns>
        WarCouncil StartCouncilForKingdom(Kingdom kingdom);
    }
}
