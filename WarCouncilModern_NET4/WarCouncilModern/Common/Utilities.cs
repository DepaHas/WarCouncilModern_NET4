using System;

namespace WarCouncilModern.Common
{
    public static class Utilities
    {
        public static string SafeToString(object o) => o == null ? "<null>" : o.ToString();
    }
}