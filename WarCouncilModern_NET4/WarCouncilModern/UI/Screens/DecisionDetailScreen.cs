using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.ScreenSystem;
using WarCouncilModern.UI.ViewModels;

namespace WarCouncilModern.UI.Screens
{
    public class DecisionDetailScreen : ScreenBase
    {
        private GauntletLayer _gauntletLayer;
        private DecisionDetailViewModel _dataSource;
        private IGauntletMovie _movie;

        // Note: _dataSource would be initialized with a specific decision context
        public DecisionDetailScreen(DecisionDetailViewModel dataSource)
        {
            _dataSource = dataSource;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            _gauntletLayer = new GauntletLayer(100);
            _movie = _gauntletLayer.LoadMovie("DecisionDetailView", _dataSource);
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
                _movie = null;
            }
        }

        protected override void OnFinalize()
        {
            base.OnFinalize();
            _dataSource = null;
        }
    }
}
