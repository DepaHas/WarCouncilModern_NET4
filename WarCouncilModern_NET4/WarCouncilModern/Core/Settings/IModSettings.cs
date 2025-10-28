namespace WarCouncilModern.Core.Settings
{
    public interface IModSettings
    {
        bool AutoProcessDecisions { get; }
        bool AutoScheduleMeetingOnProposal { get; }
    }
}