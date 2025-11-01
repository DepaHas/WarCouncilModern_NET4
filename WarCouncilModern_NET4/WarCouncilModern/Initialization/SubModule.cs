using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using WarCouncilModern.Core.Init;
using WarCouncilModern.Core.Council;
using WarCouncilModern.Core.Decisions;
using WarCouncilModern.Core.Manager;
using WarCouncilModern.Core.Services;
using WarCouncilModern.Core.Settings;
using WarCouncilModern.Core.State;
using WarCouncilModern.DevTools;
using WarCouncilModern.Models.Persistence;
using WarCouncilModern.UI;
using WarCouncilModern.UI.Interfaces;
using WarCouncilModern.UI.Providers;
using WarCouncilModern.UI.Services;
using WarCouncilModern.UI.ViewModels;
using WarCouncilModern.UI.ViewModels;
using WarCouncilModern.Utilities;
using WarCouncilModern.Utilities.Interfaces;
using WarCouncilModern.Save;
using WarCouncilModern.CouncilSystem.Behaviors;

namespace WarCouncilModern.Initialization
{
    public class SubModule : MBSubModuleBase
    {
        internal static IModLogger Logger { get; private set; } = GlobalLog.Instance;
        internal static IWarCouncilManager WarCouncilManager { get; private set; }
        internal static ICouncilService CouncilService { get; private set; }
        internal static IWarDecisionService WarDecisionService { get; private set; }
        internal static IGameApi GameApi { get; private set; }
        internal static DevCouncilPanel DevPanel { get; private set; }
        internal static IUiInvoker UiInvoker { get; private set; }
        internal static ICouncilProvider CouncilProvider { get; private set; }
        internal static ICouncilUiService CouncilUiService { get; private set; }
        internal static CouncilOverviewViewModel CouncilOverviewViewModel { get; private set; }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            // ... (rest of the method is unchanged)
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            if (game.GameType is Campaign)
            {
                // ... (rest of the method is unchanged until the end)

                var uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
                UiInvoker = new UiInvoker(uiScheduler);
                CouncilUiService = new CouncilUiService(CouncilProvider, WarCouncilManager, WarDecisionService, UiInvoker, Logger);

                // Initialize the main ViewModel
                CouncilOverviewViewModel = new CouncilOverviewViewModel(CouncilUiService);

                // DevPanel now can be simplified or take the ViewModel if needed for debugging
                DevPanel = new DevCouncilPanel(CouncilService, CouncilUiService, Logger);
            }
        }

        // ... (rest of the file is unchanged)
    }
}
