using HarmonyLib;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using WarCouncilModern.Initialization;
using WarCouncilModern.UI.ViewModels.Kingdom;

namespace WarCouncilModern.Patches
{
    [HarmonyPatch(typeof(KingdomManagementVM), MethodType.Constructor)]
    public static class KingdomManagementVMPatch
    {
        public static void Postfix(KingdomManagementVM __instance)
        {
            // TODO: [WarCouncil] This patch is now active. Verify functionality in-game.
            var categoriesProp = AccessTools.Property(typeof(KingdomManagementVM), "Categories");
            if (categoriesProp == null)
            {
                SubModule.Logger.Error("[WarCouncil] Could not find 'Categories' property on KingdomManagementVM via AccessTools.");
                return;
            }

            var categories = categoriesProp.GetValue(__instance) as MBBindingList<KingdomCategoryVM>;
            if (categories == null)
            {
                SubModule.Logger.Error("[WarCouncil] 'Categories' property is null or not of type MBBindingList<KingdomCategoryVM>.");
                return;
            }

            categories.Add(new WarCouncilCategoryVM(new TextObject("{=WC_TabName}War Council")));
            SubModule.Logger.Info("[WarCouncil] Successfully added War Council tab to Kingdom screen.");
        }
    }
}
