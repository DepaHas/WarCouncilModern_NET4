using System.Collections.Generic;
using WarCouncilModern.Models.Entities;

namespace WarCouncilModern.Models.Persistence
{
    public interface IPersistenceAdapter
    {
        void ExportCouncils(IEnumerable<WarCouncil> councils);
        IEnumerable<WarCouncil> ImportCouncils();
    }
}