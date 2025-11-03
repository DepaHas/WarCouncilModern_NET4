using System;
using System.Reflection;
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
        private object? _movie; // Use object? for API compatibility
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
            ScreenManager.Instance.AddLayer(_gauntletLayer);
            ScreenManager.TrySetFocus(_gauntletLayer);

            try
            {
                var loadMovieMethod = typeof(GauntletLayer).GetMethod("LoadMovie", new[] { typeof(string), typeof(ViewModel) });
                if (loadMovieMethod != null)
                {
                    _movie = loadMovieMethod.Invoke(_gauntletLayer, new object[] { "WarCouncil.CouncilOverview", _viewModel });
                    SubModule.Logger.Info("WarCouncilState: Movie loaded successfully via reflection.");
                }
                else
                {
                    SubModule.Logger.Error("WarCouncilState: LoadMovie method not found via reflection.");
                }
            }
            catch (Exception ex)
            {
                SubModule.Logger.Error("WarCouncilState: Failed to load movie via reflection", ex);
            }
        }

        protected override void OnFinalize()
        {
            base.OnFinalize();
            if (_gauntletLayer != null)
            {
                if (_movie != null)
                {
                    try
                    {
                        var releaseMovieMethod = typeof(GauntletLayer).GetMethod("ReleaseMovie", new[] { _movie.GetType() });
                        if (releaseMovieMethod != null)
                        {
                            releaseMovieMethod.Invoke(_gauntletLayer, new[] { _movie });
                            SubModule.Logger.Info("WarCouncilState: Movie released successfully via reflection.");
                        }
                        else
                        {
                            SubModule.Logger.Error("WarCouncilState: ReleaseMovie method not found via reflection.");
                        }
                    }
                    catch (Exception ex)
                    {
                        SubModule.Logger.Error("WarCouncilState: Failed to release movie via reflection", ex);
                    }
                    _movie = null;
                }
                ScreenManager.Instance.RemoveLayer(_gauntletLayer);
                _gauntletLayer = null;
            }
        }
    }
}
