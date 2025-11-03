using System;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;
using WarCouncilModern.Initialization;
using WarCouncilModern.UI.ViewModels;

namespace WarCouncilModern.UI.States
{
    public class WarCouncilState : GameState
    {
        private GauntletLayer? _gauntletLayer;
        private GauntletMovieIdentifier? _movie;
        private readonly CouncilOverviewViewModel _viewModel;

        public override bool IsMenuState => true;

        public WarCouncilState(CouncilOverviewViewModel viewModel)
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _gauntletLayer = new GauntletLayer(100, "GauntletLayer");
            _gauntletLayer.IsFocusLayer = true;

            if (Game.Current?.ScreenManager != null)
            {
                Game.Current.ScreenManager.AddLayer(_gauntletLayer);
                ScreenManager.TrySetFocus(_gauntletLayer);
            }

            try
            {
                _movie = _gauntletLayer.LoadMovie("WarCouncil.CouncilOverview", _viewModel);
                SubModule.Logger.Info("WarCouncilState: Movie loaded successfully.");
            }
            catch (Exception ex)
            {
                SubModule.Logger.Error("WarCouncilState: Failed to load movie", ex);
            }
        }

        protected override void OnFinalize()
        {
            base.OnFinalize();
            if (_gauntletLayer != null)
            {
                if (_movie != null)
                {
                    _gauntletLayer.UnloadMovie(_movie);
                    _movie = null;
                }

                if (Game.Current?.ScreenManager != null)
                {
                    Game.Current.ScreenManager.RemoveLayer(_gauntletLayer);
                }
                _gauntletLayer = null;
            }
        }
    }
}
