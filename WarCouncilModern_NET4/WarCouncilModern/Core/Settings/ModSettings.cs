namespace WarCouncilModern.Core.Settings
{
    public class ModSettings : IModSettings
    {
        public bool EnableCouncilUI { get; set; } = true;
        public bool EnableCouncilDevTools { get; set; } = false;
    }
}
