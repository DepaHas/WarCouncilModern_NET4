using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace WarCouncilModern.UI.States
{
    public class WarCouncilState : GameState
    {
        private GauntletLayer? _layer;
        private GauntletMovieIdentifier? _movie;
        private ViewModel? _viewModel;

        public WarCouncilState(ViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _layer = new GauntletLayer(100);
            ScreenManager.TopScreen.AddLayer(_layer);

            if (_viewModel != null)
                _movie = _layer.LoadMovie("WarCouncil.CouncilOverview", _viewModel);
        }

        protected override void OnFinalize()
        {
            if (_layer != null)
            {
                if (_movie != null)
                {
                    _layer.ReleaseMovie(_movie);
                    _movie = null;
                }
                ScreenManager.TopScreen.RemoveLayer(_layer);
                _layer = null;
            }
            _viewModel = null;

            base.OnFinalize();
        }
    }
}