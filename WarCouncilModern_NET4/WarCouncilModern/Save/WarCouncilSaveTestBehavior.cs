using System;
using TaleWorlds.CampaignSystem;
using WarCouncilModern.Core;
using WarCouncilModern.Logging;

namespace WarCouncilModern.Save.SaveTests
{
    public class WarCouncilSaveTestBehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            try
            {
                if (!ModSettings.EnableSaveTests) return;
                ModLogger.Info("[SaveTest] RegisterEvents");
            }
            catch (Exception ex) { ModLogger.Error("[SaveTest] RegisterEvents error: " + ex); }
        }

        public override void SyncData(IDataStore dataStore)
        {
            try
            {
                var entity = new SaveableWarCouncilData { SomeValue = 42, Tag = "test" };
                dataStore.SyncData("WarCouncil.SaveTest.Entity", ref entity);
                ModLogger.Info("[SaveTest] SyncData completed");
            }
            catch (Exception ex) { ModLogger.Error("[SaveTest] SyncData error: " + ex); }
        }
    }
}