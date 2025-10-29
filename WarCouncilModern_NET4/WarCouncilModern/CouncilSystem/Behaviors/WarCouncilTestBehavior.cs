using TaleWorlds.CampaignSystem;
using WarCouncilModern.Initialization;

namespace WarCouncilModern
{
    public class WarCouncilTestBehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            SubModule.Logger.Info("WarCouncilTestBehavior.RegisterEvents called");
            // لا تفعل شيئاً معقداً هنا
        }

        public override void SyncData(IDataStore dataStore)
        {
            SubModule.Logger.Info("WarCouncilTestBehavior.SyncData called");
        }
    }
}