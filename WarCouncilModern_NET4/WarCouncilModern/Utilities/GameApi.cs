using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.Utilities
{
    public class GameApi : IGameApi
    {
        public Hero? FindHeroByStringId(string heroId)
        {
            return Hero.FindAll(h => h.StringId == heroId).FirstOrDefault();
        }

        public IEnumerable<Hero> GetClanLeadersForKingdom(string kingdomId)
        {
            var kingdom = Kingdom.All.FirstOrDefault(k => k.StringId == kingdomId);
            if (kingdom == null)
            {
                return Enumerable.Empty<Hero>();
            }
            return kingdom.Clans.Where(c => c.Leader != null).Select(c => c.Leader);
        }

        public int GetRelationBetween(Hero hero1, Hero hero2)
        {
            return hero1.GetRelation(hero2);
        }

        public Hero? GetRulerHeroForKingdom(string kingdomId)
        {
            var kingdom = Kingdom.All.FirstOrDefault(k => k.StringId == kingdomId);
            return kingdom?.Leader;
        }

        public IFaction? FindFactionById(string factionId)
        {
            return Campaign.Current.Factions.FirstOrDefault(f => f.StringId == factionId);
        }

        public Kingdom? FindKingdomByStringId(string kingdomId)
        {
            return Kingdom.All.FirstOrDefault(k => k.StringId == kingdomId);
        }

        public void ChangeRelationBetween(IFaction faction1, IFaction faction2, int relationDelta)
        {
            FactionManager.SetRelation(faction1, faction2, FactionManager.GetRelation(faction1, faction2) + relationDelta);
        }
    }
}
