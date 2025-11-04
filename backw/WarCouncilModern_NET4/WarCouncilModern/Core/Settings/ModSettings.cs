using WarCouncilModern.Core.Settings;

namespace WarCouncilModern.Models.Settings
{
    public class ModSettings : IModSettings
    {
        public bool EnableFeatureX { get; set; } = true;
        public int SomeNumericSetting { get; set; } = 10;
        public string OptionalString { get; set; } = string.Empty;

        public ModSettings() { }

        public static ModSettings CreateDefault() => new ModSettings();
    }
}