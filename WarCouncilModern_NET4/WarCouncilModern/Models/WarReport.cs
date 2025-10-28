using System;
using TaleWorlds.SaveSystem;

namespace WarCouncilModern.Models
{
    /// <summary>
    /// تقرير بسيط يمكن للمجلس حفظه.
    /// </summary>
    public class WarReport
    {
        [SaveableField(1)] private string _reportId;
        [SaveableField(2)] private string _content;
        [SaveableField(3)] private DateTime _createdAt;

        public WarReport()
        {
            _reportId = string.Empty;
            _content = string.Empty;
            _createdAt = DateTime.UtcNow;
        }

        public WarReport(string id, string content)
        {
            _reportId = id ?? string.Empty;
            _content = content ?? string.Empty;
            _createdAt = DateTime.UtcNow;
        }

        public string ReportId { get { return _reportId; } set { _reportId = value ?? string.Empty; } }
        public string Content { get { return _content; } set { _content = value ?? string.Empty; } }
        public DateTime CreatedAt { get { return _createdAt; } set { _createdAt = value; } }

        public override string ToString()
        {
            return string.Format("Report {0} created at {1:u}", ReportId, CreatedAt);
        }
    }
}