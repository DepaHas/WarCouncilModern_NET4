using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using WarCouncilModern_NET4.WarCouncilModern.Core;

namespace WarCouncilModern.Core
{
    public static class FeatureRegistry
    {
        public static void RegisterAll(IGameStarter starter)
        {
            // Register behaviors based on ModSettings
            if (ModSettings.EnableWarCouncilLogic)
            {
                SubModule.SafeAddBehavior(starter, typeof(Behaviors.WarCouncilManagerBehavior));
            }

            if (ModSettings.EnableSaveTests)
            {
                SubModule.SafeAddBehavior(starter, typeof(Save.SaveTests.WarCouncilSaveTestBehavior));
            }

            if (ModSettings.EnableUI)
            {
                UI.UIRegistry.RegisterUI(starter);
            }
        }
    }
}