namespace WarCouncilModern.Core
{
    public static class ModSettings
    {
        // Feature flags — افتراضيًا false لحماية التشغيل
        public static bool EnableWarCouncilLogic { get; set; } = false;
        public static bool EnableSaveTests { get; set; } = false;
        public static bool EnableUI { get; set; } = false;

        // Runtime-overrides for dev use (can be set from SubModule on startup)
        public static void ApplyDefaults() { }
    }
}