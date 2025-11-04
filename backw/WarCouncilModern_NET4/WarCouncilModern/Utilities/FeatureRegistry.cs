using WarCouncilModern.Core.Settings;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.Utilities
{
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
