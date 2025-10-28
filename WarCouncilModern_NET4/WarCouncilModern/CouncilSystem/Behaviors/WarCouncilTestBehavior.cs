using TaleWorlds.CampaignSystem;

namespace WarCouncilModern
{
    public class WarCouncilTestBehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            ModLogger.Info("WarCouncilTestBehavior.RegisterEvents called");
            // لا تفعل شيئاً معقداً هنا
        }

        public override void SyncData(IDataStore dataStore)
        {
            ModLogger.Info("WarCouncilTestBehavior.SyncData called");
        }
    }
}