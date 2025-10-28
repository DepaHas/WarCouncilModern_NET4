using WarCouncilModern.Core.Settings;

namespace WarCouncilModern.Core.Settings
{
    public class StubModSettings : IModSettings
    {
        public bool AutoProcessDecisions { get; set; } = false;
        public bool AutoScheduleMeetingOnProposal { get; set; } = true;
    }
}