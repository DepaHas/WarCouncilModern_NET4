using System;
using TaleWorlds.SaveSystem;

namespace WarCouncilModern.Models.Entities
{
    public class WarReport
    {
        [SaveableField(1)] private string _reportId;
        [SaveableField(2)] private string _summary;
        [SaveableField(3)] private string _details;
        [SaveableField(4)] private long _createdTicks;

        public WarReport()
        {
            _reportId = Guid.NewGuid().ToString();
            _summary = string.Empty;
            _details = string.Empty;
            _createdTicks = DateTime.UtcNow.Ticks;
        }

        public WarReport(string summary, string details) : this()
        {
            _summary = summary ?? string.Empty;
            _details = details ?? string.Empty;
        }

        public string ReportId { get { return _reportId; } set { _reportId = value ?? Guid.NewGuid().ToString(); } }
        public string Summary { get { return _summary; } set { _summary = value ?? string.Empty; } }
        public string Details { get { return _details; } set { _details = value ?? string.Empty; } }
        public DateTime CreatedAtUtc { get { return new DateTime(_createdTicks, DateTimeKind.Utc); } set { _createdTicks = value.ToUniversalTime().Ticks; } }

        public override string ToString() => $"Report[{ReportId}]: {Summary}";
    }
}