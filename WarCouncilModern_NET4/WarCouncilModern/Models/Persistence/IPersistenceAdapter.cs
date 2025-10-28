using System.Collections.Generic;
using WarCouncilModern.CouncilSystem;
using WarCouncilModern.Models.Entities;

namespace WarCouncilModern.Models.Persistence
{
    public interface IPersistenceAdapter
    {
        void ExportCouncils(IEnumerable<WarCouncil> councils);
        IEnumerable<WarCouncil> ImportCouncils();
    }
}