using HarmonyLib;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement;

namespace WarCouncilModern.Patches
{
    [HarmonyPatch(typeof(KingdomManagementVM), "OnInitialize")]
    public class KingdomManagementVMPatch
    {
        public static void Postfix(KingdomManagementVM __instance)
        {
            // var list = __instance.KingdomItemVMs;
            // new KingdomItemVM(...)
        }
    }
}
