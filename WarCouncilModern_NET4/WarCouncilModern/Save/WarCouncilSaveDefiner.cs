using System.Collections.Generic;
using TaleWorlds.SaveSystem;
using WarCouncilModern.Models;
using WarCouncilModern.CouncilSystem;

namespace WarCouncilModern.Save
{
    public class WarCouncilSaveDefiner : SaveableTypeDefiner
    {
        // رقم المعرف الخاص بمودك ضمن نطاق SaveableTypeDefiner
        public WarCouncilSaveDefiner() : base(2450000) { }

        // تأكد أن توقيع override يطابق تعريف القاعدة في النسخة المرجعية من TaleWorlds.SaveSystem
        protected override void DefineClassTypes()
        {
            AddClassDefinition(typeof(WarHero), 10001);
            AddClassDefinition(typeof(WarCamp), 10002);
            AddClassDefinition(typeof(WarReport), 10003);
            AddClassDefinition(typeof(SaveableEtClass1), 10004);

            AddClassDefinition(typeof(WarDecision), 10005);
            AddClassDefinition(typeof(WarCouncil), 10006);
            AddClassDefinition(typeof(WarCouncilManager), 10007);
        }

        // تأكد أن توقيع override يطابق تعريف القاعدة — غالباً protected override
        protected override void DefineContainerDefinitions()
        {
            ConstructContainerDefinition(typeof(List<WarHero>));
            ConstructContainerDefinition(typeof(List<WarCamp>));
            ConstructContainerDefinition(typeof(List<WarReport>));
            ConstructContainerDefinition(typeof(List<WarDecision>));
            ConstructContainerDefinition(typeof(List<WarCouncil>));
            ConstructContainerDefinition(typeof(Dictionary<string, WarCouncil>));
        }

        // تأكد أن توقيع override يطابق تعريف القاعدة — غالباً protected override
        protected override void DefineEnumTypes()
        {
            AddEnumDefinition(typeof(CouncilRole), 1);
            AddEnumDefinition(typeof(CouncilStructure), 2);
        }
    }
}