using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using WarCouncilModern.Models.Entities;
using TaleWorlds.SaveSystem;

namespace WarCouncilModern.CouncilSystem.Behaviors
{
    public class WarCouncilCampaignBehavior : CampaignBehaviorBase
    {
        [SaveableField(1)]
        private Dictionary<string, WarCouncil> _councils = new Dictionary<string, WarCouncil>();

        public Dictionary<string, WarCouncil> Councils => _councils;

        public override void RegisterEvents()
        {
            // No longer needed to listen to game loaded event
        }

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("_councils", ref _councils);
        }

        public override string Id => "WarCouncilCampaignBehavior";

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
