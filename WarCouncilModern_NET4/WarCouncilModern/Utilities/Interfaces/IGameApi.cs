using System.Collections.Generic;
using TaleWorlds.CampaignSystem;

namespace WarCouncilModern.Utilities.Interfaces
{
    public interface IGameApi
    {
        Hero? GetRulerHeroForKingdom(string kingdomId);
        IEnumerable<Hero> GetClanLeadersForKingdom(string kingdomId);
        int GetRelationBetween(Hero hero1, Hero hero2);
        Hero? FindHeroByStringId(string heroId);
        IFaction? FindFactionById(string factionId);
        Kingdom? FindKingdomByStringId(string kingdomId);
        void ChangeRelationBetween(IFaction faction1, IFaction faction2, int relationDelta);
    }
}
