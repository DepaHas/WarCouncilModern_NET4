using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HarmonyLib;
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
using WarCouncilModern.UI;
using WarCouncilModern.UI.Platform;
using WarCouncilModern.UI.Providers;
using WarCouncilModern.UI.Services;
using WarCouncilModern.UI.ViewModels;
using WarCouncilModern.UI.States;
using WarCouncilModern.Utilities;
using WarCouncilModern.Utilities.Interfaces;
using WarCouncilModern.Save;
using WarCouncilModern.CouncilSystem.Behaviors;

namespace WarCouncilModern.Initialization
{
    public class SubModule : MBSubModuleBase
    {
        internal static IModLogger Logger { get; private set; } = GlobalLog.Instance;
        internal static IWarCouncilManager WarCouncilManager { get; private set; } = null!;
        internal static ICouncilService CouncilService { get; private set; } = null!;
        internal static IWarDecisionService WarDecisionService { get; private set; } = null!;
        internal static IGameApi GameApi { get; private set; } = null!;
        internal static DevCouncilPanel DevPanel { get; private set; } = null!;
        internal static IUiInvoker UiInvoker { get; private set; } = null!;
        internal static ICouncilProvider CouncilProvider { get; private set; } = null!;
        internal static ICouncilUiService CouncilUiService { get; private set; } = null!;
        internal static CouncilOverviewViewModel CouncilOverviewViewModel { get; private set; } = null!;
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

                UIResourceManager.Register("WarCouncil.CouncilOverview", "WarCouncilModern/GUI/Prefabs/Council/WarCouncil.CouncilOverview.xml");
                UIResourceManager.Register("WarCouncil.CouncilDetail", "WarCouncilModern/GUI/Prefabs/Council/WarCouncil.CouncilDetail.xml");
                UIResourceManager.Register("WarCouncil.DecisionModal", "WarCouncilModern/GUI/Prefabs/Council/WarCouncil.DecisionModal.xml");
                UIResourceManager.Register("KingdomWarCouncil", "WarCouncilModern/GUI/Prefabs/Council/KingdomWarCouncil.xml");
                Logger.Info("Registered custom UI resources.");

                Logger.Info("SubModule loaded successfully.");
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to apply Harmony patches.", ex);
            }
        }

        protected internal override void InitializeGameStarter(Game game, IGameStarter gameStarterObject)
        {
            base.InitializeGameStarter(game, gameStarterObject);
            if (game.GameType is Campaign)
            {
                var gameStarter = (CampaignGameStarter)gameStarterObject;
                _warCouncilCampaignBehavior = new WarCouncilCampaignBehavior();
                gameStarter.AddBehavior(_warCouncilCampaignBehavior);
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

                _settings = new ModSettings(); // Use ModSettings, not Stub
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

                var uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
                UiInvoker = new UiInvoker(uiScheduler);
                CouncilUiService = new CouncilUiService(CouncilProvider, WarCouncilManager, WarDecisionService, UiInvoker, Logger);

                CouncilOverviewViewModel = new CouncilOverviewViewModel(CouncilUiService);

                DevPanel = new DevCouncilPanel(CouncilService, CouncilUiService, Logger);

                CampaignEvents.OnGameLoadedSignal.AddNonSerializedListener(this, OnGameLoaded);
            }
            catch (Exception ex)
            {
                Logger.Error("Failed during SubModule initialization", ex);
            }
        }

        private async void OnGameLoaded(CampaignGameStarter starter)
        {
            if (_settings == null) return;
            try
            {
                if (!_settings.EnableCouncilUI && !_settings.EnableCouncilDevTools)
                    return;

                Logger.Info("OnSessionLaunched: Initializing Council UI Service...");
                await CouncilUiService.InitializeAsync();
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

                CampaignEvents.OnGameLoadedSignal.RemoveNonSerializedListener(this, OnGameLoaded);
                CouncilUiService?.Dispose();
                CouncilUiService = null!;
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
