using System;
using System.Collections.ObjectModel;

namespace WarCouncilModern.UI.Dto
{
    public class WarCouncilDto
    {
        public Guid SaveId { get; set; }
        public string KingdomId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public int MemberCount { get; set; }
        public ObservableCollection<WarDecisionDto> Decisions { get; set; } = new ObservableCollection<WarDecisionDto>();
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
