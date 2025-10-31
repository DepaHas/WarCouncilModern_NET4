using WarCouncilModern.Core.Settings;

namespace WarCouncilModern.Core.State
{
    public static class FeatureKeys
    {
        public const string AutoScheduleMeetingOnProposal = "AutoScheduleMeetingOnProposal";
        public const string AutoDecisionProcessing = "AutoDecisionProcessing";
    }

    public interface IFeatureRegistry
    {
        bool IsEnabled(string featureName);
    }

    public class FeatureRegistry : IFeatureRegistry
    {
        private readonly IModSettings _settings;

        public FeatureRegistry(IModSettings settings)
        {
            _settings = settings;
        }

        public bool IsEnabled(string featureName)
        {
            // Defaulting to false as per safety guidelines.
            // A real implementation would check _settings.
            return false;
        }
    }
}
