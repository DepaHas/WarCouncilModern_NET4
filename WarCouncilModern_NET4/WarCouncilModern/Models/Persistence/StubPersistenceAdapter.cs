using System.Collections.Generic;
using WarCouncilModern.Models.Entities;

namespace WarCouncilModern.Models.Persistence
{
    public class StubPersistenceAdapter : IPersistenceAdapter
    {
        private List<WarCouncil> _councils = new List<WarCouncil>();

        public void ExportCouncils(List<WarCouncil> councils)
        {
            _councils = councils;
        }

        public List<WarCouncil> ImportCouncils()
        {
            return _councils;
        }
    }
}
