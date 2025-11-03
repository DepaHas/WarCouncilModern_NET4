using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement;
using TaleWorlds.Localization;

namespace WarCouncilModern.UI.ViewModels.Kingdom
{
    public class WarCouncilCategoryVM : KingdomCategoryVM
    {
        public WarCouncilCategoryVM(TextObject title) : base(title, null, null, false)
        {
        }

        protected override void OnSelect()
        {
            // TODO: Open the War Council screen.
        }
    }
}
