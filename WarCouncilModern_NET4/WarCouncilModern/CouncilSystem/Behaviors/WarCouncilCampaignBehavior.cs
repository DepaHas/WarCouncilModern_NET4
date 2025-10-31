using TaleWorlds.CampaignSystem;
using WarCouncilModern.Core.Manager;
using WarCouncilModern.Initialization;

namespace WarCouncilModern.CouncilSystem.Behaviors
{
    public class WarCouncilCampaignBehavior : CampaignBehaviorBase
    {
        private readonly IWarCouncilManager _warCouncilManager;

        public WarCouncilCampaignBehavior()
        {
            // This is a temporary solution for dependency injection.
            // A proper DI framework should be used in the future.
            _warCouncilManager = SubModule.WarCouncilManager;
        }

        public override void RegisterEvents()
        {
            CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, OnGameLoaded);
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, OnSessionLaunched);
        }

        public override void SyncData(IDataStore dataStore)
        {
            if (dataStore.IsSaving)
            {
                _warCouncilManager.ExportAllForSave();
            }
        }

        private void OnGameLoaded(CampaignGameStarter gameStarter)
        {
            _warCouncilManager.ImportFromSave();
        }

        private void OnSessionLaunched(CampaignGameStarter gameStarter)
        {
            // This is where we would add the behavior to the game starter.
            // We will do this in SubModule.cs
        }
    }
}
