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
            // This is a placeholder for a real implementation that would check the settings.
            // For now, we will consider all features enabled.
            return true;
        }
    }
}
