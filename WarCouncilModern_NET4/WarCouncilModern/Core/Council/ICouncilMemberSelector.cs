using System.Collections.Generic;
using WarCouncilModern.Models.Entities;

namespace WarCouncilModern.Core.Council
{
    public class CouncilContext
    {
        public string Reason { get; set; }
    }

    public interface ICouncilMemberSelector
    {
        IEnumerable<string> GetMembersForCouncil(string kingdomStringId, CouncilContext context = null);
    }
}
