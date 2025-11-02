using System;
using System.Reflection;
using HarmonyLib;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using WarCouncilModern.Initialization;
using WarCouncilModern.UI.ViewModels.Kingdom;

namespace WarCouncilModern.Patches
{
    [HarmonyPatch(typeof(KingdomVM), "Refresh")]
    public static class KingdomVMPatch
    {
        private static readonly FieldInfo? TabsField = AccessTools.Field(typeof(KingdomVM), "_tabs");

        public static void Postfix(KingdomVM __instance)
        {
            if (SubModule._settings == null || !SubModule._settings.EnableCouncilUI)
            {
                return;
            }

            try
            {
                if (TabsField?.GetValue(__instance) is not MBBindingList<KingdomTabVM> tabs)
                {
                    SubModule.Logger.Warn("KingdomVMPatch: Could not find or access the '_tabs' field.");
                    return;
                }

                foreach (var tab in tabs)
                {
                    if (tab.Identifier == "WarCouncil")
                    {
                        return;
                    }
                }

                var warCouncilVm = new KingdomWarCouncilVM();
                var warCouncilTab = new KingdomTabVM(
                    "War Council",
                    "WarCouncil",
                    "KingdomWarCouncil",
                    warCouncilVm,
                    () => (true, new TextObject("War Council Tab"))
                );

                tabs.Add(warCouncilTab);
                SubModule.Logger.Info("Successfully patched KingdomVM to add War Council tab.");
            }
            catch (Exception ex)
            {
                SubModule.Logger.Error("KingdomVMPatch: An exception occurred while adding the War Council tab.", ex);
            }
        }
    }
}
