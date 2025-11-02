using System;
using HarmonyLib;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using WarCouncilModern.Initialization;
using WarCouncilModern.UI.ViewModels.Kingdom;

namespace WarCouncilModern.Patches
{
    [HarmonyPatch(typeof(KingdomManagementVM), MethodType.Constructor)]
    public static class KingdomManagementVMPatch
    {
        public static void Postfix(KingdomManagementVM __instance, Action<KingdomItemVM> ____onItemSelection, Action ____onClose)
        {
            if (SubModule._settings == null || !SubModule._settings.EnableCouncilUI)
            {
                return;
            }

            try
            {
                var kingdomItemVMs = __instance.KingdomItemVMs;
                if (kingdomItemVMs == null)
                {
                    SubModule.Logger.Warn("KingdomManagementVMPatch: KingdomItemVMs is null.");
                    return;
                }

                foreach (var item in kingdomItemVMs)
                {
                    if (item.Identifier == "WarCouncil")
                    {
                        return;
                    }
                }

                var warCouncilVm = new KingdomWarCouncilVM();
                var warCouncilItem = new KingdomItemVM(
                    "WarCouncil",
                    new TextObject("War Council"),
                    warCouncilVm,
                    ____onItemSelection,
                    ____onClose
                );

                kingdomItemVMs.Add(warCouncilItem);
                SubModule.Logger.Info("Successfully patched KingdomManagementVM to add War Council item.");
            }
            catch (Exception ex)
            {
                SubModule.Logger.Error("KingdomManagementVMPatch: An exception occurred.", ex);
            }
        }
    }
}
