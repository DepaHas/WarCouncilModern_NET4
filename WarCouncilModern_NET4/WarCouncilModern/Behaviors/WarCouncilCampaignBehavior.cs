using System;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.SaveSystem;
using WarCouncilModern.CouncilSystem;
using WarCouncilModern.Save;

namespace WarCouncilModern.Behaviors
{
    public class WarCouncilCampaignBehavior : CampaignBehaviorBase
    {
        [SaveableField(1)]
        private WarCouncilManager _manager;

        public WarCouncilManager Manager => _manager;

        private bool _initialized;

        public WarCouncilCampaignBehavior()
        {
            _manager = new WarCouncilManager();
            WarCouncilManager.Instance = _manager;
        }

        // RegisterEvents is public in the base class; keep the same access modifier
        public override void RegisterEvents()
        {
            try
            {
                // Try the common subscription API first
                CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, OnGameLoaded);
            }
            catch
            {
                try
                {
                    // Reflection fallback: look for an "Add..." overload with two parameters
                    var evt = CampaignEvents.OnGameLoadedEvent;
                    var addMethod = evt.GetType()
                        .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                        .FirstOrDefault(m =>
                        {
                            if (!m.Name.StartsWith("Add", StringComparison.OrdinalIgnoreCase)) return false;
                            var ps = m.GetParameters();
                            return ps.Length == 2;
                        });

                    if (addMethod != null)
                    {
                        // Try to invoke with (object listener, Action<CampaignGameStarter> callback)
                        var callback = (Action<CampaignGameStarter>)OnGameLoaded;
                        addMethod.Invoke(evt, new object[] { this, callback });
                    }
                }
                catch
                {
                    // If subscription fails, continue silently; initialization will attempt to proceed on load if possible
                }
            }
        }

        // SyncData is public in base; keep public override
        public override void SyncData(IDataStore dataStore)
        {
            if (dataStore == null) throw new ArgumentNullException(nameof(dataStore));

            dataStore.SyncData("WarCouncil_Manager_v1", ref _manager);
            WarCouncilManager.Instance = _manager;

            if (!_initialized && _manager != null)
            {
                TryInitializeManagerCaches();
            }
        }

        private void OnGameLoaded(CampaignGameStarter starter)
        {
            InitializeAfterGameLoaded();
        }

        public void InitializeAfterGameLoaded()
        {
            if (_initialized) return;

            try
            {
                _manager?.InitializeDataFromKingdoms();
            }
            catch
            {
                // Ignore API access errors during initialization
            }

            TryInitializeManagerCaches();
            _initialized = true;
        }

        private void TryInitializeManagerCaches()
        {
            try
            {
                // Build caches or repair data after load
                // Example placeholder: ensure singleton instance is set
                if (WarCouncilManager.Instance == null && _manager != null)
                {
                    WarCouncilManager.Instance = _manager;
                }
            }
            catch
            {
                // Swallow exceptions coming from optional cache building
            }
        }

        public void Shutdown()
        {
            try
            {
                var evt = CampaignEvents.OnGameLoadedEvent;
                var evtType = evt.GetType();

                // Common removal method names
                var removeNames = new[] { "RemoveNonSerializedListener", "RemoveListener", "Remove" };

                foreach (var name in removeNames)
                {
                    // Try signature: (object, Action<CampaignGameStarter>)
                    var method = evtType.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                        null, new Type[] { typeof(object), typeof(Action<CampaignGameStarter>) }, null)
                              // Or try (object, Delegate)
                              ?? evtType.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                        null, new Type[] { typeof(object), typeof(Delegate) }, null);

                    if (method != null)
                    {
                        try
                        {
                            method.Invoke(evt, new object[] { this, (Action<CampaignGameStarter>)OnGameLoaded });
                        }
                        catch
                        {
                            // Try next removal method if invocation fails
                        }
                    }
                }
            }
            catch
            {
                // Ignore cleanup failures to avoid interfering with shutdown
            }
        }

        public override string ToString()
        {
            return $"WarCouncilCampaignBehavior: Manager present = {_manager != null}";
        }
    }
}