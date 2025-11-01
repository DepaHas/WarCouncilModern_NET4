using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI.Data;
using WarCouncilModern.Initialization;
using WarCouncilModern.UI.States;
using WarCouncilModern.UI.ViewModels;

namespace WarCouncilModern.UI.Screens
{
    [GameStateScreen(typeof(WarCouncilState))]
    public class WarCouncilScreen : ScreenBase
    {
        private GauntletLayer _gauntletLayer;
        private CouncilOverviewViewModel _dataSource;
        private IGauntletMovie _movie;

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

        protected override void OnFinalize()
        {
            base.OnFinalize();
            if (_gauntletLayer != null)
            {
                RemoveLayer(_gauntletLayer);
                _gauntletLayer.ReleaseMovie(_movie);
                _gauntletLayer = null;
            }
            _dataSource = null;
            _movie = null;
        }
    }
}
