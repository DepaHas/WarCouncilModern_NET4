using System;
using System.Collections.Generic;
using WarCouncilModern.Models.Entities;

namespace WarCouncilModern.Models.Persistence
{
    public class CouncilPersistenceAdapter : IPersistenceAdapter
    {
        // تنفيذ مبسّط: يمكن استبداله بربط مع Save system الخاص باللعبة
        private readonly List<WarCouncil> _store = new List<WarCouncil>();

        public void ExportCouncils(IEnumerable<WarCouncil> councils)
        {
            _store.Clear();
            if (councils == null) return;
            _store.AddRange(councils);
        }

        public IEnumerable<WarCouncil> ImportCouncils()
        {
            // إعادة قائمة عميقة إن لزم؛ هنا نعيد النسخ المخزنة مباشرةً
            foreach (var c in _store) yield return c;
        }
    }
}