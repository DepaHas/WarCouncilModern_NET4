using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.SaveSystem;
using TaleWorlds.CampaignSystem;
using WarCouncilModern.Models;

namespace WarCouncilModern.CouncilSystem
{
    /// <summary>
    /// مدير المجالس؛ يخزن خريطة KingdomStringId -> WarCouncil ويُحفظ عبر SaveSystem.
    /// يتضمن خاصية Instance للوصول العالمي الآمن وإجراءات مساعدة للتهيئة وإدارة التقارير.
    /// </summary>
    public class WarCouncilManager
    {
        // Singleton عام لتسهيل الوصول من السلوكيات وقطع الواجهة الأخرى.
        // يمكن إعادة تعيينه عند SyncData بعد التحميل.
        public static WarCouncilManager Instance { get; set; }

        [SaveableField(1)] private Dictionary<string, WarCouncil> _allCouncils;

        private readonly object _lock = new object();

        public WarCouncilManager()
        {
            _allCouncils = new Dictionary<string, WarCouncil>(StringComparer.Ordinal);
        }

        public IReadOnlyDictionary<string, WarCouncil> AllCouncils
        {
            get { lock (_lock) { return new Dictionary<string, WarCouncil>(_allCouncils); } }
        }

        public bool TryGetCouncil(string kingdomId, out WarCouncil council)
        {
            council = null;
            if (string.IsNullOrEmpty(kingdomId)) return false;
            lock (_lock) return _allCouncils.TryGetValue(kingdomId, out council);
        }

        public WarCouncil GetOrCreateCouncil(string kingdomId)
        {
            if (string.IsNullOrEmpty(kingdomId)) throw new ArgumentNullException(nameof(kingdomId));
            lock (_lock)
            {
                if (!_allCouncils.TryGetValue(kingdomId, out var c))
                {
                    c = new WarCouncil(kingdomId, CouncilStructure.RoyalAppointed);
                    _allCouncils[kingdomId] = c;
                }
                return c;
            }
        }

        public bool RemoveCouncil(string kingdomId)
        {
            if (string.IsNullOrEmpty(kingdomId)) return false;
            lock (_lock) return _allCouncils.Remove(kingdomId);
        }

        /// <summary>
        /// يملأ المجالس الأساسية اعتمادًا على Kingdoms المتاحة في الحملة.
        /// يجب استدعاءه بعد أن تكون بيانات الحملة جاهزة (بعد تحميل اللعبة).
        /// </summary>
        public void InitializeDataFromKingdoms()
        {
            lock (_lock)
            {
                _allCouncils.Clear();

                foreach (var kingdom in Kingdom.All)
                {
                    if (kingdom == null) continue;
                    var kId = kingdom.StringId;
                    if (string.IsNullOrEmpty(kId)) continue;
                    if (_allCouncils.ContainsKey(kId)) continue;

                    var council = new WarCouncil(kId, CouncilStructure.RoyalAppointed);

                    try
                    {
                        var candidates = new List<string>();

                        // محاولة استخدام kingdom.Heroes إن كانت متاحة
                        var kingdomHeroes = kingdom.Heroes;
                        if (kingdomHeroes != null)
                        {
                            foreach (var h in kingdomHeroes)
                            {
                                if (h == null) continue;
                                if (h.IsLord) candidates.Add(h.StringId ?? string.Empty);
                            }
                        }

                        // كاحتياط، نبحث عبر عشائر المملكة
                        if (candidates.Count == 0)
                        {
                            foreach (var clan in Clan.All)
                            {
                                if (clan == null) continue;
                                foreach (var h in clan.Heroes ?? Enumerable.Empty<Hero>())
                                {
                                    if (h == null) continue;
                                    if (h.IsLord && h.Clan?.Kingdom?.StringId == kId)
                                        candidates.Add(h.StringId ?? string.Empty);
                                }
                            }
                        }

                        council.AssignMembersByHeroIds(candidates);
                        if (candidates.Count > 0) council.AssignLeaderByHeroId(candidates[0]);
                    }
                    catch
                    {
                        // تجاهل مشاكل الوصول إلى API واستمر بإنشاء المجلس
                    }

                    _allCouncils[kId] = council;
                }
            }
        }

        /// <summary>
        /// أضف قرارًا إلى مجلس المملكة (يُنشئ المجلس إن لم يكن موجودًا).
        /// </summary>
        public void AddDecisionToCouncil(string kingdomId, WarDecision decision)
        {
            if (string.IsNullOrEmpty(kingdomId) || decision == null) return;
            lock (_lock)
            {
                if (!_allCouncils.TryGetValue(kingdomId, out var c))
                {
                    c = new WarCouncil(kingdomId, CouncilStructure.RoyalAppointed);
                    _allCouncils[kingdomId] = c;
                }
                c.AddDecision(decision);
            }
        }

        /// <summary>
        /// أضف تقريرًا إلى مجلس المملكة (يُنشئ المجلس إن لم يكن موجودًا).
        /// </summary>
        public void AddReportToCouncil(string kingdomId, WarReport report)
        {
            if (string.IsNullOrEmpty(kingdomId) || report == null) return;
            lock (_lock)
            {
                if (!_allCouncils.TryGetValue(kingdomId, out var c))
                {
                    c = new WarCouncil(kingdomId, CouncilStructure.RoyalAppointed);
                    _allCouncils[kingdomId] = c;
                }
                c.AddReport(report);
            }
        }

        public IEnumerable<WarCouncil> GetAllCouncilsSnapshot()
        {
            lock (_lock) return _allCouncils.Values.ToList();
        }

        public void ClearAllCouncils()
        {
            lock (_lock) _allCouncils.Clear();
        }

        /// <summary>
        /// يسمح بتغيير هيكل مجلس محدد (لإدخال منطق لعبة يتبدّل حسب الأحداث).
        /// </summary>
        public bool SetCouncilStructure(string kingdomId, CouncilStructure structure)
        {
            if (string.IsNullOrEmpty(kingdomId)) return false;
            lock (_lock)
            {
                if (!_allCouncils.TryGetValue(kingdomId, out var c)) return false;
                c.Structure = structure;
                return true;
            }
        }

        /// <summary>
        /// الحصول على مجلس عبر البحث في كل المجالس بالاسم (غير مُستخدم عادةً إن كنت تمتلك الـ key).
        /// </summary>
        public WarCouncil FindCouncilByKingdomId(string kingdomId)
        {
            if (string.IsNullOrEmpty(kingdomId)) return null;
            lock (_lock)
            {
                _allCouncils.TryGetValue(kingdomId, out var c);
                return c;
            }
        }

        public override string ToString()
        {
            lock (_lock) return string.Format("WarCouncilManager: {0} councils tracked", _allCouncils.Count);
        }
    }
}