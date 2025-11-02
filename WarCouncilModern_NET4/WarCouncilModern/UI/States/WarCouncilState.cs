using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;
using WarCouncilModern.UI.ViewModels;

namespace WarCouncilModern.UI.States
{
    public class WarCouncilState : GameState
    {
        private GauntletLayer? _gauntletLayer;
        private IGauntletMovie? _movie;
        private readonly ViewModel _viewModel;

        public override bool IsMenuState => true;

        public WarCouncilState(ViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _gauntletLayer = new GauntletLayer(100);
            Game.Current.ScreenManager.AddLayer(_gauntletLayer);
            _gauntletLayer.IsFocusLayer = true;
            ScreenManager.TrySetFocus(_gauntletLayer);

            _movie = _gauntletLayer.LoadMovie("WarCouncil.CouncilOverview", _viewModel);
        }

        protected override void OnFinalize()
        {
            base.OnFinalize();
            if (_gauntletLayer != null)
            {
                if (_movie != null)
                {
                    _gauntletLayer.ReleaseMovie(_movie);
                    _movie = null;
                }
                Game.Current.ScreenManager.RemoveLayer(_gauntletLayer);
                _gauntletLayer = null;
            }
        }
    }
}
