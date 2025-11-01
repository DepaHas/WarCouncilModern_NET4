using System;
using WarCouncilModern.Models.Entities;

namespace WarCouncilModern.Core.Events
{
    /// <summary>
    /// Provides static events for council-related activities.
    /// </summary>
    public static class CouncilEvents
    {
        // Council Lifecycle Events
        public static event Action<WarCouncil> OnCouncilCreated;
        public static event Action<WarCouncil> OnCouncilEnded;

        // Decision Lifecycle Events
        public static event Action<WarCouncil, WarDecision> OnDecisionProposed;
        public static event Action<WarCouncil, WarDecision, string, bool> OnVoteRecorded;
        public static event Action<WarCouncil, WarDecision> OnDecisionProcessed;
        public static event Action<WarCouncil, WarDecision> OnDecisionExecuted;


        internal static void RaiseCouncilCreated(WarCouncil council)
        {
            OnCouncilCreated?.Invoke(council);
        }

        internal static void RaiseCouncilEnded(WarCouncil council)
        {
            OnCouncilEnded?.Invoke(council);
        }

        internal static void RaiseDecisionProposed(WarCouncil council, WarDecision decision)
        {
            OnDecisionProposed?.Invoke(council, decision);
        }

        internal static void RaiseDecisionExecuted(WarCouncil council, WarDecision decision)
        {
            OnDecisionExecuted?.Invoke(council, decision);
        }

        internal static void RaiseVoteRecorded(WarCouncil council, WarDecision decision, string voterId, bool vote)
        {
            OnVoteRecorded?.Invoke(council, decision, voterId, vote);
        }

        internal static void RaiseDecisionProcessed(WarCouncil council, WarDecision decision)
        {
            OnDecisionProcessed?.Invoke(council, decision);
        }
    }
}
