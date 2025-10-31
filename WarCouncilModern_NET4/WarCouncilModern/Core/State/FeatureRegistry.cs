namespace WarCouncilModern.Core.State
{
    public interface IFeatureRegistry
    {
        bool IsEnabled(string featureName);
    }

    public class FeatureRegistry : IFeatureRegistry
    {
        public bool IsEnabled(string featureName)
        {
            // For now, all features are enabled.
            // This can be replaced with a proper implementation that reads from a config file.
            return true;
        }
    }
}
