using System;
using TaleWorlds.Library;

namespace WarCouncilModern.Core
{
    public static class CompatibilityGuard
    {
        public static bool IsGameVersionCompatible(string minVersion)
        {
            try
            {
                // Flexible parsing: accept "v1.0", "1.0.0", "1.0" etc.
                var current = ApplicationVersion.CurrentVersion?.ToString() ?? "0.0.0";
                var normalizedCurrent = NormalizeVersion(current);
                var normalizedMin = NormalizeVersion(minVersion);
                return string.Compare(normalizedCurrent, normalizedMin, StringComparison.Ordinal) >= 0;
            }
            catch
            {
                ModLogger.Warn("[CompatibilityGuard] Failed to parse version; allowing by fallback.");
                return true;
            }
        }

        private static string NormalizeVersion(string v)
        {
            if (string.IsNullOrEmpty(v)) return "0.0.0";
            v = v.Trim().TrimStart('v', 'V');
            var parts = v.Split('.');
            while (parts.Length < 3) v += ".0";
            return v;
        }
    }
}