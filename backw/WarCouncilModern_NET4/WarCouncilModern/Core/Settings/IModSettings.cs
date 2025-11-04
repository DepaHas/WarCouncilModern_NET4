namespace WarCouncilModern.Core.Settings
{
    public interface IModSettings
    {
        bool EnableFeatureX { get; set; }
        int SomeNumericSetting { get; set; }
        string OptionalString { get; set; }
    }
}