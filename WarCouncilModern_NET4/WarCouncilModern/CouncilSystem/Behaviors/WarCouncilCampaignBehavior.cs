using System.Collections.Generic;
using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using WarCouncilModern.Initialization;
using WarCouncilModern.Models.Entities;
using TaleWorlds.SaveSystem;
using WarCouncilModern.Utilities;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.CouncilSystem.Behaviors
{
    public class WarCouncilCampaignBehavior : CampaignBehaviorBase
    {
        [SaveableField(1)]
        private Dictionary<string, WarCouncil> _councils = new Dictionary<string, WarCouncil>();

        private bool _persistenceRestored = false;

        public Dictionary<string, WarCouncil> Councils => _councils;

        public override void RegisterEvents()
        {
            CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, OnGameLoaded);
            // TODO: Verify this is the correct event for pre-save logic in the target Bannerlord version.
            CampaignEvents.OnBeforeSaveEvent.AddNonSerializedListener(this, OnGameSaved);
        }

        private void OnGameLoaded(CampaignGameStarter gameStarter)
        {
            if (_persistenceRestored) return;

            try
            {
                SubModule.WarCouncilManager?.RebuildReferencesAfterLoad(new GameApi());
                _persistenceRestored = true;
                EnsureInitialCouncilsOnce();
            }
            catch (Exception ex)
            {
                SubModule.Logger.Error("[WarCouncilCampaignBehavior] Error during OnGameLoaded", ex);
            }
        }

        private void EnsureInitialCouncilsOnce()
        {
            var playerKingdom = Campaign.Current?.Kingdoms?.FirstOrDefault(k => k.IsPlayerKingdom);
            if (playerKingdom == null || SubModule.WarCouncilManager == null || SubModule.CouncilService == null) return;

            if (!SubModule.WarCouncilManager.HasActiveCouncilForKingdom(playerKingdom.StringId))
            {
                SubModule.CouncilService.StartCouncilForKingdom(playerKingdom);
            }
        }

        private void OnGameSaved()
        {
            SubModule.Logger.Info("[WarCouncilCampaignBehavior] Game saved.");
        }

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("_councils", ref _councils);
        }

        public void AddCouncil(WarCouncil council)
        {
            if (!_councils.ContainsKey(council.SaveId))
            {
                _councils.Add(council.SaveId, council);
            }
        }

        public WarCouncil GetCouncilById(string saveId)
        {
            _councils.TryGetValue(saveId, out var council);
            return council;
        }

        public bool RemoveCouncil(string saveId)
        {
            return _councils.Remove(saveId);
        }
    }
}
