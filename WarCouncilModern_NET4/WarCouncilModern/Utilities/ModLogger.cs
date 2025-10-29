using System;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.Utilities.Logging
{
    public class ModLogger : IModLogger
    {
        private readonly string _prefix;
        public ModLogger(string prefix = "WarCouncil") { _prefix = prefix; }

        public void Info(string message) => Log("INFO", message);
        public void Warn(string message) => Log("WARN", message);
        public void Debug(string message) => Log("DEBUG", message);
        public void Error(string message) => Error(message, null);
        public void Error(string message, Exception? ex)
        {
            var payload = ex == null ? message : $"{message} | Exception: {ex}";
            Log("ERROR", payload);
        }

        private void Log(string level, string message)
        {
            try { System.Diagnostics.Debug.WriteLine($"[{_prefix}][{level}] {message}"); }
            catch { }
        }
    }
}