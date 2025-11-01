using System;

namespace WarCouncilModern.UI.Dto
{
    public class WarDecisionDto
    {
        public Guid DecisionGuid { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Status { get; set; } = "";
        public int YeaCount { get; set; }
        public int NayCount { get; set; }
        public string ProposerId { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string ShortPayloadSummary { get; set; } = "";
    }
}
