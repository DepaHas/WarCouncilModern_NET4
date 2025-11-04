using TaleWorlds.Library;
using WarCouncilModern.UI.Services;

namespace WarCouncilModern.UI.ViewModels
{
    public class CouncilOverviewViewModel : ViewModel
    {
        public MBBindingList<CouncilItemViewModel> Decisions { get; }
            = new MBBindingList<CouncilItemViewModel>();

        public CouncilOverviewViewModel()
        {
            Decisions.Add(new CouncilItemViewModel("Test Decision"));
        }

        public CouncilOverviewViewModel(ICouncilUiService uiService)
        {
            Decisions.Add(new CouncilItemViewModel("Decision via Service"));
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
        }
        public void StartMeeting()
        {
            InformationManager.DisplayMessage(new InformationMessage("StartMeeting!"));
        }
    }

}