using System;
using TaleWorlds.SaveSystem;

namespace WarCouncilModern.Models.Entities
{
    public class WarHero
    {
        [SaveableField(1)] private string _heroId;
        [SaveableField(2)] private string _name;
        [SaveableField(3)] private int _influence;
        [SaveableField(4)] private string _clanId;

        public WarHero()
        {
            _heroId = Guid.NewGuid().ToString();
            _name = string.Empty;
            _influence = 0;
            _clanId = string.Empty;
        }

        public WarHero(string heroId, string name, int influence = 0, string clanId = "")
        {
            _heroId = heroId ?? Guid.NewGuid().ToString();
            _name = name ?? string.Empty;
            _influence = Math.Max(0, influence);
            _clanId = clanId ?? string.Empty;
        }

        // الاسم/المعرف الذي يبدو أن بقية الكود يتوقعه
        public string HeroStringId { get { return _heroId; } set { _heroId = value ?? Guid.NewGuid().ToString(); } }

        public string HeroId { get { return _heroId; } set { _heroId = value ?? Guid.NewGuid().ToString(); } }
        public string Name { get { return _name; } set { _name = value ?? string.Empty; } }
        public int Influence { get { return _influence; } set { _influence = Math.Max(0, value); } }
        public string ClanId { get { return _clanId; } set { _clanId = value ?? string.Empty; } }

        public override string ToString() => $"Hero[{HeroStringId}]: {Name} (Inf:{Influence})";
    }
}