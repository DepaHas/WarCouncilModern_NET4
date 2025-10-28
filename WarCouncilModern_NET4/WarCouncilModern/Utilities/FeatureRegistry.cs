using System.Collections.Concurrent;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.Utilities
{
    public class FeatureRegistry : IFeatureRegistry
    {
        private readonly ConcurrentDictionary<string, bool> _flags = new();

        public FeatureRegistry()
        {
            // قيم افتراضية؛ يمكن ملؤها من ModSettings عند التهيئة
            _flags["AutoDecisionProcessing"] = false;
            _flags["AutoScheduleMeetingOnProposal"] = true;
        }

        public bool IsEnabled(string featureKey)
        {
            if (string.IsNullOrWhiteSpace(featureKey)) return false;
            return _flags.TryGetValue(featureKey, out var value) && value;
        }

        public void Set(string featureKey, bool enabled) => _flags[featureKey] = enabled;
    }
}