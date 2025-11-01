using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using WarCouncilModern.UI.Dto;

namespace WarCouncilModern.UI.Providers
{
    public class MockCouncilProvider : ICouncilProvider
    {
        public Task<IEnumerable<WarCouncilDto>> GetCouncilsAsync(CancellationToken cancellationToken = default)
        {
            var mockCouncils = new List<WarCouncilDto>
            {
                new WarCouncilDto
                {
                    SaveId = Guid.NewGuid(),
                    KingdomId = "empire",
                    Title = "Imperial War Council",
                    MemberCount = 5,
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    Status = "Active",
                    Decisions = new ObservableCollection<WarDecisionDto>
                    {
                        new WarDecisionDto
                        {
                            DecisionGuid = Guid.NewGuid(),
                            Title = "Declare War on Vlandia",
                            Description = "Vlandia has become too powerful. We must act now to curb their influence.",
                            Status = "Proposed",
                            YeaCount = 0,
                            NayCount = 0,
                        },
                        new WarDecisionDto
                        {
                            DecisionGuid = Guid.NewGuid(),
                            Title = "Propose Peace with Aserai",
                            Description = "Our southern border is stretched thin. A peace treaty with the Aserai would allow us to refocus our forces.",
                            Status = "VotingOpen",
                            YeaCount = 2,
                            NayCount = 1,
                        }
                    }
                },
                new WarCouncilDto
                {
                    SaveId = Guid.NewGuid(),
                    KingdomId = "vlandia",
                    Title = "Vlandian Royal Court",
                    MemberCount = 8,
                    CreatedAt = DateTime.UtcNow.AddMonths(-2),
                    Status = "Active",
                    Decisions = new ObservableCollection<WarDecisionDto>()
                }
            };

            return Task.FromResult<IEnumerable<WarCouncilDto>>(mockCouncils);
        }
    }
}
