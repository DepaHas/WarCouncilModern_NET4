using HarmonyLib;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement;
using TaleWorlds.Library;
using WarCouncilModern.UI.ViewModels.Kingdom; // ✅ هذا هو الـ namespace الصحيح
using System;

namespace WarCouncilModern.Patches
{
    [HarmonyPatch(typeof(KingdomManagementVM))]
    [HarmonyPatch(MethodType.Constructor)]
    public static class KingdomManagementVMPatch
    {
        public static void Postfix(KingdomManagementVM __instance)
        {
            try
            {
                var categoriesProp = AccessTools.Property(typeof(KingdomManagementVM), "Categories");
                var categories = categoriesProp.GetValue(__instance) as MBBindingList<KingdomCategoryVM>;
                if (categories != null)
                {
                    categories.Add(new WarCouncilCategoryVM());
                }
            }
            catch (Exception ex)
            {
                WarCouncilModern.Initialization.SubModule.Logger.Error("Failed to add WarCouncilCategoryVM tab", ex);
            }
        }
    }
}