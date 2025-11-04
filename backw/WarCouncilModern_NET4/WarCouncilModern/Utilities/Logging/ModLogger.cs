using System;
using System.IO;
using TaleWorlds.Library;
using System.Text;

namespace WarCouncilModern.Logging
{
    public static class ModLogger
    {
        private static readonly string LogDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Mount & Blade II Bannerlord", "ModLogs", "WarCouncilModern");
        private static readonly string LogFile = Path.Combine(LogDir, $"log_{DateTime.Now:yyyyMMdd_HHmmss}.txt");

        static ModLogger()
        {
            try
            {
                Directory.CreateDirectory(LogDir);
                File.AppendAllText(LogFile, $"[Init] WarCouncilModern log started at {DateTime.Now}\n");
            }
            catch { /* swallow */ }
        }

        public static void Info(string msg) => Write("INFO", msg);
        public static void Warn(string msg) => Write("WARN", msg);
        public static void Error(string msg) => Write("ERROR", msg);

        private static void Write(string level, string msg)
        {
            try
            {
                var line = $"[{DateTime.Now:HH:mm:ss}][{level}] {msg}\n";
                File.AppendAllText(LogFile, line, Encoding.UTF8);
                // Show minimal info in dev builds
#if DEBUG
                InformationManager.DisplayMessage(new InformationMessage($"[{level}] {msg}"));
#endif
            }
            catch { /* ignore logging errors */ }
        }

        public static string? GetLatestLogPath() => File.Exists(LogFile) ? LogFile : null;
    }
}