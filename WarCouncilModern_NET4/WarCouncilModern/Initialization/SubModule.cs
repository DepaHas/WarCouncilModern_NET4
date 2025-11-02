using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using WarCouncilModern.Core.Council;
using WarCouncilModern.Core.Decisions;
using WarCouncilModern.Core.Init;
using WarCouncilModern.Core.Manager;
using WarCouncilModern.Core.Services;
using WarCouncilModern.Core.Settings;
using WarCouncilModern.Core.State;
using WarCouncilModern.CouncilSystem.Behaviors;
using WarCouncilModern.DevTools;
using WarCouncilModern.Save;
using WarCouncilModern.UI;
using WarCouncilModern.UI.Platform;
using WarCouncilModern.UI.Providers;
using WarCouncilModern.UI.Services;
using WarCouncilModern.UI.States;
using WarCouncilModern.UI.ViewModels;
using WarCouncilModern.Utilities;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.Initialization
{
    public class SubModule : MBSubModuleBase
    {
        internal static IModLogger Logger { get; private set; } = GlobalLog.Instance;
        internal static IWarCouncilManager? WarCouncilManager { get; private set; }
        internal static ICouncilService? CouncilService { get; private set; }
        internal static IWarDecisionService? WarDecisionService { get; private set; }
        internal static IGameApi? GameApi { get; private set; }
        internal static DevCouncilPanel? DevPanel { get; private set; }
        internal static IUiInvoker? UiInvoker { get; private set; }
        internal static ICouncilProvider? CouncilProvider { get; private set; }
        internal static ICouncilUiService? CouncilUiService { get; private set; }
        internal static CouncilOverviewViewModel? CouncilOverviewViewModel { get; private set; }
        internal static IModSettings? _settings;
        private Harmony? _harmony;
        private WarCouncilCampaignBehavior? _warCouncilCampaignBehavior;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            try
            {
                Logger.Info("SubModule loading - WarCouncilModern initializing.");

                _harmony = new Harmony("com.warcouncilmodern.kingdomtab");
                _harmony.PatchAll();
                Logger.Info("Harmony patches applied (WarCouncilModern).");

                var asm = AppDomain.CurrentDomain.GetAssemblies()
                           .FirstOrDefault(a => a.GetName().Name?.IndexOf("Harmony", StringComparison.OrdinalIgnoreCase) >= 0);
                Logger.Info(asm != null ? $"Harmony loaded from: {asm.Location}" : "Harmony not found among loaded assemblies.");

                RegisterSaveDefinerSafely();

                // Note: UI resources are loaded directly inside WarCouncilState (Gauntlet), do not Register here.
                Logger.Info("SubModule loaded successfully.");
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to apply Harmony patches.", ex);
            }
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            if (game.GameType is not Campaign)
                return;

            try
            {
                Logger.Info("Initializing WarCouncilModern SubModule...");

                _settings ??= new ModSettings();
                var featureRegistry = new FeatureRegistry(_settings);
                var initializer = new ModuleInitializer();
                initializer.Initialize(
                    _warCouncilCampaignBehavior,
                    featureRegistry,
                    Logger,
                    new ModStateTracker(Logger),
                    new CouncilMeetingService(Logger),
                    new DecisionProcessingService(Logger),
                    new AdvisorService(Logger),
                    _settings
                );
                WarCouncilManager = initializer.Manager ?? throw new InvalidOperationException("ModuleInitializer failed to initialize WarCouncilManager.");

                GameApi = new GameApi();
                var memberSelector = new DefaultCouncilMemberSelector(GameApi, Logger);
                var executionHandler = new ChangeFactionRelationHandler(GameApi, Logger);

                CouncilService = new CouncilService(WarCouncilManager, memberSelector, Logger);
                WarDecisionService = new WarDecisionService(WarCouncilManager, featureRegistry, executionHandler, Logger);

#if DEBUG
                CouncilProvider = new MockCouncilProvider();
                Logger.Info("Using MockCouncilProvider for UI development.");
#else
                CouncilProvider = new LiveCouncilProvider(WarCouncilManager);
#endif

                var uiScheduler = SynchronizationContext.Current != null ? TaskScheduler.FromCurrentSynchronizationContext() : TaskScheduler.Default;
                UiInvoker = new UiInvoker(uiScheduler);
                CouncilUiService = new CouncilUiService(CouncilProvider, WarCouncilManager, WarDecisionService, UiInvoker, Logger);

                CouncilOverviewViewModel = new CouncilOverviewViewModel(CouncilUiService);

                DevPanel = new DevCouncilPanel(CouncilService, CouncilUiService, Logger);

                CampaignEvents.OnSessionLaunchedEvent.AddListener(OnSessionLaunched);
            }
            catch (Exception ex)
            {
                Logger.Error("Failed during SubModule initialization", ex);
            }
        }

        private async void OnSessionLaunched(CampaignGameStarter starter)
        {
            try
            {
                if (_warCouncilCampaignBehavior == null)
                {
                    _warCouncilCampaignBehavior = new WarCouncilCampaignBehavior();
                    starter.AddBehavior(_warCouncilCampaignBehavior);
                    Logger.Info("WarCouncilCampaignBehavior added on session launch.");
                }

                if (_settings == null)
                {
                    Logger.Info("Settings not initialized; skipping UI initialization.");
                    return;
                }

                if (!_settings.EnableCouncilUI && !_settings.EnableCouncilDevTools)
                    return;

                if (CouncilUiService == null)
                {
                    Logger.Warn("CouncilUiService is null; aborting UI initialization.");
                    return;
                }

                Logger.Info("OnSessionLaunched: Initializing Council UI Service...");
                await CouncilUiService.InitializeAsync();

                int attempts = 0;
                while ((Game.Current?.GameStateManager == null) && attempts++ < 10)
                    await Task.Delay(200);

                if (Game.Current?.GameStateManager == null)
                {
                    Logger.Warn("GameStateManager not ready; skipping WarCouncilState push.");
                    return;
                }

                if (CouncilOverviewViewModel == null)
                {
                    Logger.Warn("CouncilOverviewViewModel is null; cannot push WarCouncilState.");
                    return;
                }

                try
                {
                    Game.Current.GameStateManager.PushState(new WarCouncilModern.UI.States.WarCouncilState(CouncilOverviewViewModel));
                    Logger.Info("WarCouncilState pushed successfully.");
                }
                catch (Exception ex)
                {
                    Logger.Error("Failed to push WarCouncilState", ex);
                }

                CampaignEvents.OnSessionLaunchedEvent.RemoveListener(OnSessionLaunched);
            }
            catch (Exception ex)
            {
                Logger.Error("Error during OnSessionLaunched execution", ex);
            }
        }

        protected override void OnSubModuleUnloaded()
        {
            try
            {
                Logger.Info("SubModule unloading — cleaning up WarCouncilModern...");

                if (_harmony != null)
                {
                    _harmony.UnpatchAll("com.warcouncilmodern.kingdomtab");
                    Logger.Info("Harmony patches removed (WarCouncilModern).");
                    _harmony = null;
                }

                CampaignEvents.OnSessionLaunchedEvent.RemoveListener(OnSessionLaunched);
                CouncilUiService?.Dispose();
                CouncilUiService = null;

                CouncilOverviewViewModel = null;
                DevPanel = null;
                WarCouncilManager = null;
                CouncilService = null;
                WarDecisionService = null;
                GameApi = null;
                UiInvoker = null;
                CouncilProvider = null;
                _settings = null;
            }
            catch (Exception ex)
            {
                Logger.Error("Error while removing Harmony patches", ex);
            }
            finally
            {
                base.OnSubModuleUnloaded();
            }
        }

        private void RegisterSaveDefinerSafely()
        {
            try
            {
                var definer = new WarCouncilSaveDefiner();
                var registryType = Type.GetType("TaleWorlds.SaveSystem.SaveTypeRegistry, TaleWorlds.SaveSystem");
                if (registryType != null)
                {
                    var instanceProp = registryType.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public);
                    var instance = instanceProp?.GetValue(null);
                    if (instance != null)
                    {
                        var addMethod = registryType.GetMethod("AddTypeDefiner", new Type[] { definer.GetType() });
                        if (addMethod != null)
                        {
                            addMethod.Invoke(instance, new object[] { definer });
                            Logger.Info("WarCouncilSaveDefiner registered via SaveTypeRegistry.Instance.AddTypeDefiner.");
                            return;
                        }
                    }
                }
                var definerType = Type.GetType("TaleWorlds.SaveSystem.SaveTypeDefiner, TaleWorlds.SaveSystem");
                var addStatic = definerType?.GetMethod("AddTypeDefiner", BindingFlags.Static | BindingFlags.Public);
                if (addStatic != null)
                {
                    addStatic.Invoke(null, new object[] { definer });
                    Logger.Info("WarCouncilSaveDefiner registered via SaveTypeDefiner.AddTypeDefiner fallback.");
                    return;
                }
                Logger.Warn("Could not find a supported API to register WarCouncilSaveDefiner.");
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to construct/register WarCouncilSaveDefiner", ex);
            }
        }
    }
}