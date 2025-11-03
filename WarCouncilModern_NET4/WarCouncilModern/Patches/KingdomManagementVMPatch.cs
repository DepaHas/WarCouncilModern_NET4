using System;
using System.Linq;
using HarmonyLib;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement;
using TaleWorlds.Library;
using WarCouncilModern.Initialization;
using WarCouncilModern.UI.ViewModels.Kingdom;
using WarCouncilModern.Utilities.Reflection;

namespace WarCouncilModern.Patches
{
    [HarmonyPatch(typeof(KingdomManagementVM), "OnInitialize")]
    public static class KingdomManagementVMPatch
    {
        public static void Postfix(KingdomManagementVM __instance)
        {
            try
            {
                var categories = ReflectionHelpers.GetPropertyValue<MBBindingList<KingdomCategoryVM>>(__instance, "Categories");
                if (categories == null)
                {
                    SubModule.Logger.Warn("[WarCouncil] Could not find 'Categories' property on KingdomManagementVM.");
                    return;
                }

                if (categories.Any(c => c is KingdomWarCouncilVM))
                {
                    return; // Already added.
                }

                var onCategorySelect = ReflectionHelpers.GetFieldValue<Action<KingdomCategoryVM>>(__instance, "_onCategorySelect");
                if (onCategorySelect == null)
                {
                     SubModule.Logger.Warn("[WarCouncil] Could not find '_onCategorySelect' action on KingdomManagementVM.");
                     return;
                }

                var warCouncilVm = new KingdomWarCouncilVM(onCategorySelect, __instance);
                categories.Add(warCouncilVm);
                SubModule.Logger.Info("[WarCouncil] Successfully added War Council tab to Kingdom screen.");
            }
            catch (Exception ex)
            {
                SubModule.Logger.Error("[WarCouncil] Failed to patch KingdomManagementVM.", ex);
            }
        }
    }
}
