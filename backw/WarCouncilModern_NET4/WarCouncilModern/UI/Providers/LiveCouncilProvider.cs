using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WarCouncilModern.Core.Manager;
using WarCouncilModern.Models.Entities;
using WarCouncilModern.UI.Dto;

namespace WarCouncilModern.UI.Providers
{
    public class LiveCouncilProvider : ICouncilProvider
    {
        private readonly IWarCouncilManager _manager;

        public LiveCouncilProvider(IWarCouncilManager manager)
        {
            _manager = manager;
        }

        public Task<IEnumerable<WarCouncilDto>> GetCouncilsAsync(CancellationToken cancellationToken = default)
        {
            var councils = _manager.Councils.Select(ToDto).ToList();
            return Task.FromResult<IEnumerable<WarCouncilDto>>(councils);
        }

        private WarCouncilDto ToDto(WarCouncil council)
        {
            var dto = new WarCouncilDto
            {
                SaveId = new System.Guid(council.SaveId),
                KingdomId = council.KingdomStringId,
                Title = council.Name ?? $"{council.KingdomStringId} Council",
                MemberCount = council.MemberHeroIds?.Count ?? 0,
                CreatedAt = council.CreatedAt,
                Status = "Active"
            };

            foreach (var decision in council.Decisions)
            {
                dto.Decisions.Add(new WarDecisionDto
                {
                    DecisionGuid = new System.Guid(decision.DecisionId),
                    Title = decision.Title,
                    Description = decision.Description,
                    Status = decision.Status,
                    YeaCount = decision.GetYeaVotes(),
                    NayCount = decision.GetNayVotes()
                });
            }

            return dto;
        }
    }
}
