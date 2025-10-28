using System;
using TaleWorlds.CampaignSystem;
using WarCouncilModern.Logging;

namespace WarCouncilModern.Behaviors
{
    public class BehaviorTemplate : CampaignBehaviorBase
    {
        public BehaviorTemplate() { }

        public override void RegisterEvents()
        {
            try
            {
                ModLogger.Info(LogFormats.Phase("Behavior", nameof(BehaviorTemplate), "RegisterEvents", "registered"));
            }
            catch (Exception ex) { ModLogger.Error("[BehaviorTemplate] " + ex); }
        }

        public override void SyncData(IDataStore dataStore)
        {
            try { }
            catch (Exception ex) { ModLogger.Error("[BehaviorTemplate] SyncData " + ex); }
        }
    }
}