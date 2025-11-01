using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WarCouncilModern.UI.Dto;

namespace WarCouncilModern.UI.Providers
{
    public interface ICouncilProvider
    {
        Task<IEnumerable<WarCouncilDto>> GetCouncilsAsync(CancellationToken cancellationToken = default);
    }
}
