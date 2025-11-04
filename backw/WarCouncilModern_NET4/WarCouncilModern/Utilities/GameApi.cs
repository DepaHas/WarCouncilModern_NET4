using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
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
            var kingdom = FindKingdomByStringId(kingdomId);
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
            var kingdom = FindKingdomByStringId(kingdomId);
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
            Hero? leader1 = GetRepresentativeHeroForFaction(faction1);
            Hero? leader2 = GetRepresentativeHeroForFaction(faction2);
            if (leader1 != null && leader2 != null)
            {
                ChangeRelationAction.ApplyRelationChangeBetweenHeroes(leader1, leader2, relationDelta, true);
            }
        }

        private Hero? GetRepresentativeHeroForFaction(IFaction faction)
        {
            if (faction.IsKingdomFaction)
            {
                return ((Kingdom)faction).Leader;
            }
            if (faction is Clan clan)
            {
                return clan.Leader;
            }
            return null;
        }

    }
}
