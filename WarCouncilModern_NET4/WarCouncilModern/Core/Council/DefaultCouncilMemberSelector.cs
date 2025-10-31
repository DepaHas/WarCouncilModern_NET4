using System.Collections.Generic;
using System.Linq;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.Core.Council
{
    public class DefaultCouncilMemberSelector : ICouncilMemberSelector
    {
        private readonly IGameApi _gameApi;
        private readonly IModLogger _logger;

        public DefaultCouncilMemberSelector(IGameApi gameApi, IModLogger logger)
        {
            _gameApi = gameApi;
            _logger = logger;
        }

        public IEnumerable<string> GetMembersForCouncil(string kingdomStringId, CouncilContext context = null)
        {
            var result = new List<string>();
            var ruler = _gameApi.GetRulerHeroForKingdom(kingdomStringId);
            if (ruler != null)
            {
                result.Add(ruler.StringId);
            }

            var clanLeaders = _gameApi.GetClanLeadersForKingdom(kingdomStringId);
            foreach (var clanLeader in clanLeaders)
            {
                if (ruler != null)
                {
                    var relation = _gameApi.GetRelationBetween(ruler, clanLeader);
                    if (relation >= 10) // To-do: Make this configurable
                    {
                        result.Add(clanLeader.StringId);
                    }
                }
                else
                {
                    result.Add(clanLeader.StringId);
                }
            }
            _logger.Info($"[MemberSelector] Found {result.Count} members for kingdom {kingdomStringId}.");
            return result.Distinct();
        }
    }
}
