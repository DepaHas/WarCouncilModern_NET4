using System;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.MountAndBlade;
using WarCouncilModern.UI.ViewModels;

namespace WarCouncilModern.UI.States
{
    public class WarCouncilState : TaleWorlds.Core.GameState
    {
        private GauntletLayer? _gauntletLayer;
        private IGauntletMovie? _movie;
        private CouncilOverviewViewModel _viewModel;

        public WarCouncilState(CouncilOverviewViewModel viewModel)
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }

        protected override void OnInitialize()

        {
            try
            {
                _gauntletLayer = new GauntletLayer(0);
                // Load movie directly from XML prefab; returns IGauntletMovie in newer APIs
                _movie = _gauntletLayer.LoadMovie("WarCouncilModern/GUI/Prefabs/Council/WarCouncil.CouncilOverview.xml", _viewModel)?.Movie;
                if (_movie != null)
                {
                    _gauntletLayer.AddMovie(_movie);
                }

                // Push layer via ScreenManager or ScreenBase API
                var screenManagerType = Type.GetType("TaleWorlds.Engine.ScreenManager, TaleWorlds.Engine")
                                     ?? Type.GetType("TaleWorlds.MountAndBlade.ScreenManager, TaleWorlds.MountAndBlade");
                if (screenManagerType != null)
                {
                    // If Game has ScreenManager property accessible via Game.Current, try to add layer
                    var gm = Game.Current;
                    var prop = gm?.GetType().GetProperty("ScreenManager");
                    var screenManager = prop?.GetValue(gm);
                    if (screenManager != null)
                    {
                        var addLayer = screenManager.GetType().GetMethod("AddLayer");
                        addLayer?.Invoke(screenManager, new object[] { _gauntletLayer });
                    }
                }

                 base.OnInitialize();
            }
            catch (Exception ex)
            {
                WarCouncilModern.Initialization.SubModule.Logger?.Error("WarCouncilState initialization failed", ex);
                
            }
        }

        protected override void OnFinalize()

        {
            try
            {
                if (_movie != null)
                {
                    _gauntletLayer?.RemoveMovie(_movie);
                    _movie = null;
                }
                base.OnFinalize();
            }
            catch (Exception ex)
            {
                WarCouncilModern.Initialization.SubModule.Logger?.Error("WarCouncilState finalize failed", ex);
            }
        }
    }
}