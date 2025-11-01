using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.ScreenSystem;
using WarCouncilModern.Initialization;
using WarCouncilModern.UI.ViewModels;

namespace WarCouncilModern.UI.Screens
{
    public class CouncilOverviewScreen : ScreenBase
    {
        private GauntletLayer _gauntletLayer;
        private CouncilOverviewViewModel _dataSource;
        private GauntletMovieIdentifier _movie;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _dataSource = SubModule.CouncilOverviewViewModel;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            _gauntletLayer = new GauntletLayer(100);
            _movie = _gauntletLayer.LoadMovie("CouncilOverviewView", _dataSource);
            AddLayer(_gauntletLayer);
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
            if (_gauntletLayer != null)
            {
                RemoveLayer(_gauntletLayer);
                _gauntletLayer.ReleaseMovie(_movie);
                _gauntletLayer = null;
            }
        }

        protected override void OnFinalize()
        {
            base.OnFinalize();
            _dataSource = null;
        }
    }
}
