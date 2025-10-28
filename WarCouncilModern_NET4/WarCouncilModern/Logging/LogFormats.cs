namespace WarCouncilModern.Logging
{
    public static class LogFormats
    {
        // Example helper for structured messages
        public static string Phase(string phase, string @class, string method, string message)
            => $"[{phase}][{@class}][{method}] {message}";
    }
}