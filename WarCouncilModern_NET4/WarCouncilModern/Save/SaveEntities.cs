using TaleWorlds.SaveSystem;
using WarCouncilModern.Core;

namespace WarCouncilModern.Save
{
    [SaveableClass(Constants.SaveableNamespaceId + 1)]
    public class SaveableWarCouncilData
    {
        [SaveableField(1)] public int SomeValue;
        [SaveableField(2)] public string Tag;

        public SaveableWarCouncilData() { }
    }
}