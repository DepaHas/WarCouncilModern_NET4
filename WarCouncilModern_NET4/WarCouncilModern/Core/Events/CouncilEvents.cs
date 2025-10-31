using System;
using WarCouncilModern.Models.Entities;

namespace WarCouncilModern.Core.Events
{
    /// <summary>
    /// Provides static events for council-related activities.
    /// </summary>
    public static class CouncilEvents
    {
        /// <summary>
        /// Fired when a new council is created.
        /// </summary>
        public static event Action<WarCouncil> OnCouncilCreated;

        internal static void RaiseCouncilCreated(WarCouncil council)
        {
            OnCouncilCreated?.Invoke(council);
        }
    }
}
