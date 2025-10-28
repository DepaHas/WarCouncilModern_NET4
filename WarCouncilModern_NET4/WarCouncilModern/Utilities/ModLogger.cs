using System;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.Utilities
{
    public class ModLogger : IModLogger
    {
        private readonly string _prefix;

        public ModLogger(string prefix = "WarCouncil")
        {
            _prefix = prefix;
        }

        public void Debug(string message) => Write("DEBUG", message);
        public void Info(string message) => Write("INFO", message);
        public void Warn(string message) => Write("WARN", message);
        public void Error(string message) => Write("ERROR", message);
        public void Error(string message, Exception ex) => Write("ERROR", $"{message} - Exception: {ex}");

        private void Write(string level, string msg)
        {
            var text = $"[{_prefix}] [{level}] {msg}";
            // هنا تضع آلية السجل الفعلية: ملف، TaleWorlds.Logging، أو Console
            System.Diagnostics.Debug.WriteLine(text);
        }
    }
}