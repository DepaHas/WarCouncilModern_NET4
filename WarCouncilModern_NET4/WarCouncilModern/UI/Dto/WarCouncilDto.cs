using System;

namespace WarCouncilModern.UI.Dto
{
    public class WarCouncilDto
    {
        public Guid SaveId { get; set; }
        public string KingdomId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public int MemberCount { get; set; }
        public int ActiveDecisionsCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
    }
}
