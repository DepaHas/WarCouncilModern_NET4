using System;
using System.Reflection;
using TaleWorlds.MountAndBlade;
using WarCouncilModern.Utilities;
using WarCouncilModern.Utilities.Interfaces;
using WarCouncilModern.Save;

namespace WarCouncilModern.Initialization
{
    public class SubModule : MBSubModuleBase
    {
        internal static IModLogger Logger { get; private set; } = GlobalLog.Instance;

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