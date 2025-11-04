using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement;
using TaleWorlds.Localization;

namespace WarCouncilModern.UI.ViewModels.Kingdom
{
    public class WarCouncilCategoryVM : KingdomCategoryVM
    {
        // هذه الخاصية هي اللي هتُستخدم في الـ UI لعرض اسم التبويب
        public TextObject Title { get; }

        public WarCouncilCategoryVM() : base()
        {
            Title = new TextObject("{=WC_TabName}War Council");
        }
    }
}