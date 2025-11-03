using System;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using WarCouncilModern.Initialization;
using WarCouncilModern.UI.Commands;
using WarCouncilModern.UI.States;

namespace WarCouncilModern.UI.ViewModels.Kingdom
{
    public class KingdomWarCouncilVM : KingdomCategoryVM
    {
        private readonly KingdomManagementVM _kingdomManagement;

        public KingdomWarCouncilVM(Action<KingdomCategoryVM> onSelect, KingdomManagementVM kingdomManagement)
            : base(new TextObject("War Council"), kingdomManagement, onSelect, false)
        {
            _kingdomManagement = kingdomManagement;
            // The command to open the War Council screen.
            // We need to figure out how to push the state from here.
            // For now, let's log that the button was clicked.
            OnSelect = new DelegateCommand(ExecuteSelect);
        }

        private void ExecuteSelect(object? obj)
        {
            SubModule.Logger.Info("[WarCouncil] War Council tab selected. Opening screen...");
            // This is where we would push the game state.
            var state = new WarCouncilState(SubModule.CouncilOverviewViewModel);
            Game.Current.GameStateManager.PushState(state);
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            // Additional refresh logic here.
        }
    }
}
