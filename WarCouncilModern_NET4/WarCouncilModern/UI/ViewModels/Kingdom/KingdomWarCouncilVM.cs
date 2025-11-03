using System;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace WarCouncilModern.UI.ViewModels.Kingdom
{
    public class KingdomWarCouncilVM : KingdomCategoryVM
    {
        public KingdomWarCouncilVM(Action<KingdomCategoryVM> onSelect, KingdomManagementVM kingdomManagement)
            : base(new TextObject("War Council"), kingdomManagement, onSelect, false)
        {
            // The base constructor handles most of the work.
            // We can add specific logic here if needed.
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            // Additional refresh logic here.
        }
    }
}
