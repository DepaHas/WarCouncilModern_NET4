using WarCouncilModern.Utilities.Interfaces;
using WarCouncilModern.Utilities.Logging;

namespace WarCouncilModern.Utilities
{
    public static class GlobalLog
    {
        public static IModLogger Instance { get; set; } = new ModLogger("WarCouncil");
    }
}