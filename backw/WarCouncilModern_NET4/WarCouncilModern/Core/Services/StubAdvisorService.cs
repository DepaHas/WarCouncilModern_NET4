#nullable enable
using System;
using System.Collections.Generic;
using System.Reflection;
using WarCouncilModern.Models.Entities;
using WarCouncilModern.Utilities.Interfaces;
using WarCouncilModern.Core.Manager;
using WarCouncilModern.Utilities;

namespace WarCouncilModern.Core.Services
{
    public sealed class StubAdvisorService : IAdvisorService
    {
        private readonly IModLogger _logger;

        public StubAdvisorService(IModLogger? logger = null)
        {
            _logger = logger ?? GlobalLog.Instance ?? new NullLogger();
            _logger.Info("StubAdvisorService created.");
        }

        public void Initialize(object? initArgs = null)
        {
            _logger.Info("StubAdvisorService.Initialize called.");
        }

        public void Shutdown()
        {
            _logger.Info("StubAdvisorService.Shutdown called.");
        }

        public IEnumerable<string> GetAdvisorRecommendations(WarCouncil council)
        {
            _logger.Debug("StubAdvisorService.GetAdvisorRecommendations called.");
            return new List<string>
            {
                "Gather intelligence before moving",
                "Delay offensive until reinforcements arrive"
            };
        }

        public void ApplyAdvice(WarCouncil council, string advice)
        {
            _logger.Info($"StubAdvisorService.ApplyAdvice invoked with advice: {advice}");
        }

        public void InitializeForCouncil(WarCouncil council)
        {
            var councilId = GetMemberAsStringSafely(council, new[] { "Name", "Title", "Id" }) ?? council?.ToString() ?? "null";
            _logger.Info($"StubAdvisorService.InitializeForCouncil called for council: {councilId}");
        }

        public void OnDecisionProposed(WarCouncil council, WarDecision decision)
        {
            var councilId = GetMemberAsStringSafely(council, new[] { "Name", "Title", "Id" }) ?? council?.ToString() ?? "null";
            var decisionTitle = GetMemberAsStringSafely(decision, new[] { "Title", "Name", "Id" }) ?? decision?.ToString() ?? "unknown";

            _logger.Debug($"StubAdvisorService.OnDecisionProposed called. Council: {councilId}, Decision: {decisionTitle}");
            _logger.Info($"StubAdvisorService produced recommendation for decision '{decisionTitle}'");
        }

        private static string? GetMemberAsStringSafely(object? obj, string[] candidateNames)
        {
            if (obj == null) return null;

            var t = obj.GetType();
            foreach (var name in candidateNames)
            {
                var prop = t.GetProperty(name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if (prop != null)
                {
                    try
                    {
                        var v = prop.GetValue(obj);
                        if (v is string s) return s;
                        if (v != null) return v.ToString();
                    }
                    catch { }
                }

                var field = t.GetField(name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if (field != null)
                {
                    try
                    {
                        var v = field.GetValue(obj);
                        if (v is string s) return s;
                        if (v != null) return v.ToString();
                    }
                    catch { }
                }
            }

            return null;
        }
    }

    // NullLogger يطابق IModLogger بدقة (Exception? في الوسيط الثاني)
    public class NullLogger : IModLogger
    {
        public void Info(string message) { }
        public void Debug(string message) { }
        public void Warn(string message) { }
        public void Error(string message) { }
        public void Error(string message, Exception? ex) { }
    }
}