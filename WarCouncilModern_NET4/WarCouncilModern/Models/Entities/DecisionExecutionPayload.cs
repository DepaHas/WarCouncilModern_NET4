using TaleWorlds.SaveSystem;

namespace WarCouncilModern.Models.Entities
{
    public class DecisionExecutionPayload
    {
        [SaveableField(1)]
        private string _targetFactionId;
        public string TargetFactionId { get => _targetFactionId; set => _targetFactionId = value; }

        [SaveableField(2)]
        private string _rawPayload;
        public string RawPayload { get => _rawPayload; set => _rawPayload = value; }
    }
}
