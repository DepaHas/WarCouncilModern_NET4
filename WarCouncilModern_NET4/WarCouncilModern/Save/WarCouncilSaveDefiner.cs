using System.Collections.Generic;
using TaleWorlds.SaveSystem;
using WarCouncilModern.Models.Entities;

namespace WarCouncilModern.Save
{
    public class WarCouncilSaveDefiner : SaveableTypeDefiner
    {
        // رمز فريد ضمن نطاقك. تأكد أنه لا يتصادم مع مودات أخرى.
        public WarCouncilSaveDefiner() : base(2450000) { }

        protected override void DefineClassTypes()
        {
            AddClassDefinition(typeof(WarHero), 10001);
            AddClassDefinition(typeof(WarCamp), 10002);
            AddClassDefinition(typeof(WarReport), 10003);
            AddClassDefinition(typeof(WarDecision), 10004);
            AddClassDefinition(typeof(WarCouncil), 10005);
            // لا تُسجّل المدير أو أنواع غير قابلة للحفظ هنا
        }

        protected override void DefineContainerDefinitions()
        {
            ConstructContainerDefinition(typeof(List<WarHero>));
            ConstructContainerDefinition(typeof(List<WarCamp>));
            ConstructContainerDefinition(typeof(List<WarReport>));
            ConstructContainerDefinition(typeof(List<WarDecision>));
            ConstructContainerDefinition(typeof(List<WarCouncil>));
            ConstructContainerDefinition(typeof(Dictionary<string, WarCouncil>));
        }

        protected override void DefineEnumTypes()
        {
            AddEnumDefinition(typeof(CouncilRole), 1);
            AddEnumDefinition(typeof(CouncilStructure), 2);
        }
    }
}