using System;

namespace WarCouncilModern.UI.Dto
{
    public class WarDecisionDto
    {
        public Guid DecisionGuid { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int YeaCount { get; set; }
        public int NayCount { get; set; }
        public string ProposerId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string ShortPayloadSummary { get; set; } = string.Empty;
    }
}
