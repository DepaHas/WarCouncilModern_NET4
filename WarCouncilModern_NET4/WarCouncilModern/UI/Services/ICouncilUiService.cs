using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using WarCouncilModern.UI.Dto;

namespace WarCouncilModern.UI.Services
{
    public interface ICouncilUiService : IDisposable, INotifyPropertyChanged
    {
        ObservableCollection<WarCouncilDto> AllCouncils { get; }
        bool IsLoading { get; }
        bool IsProposing { get; }
        bool IsVoting { get; }
        bool IsTallying { get; }

        Task InitializeAsync(CancellationToken cancellationToken = default);
        Task ProposeDecisionAsync(Guid councilId, string title, string description, string payload, CancellationToken cancellationToken = default);
        Task CastVoteAsync(Guid councilId, Guid decisionId, bool vote, CancellationToken cancellationToken = default);
        Task RequestTallyAndExecuteAsync(Guid councilId, Guid decisionId, CancellationToken cancellationToken = default);
    }
}
