using System;
using System.Linq;
using TaleWorlds.SaveSystem;
using TaleWorlds.CampaignSystem;

namespace WarCouncilModern.Models.Entities

{
    /// <summary>
    /// نموذج مبسّط من البطل قابل للحفظ داخل نظام WarCouncil.
    /// متوافق مع .NET Framework 4.7. لا تعتمد على Hero.All.
    /// </summary>
    public class WarHero
    {
        [SaveableField(1)] private string _heroStringId;
        [SaveableField(2)] private string _name;
        [SaveableField(3)] private float _age;
        [SaveableField(4)] private bool _isLord;

        public WarHero()
        {
            _heroStringId = string.Empty;
            _name = string.Empty;
            _age = 0f;
            _isLord = false;
        }

        public WarHero(string id, string name, float age, bool isLord)
        {
            _heroStringId = id ?? string.Empty;
            _name = name ?? string.Empty;
            _age = age;
            _isLord = isLord;
        }

        public string HeroStringId
        {
            get { return _heroStringId; }
            set { _heroStringId = value ?? string.Empty; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value ?? string.Empty; }
        }

        public float Age
        {
            get { return _age; }
            set { _age = value; }
        }

        public bool IsLord
        {
            get { return _isLord; }
            set { _isLord = value; }
        }

        public static WarHero FromGameHero(Hero h)
        {
            if (h == null) return null;
            return new WarHero(
                h.StringId ?? string.Empty,
                h.Name != null ? h.Name.ToString() : string.Empty,
                h.Age,
                h.IsLord
            );
        }

        // محاولة استرجاع كائن Hero باستخدام ObjectManager ثم fallbacks
        public bool TryGetGameHero(out Hero hero)
        {
            hero = null;
            if (string.IsNullOrEmpty(_heroStringId)) return false;

            try
            {
                // 1) ObjectManager (أفضل خيار إن كان متاحاً)
                var campaign = Campaign.Current;
                if (campaign != null)
                {
                    try
                    {
                        var objManager = campaign.ObjectManager;
                        if (objManager != null)
                        {
                            var obj = objManager.GetObject<Hero>(_heroStringId);
                            if (obj != null)
                            {
                                hero = obj;
                                return true;
                            }
                        }
                    }
                    catch { /* ignore and fallback */ }
                }

                // 2) Kingdoms -> Heroes (fallback شائع)
                foreach (var kingdom in Kingdom.All)
                {
                    if (kingdom == null) continue;
                    var kh = kingdom.Heroes;
                    if (kh == null) continue;
                    foreach (var h in kh)
                    {
                        if (h == null) continue;
                        if (string.Equals(h.StringId, _heroStringId, StringComparison.Ordinal))
                        {
                            hero = h;
                            return true;
                        }
                    }
                }

                // 3) Clans -> Heroes و Clan.Leader
                foreach (var clan in Clan.All)
                {
                    if (clan == null) continue;
                    var ch = clan.Heroes;
                    if (ch != null)
                    {
                        foreach (var h in ch)
                        {
                            if (h == null) continue;
                            if (string.Equals(h.StringId, _heroStringId, StringComparison.Ordinal))
                            {
                                hero = h;
                                return true;
                            }
                        }
                    }

                    if (clan.Leader != null && string.Equals(clan.Leader.StringId, _heroStringId, StringComparison.Ordinal))
                    {
                        hero = clan.Leader;
                        return true;
                    }
                }
            }
            catch { /* ignore */ }

            return false;
        }

        public override bool Equals(object obj)
        {
            var other = obj as WarHero;
            if (other == null) return false;
            return string.Equals(_heroStringId, other._heroStringId, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(_heroStringId ?? string.Empty);
        }

        public override string ToString()
        {
            return string.Format("{0} (Age: {1:0}) {2}", Name, Age, IsLord ? "[Lord]" : "[Commoner]");
        }
    }
}