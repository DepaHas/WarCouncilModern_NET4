using TaleWorlds.SaveSystem;

namespace WarCouncilModern.Models.Entities
{
    public class DecisionExecutionPayload
    {
        [SaveableField(1)]
        public string TargetFactionId { get; set; }
    }
}
