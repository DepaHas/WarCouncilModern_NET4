using WarCouncilModern.Core.Settings;

namespace WarCouncilModern.Core.Settings
{
    public class StubModSettings : IModSettings
    {
        public bool EnableFeatureX { get; set; } = true;
        public int SomeNumericSetting { get; set; } = 10;
        public string OptionalString { get; set; } = string.Empty;

        public StubModSettings() { }

        public static StubModSettings CreateDefault() => new StubModSettings();
    }
}