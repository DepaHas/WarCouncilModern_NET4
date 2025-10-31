using System;
using System.Reflection;
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
using WarCouncilModern.Models.Persistence;
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

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            try
            {
                Logger.Info("SubModule loading - WarCouncilModern initializing.");
                RegisterSaveDefinerSafely();
                Logger.Info("SubModule loaded successfully.");
            }
            catch (Exception ex)
            {
                try { Logger.Error("SubModule.OnSubModuleLoad exception", ex); } catch { }
            }
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            if (game.GameType is Campaign)
            {
                var gameStarter = (CampaignGameStarter)gameStarterObject;
                var behavior = new WarCouncilCampaignBehavior();
                gameStarter.AddBehavior(behavior);

                var settings = new StubModSettings();
                var featureRegistry = new FeatureRegistry(settings);
                var initializer = new ModuleInitializer();
                initializer.Initialize(
                    behavior,
                    featureRegistry,
                    Logger,
                    new ModStateTracker(Logger),
                    new CouncilMeetingService(Logger),
                    new DecisionProcessingService(Logger),
                    new AdvisorService(Logger),
                    new StubModSettings()
                );
                WarCouncilManager = initializer.Manager;

                GameApi = new GameApi();
                var memberSelector = new DefaultCouncilMemberSelector(GameApi, Logger);
                var executionHandler = new LogExecutionHandler(Logger);

                CouncilService = new CouncilService(WarCouncilManager, memberSelector, Logger);
                WarDecisionService = new WarDecisionService(WarCouncilManager, featureRegistry, executionHandler, Logger);
            }
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
            // Hook for UI initialization or late game setup if needed
        }

        protected override void OnSubModuleUnloaded()
        {
            try
            {
                Logger.Info("SubModule unloading - WarCouncilModern cleanup started.");
                // perform cleanup if necessary
            }
            catch (Exception ex)
            {
                try { Logger.Error("SubModule.OnSubModuleUnloaded exception", ex); } catch { }
            }
            finally
            {
                base.OnSubModuleUnloaded();
            }
        }

        // Registers WarCouncilSaveDefiner using reflection-safe approach to support multiple TaleWorlds.SaveSystem versions.
        private void RegisterSaveDefinerSafely()
        {
            try
            {
                var definer = new WarCouncilSaveDefiner();

                // Try SaveTypeRegistry.Instance.AddTypeDefiner(definer)
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

                // Fallback to static SaveTypeDefiner.AddTypeDefiner(definer)
                var definerType = Type.GetType("TaleWorlds.SaveSystem.SaveTypeDefiner, TaleWorlds.SaveSystem");
                var addStatic = definerType?.GetMethod("AddTypeDefiner", BindingFlags.Static | BindingFlags.Public);
                if (addStatic != null)
                {
                    addStatic.Invoke(null, new object[] { definer });
                    Logger.Info("WarCouncilSaveDefiner registered via SaveTypeDefiner.AddTypeDefiner fallback.");
                    return;
                }

                // If neither approach available, log a warning
                Logger.Warn("Could not find a supported API to register WarCouncilSaveDefiner for this TaleWorlds.SaveSystem version.");
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to construct/register WarCouncilSaveDefiner", ex);
            }
        }
    }
}