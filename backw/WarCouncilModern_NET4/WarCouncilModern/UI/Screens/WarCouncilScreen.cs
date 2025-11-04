using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.ScreenSystem;
using WarCouncilModern.Initialization;
using WarCouncilModern.UI.States;
using WarCouncilModern.UI.ViewModels;
//using WarCouncilModern.UI.Views;

namespace WarCouncilModern.UI.Screens
{
    public class WarCouncilScreen : ScreenBase
    {
        private GauntletLayer? _gauntletLayer;
        private CouncilOverviewViewModel _dataSource = null!;
        private GauntletMovieIdentifier _movie = null!;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _dataSource = SubModule.CouncilOverviewViewModel;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            _gauntletLayer = new GauntletLayer(100);
            _movie = _gauntletLayer.LoadMovie("CouncilOverviewScreen", _dataSource);
            AddLayer(_gauntletLayer);
            ScreenManager.TrySetFocus(_gauntletLayer);
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
            if (_gauntletLayer != null)
            {
                RemoveLayer(_gauntletLayer);
                if (_movie != null)
                {
                    _gauntletLayer.ReleaseMovie(_movie);
                }
                _gauntletLayer = null;
            }
        }

        protected override void OnFinalize()
        {
            base.OnFinalize();
        }
    }
}