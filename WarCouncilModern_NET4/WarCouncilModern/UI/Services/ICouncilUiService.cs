using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using WarCouncilModern.UI.Dto;
using WarCouncilModern.UI.Enums;

namespace WarCouncilModern.UI.Services
{
    public interface ICouncilUiService : IDisposable, INotifyPropertyChanged
    {
        ObservableCollection<WarCouncilDto> AllCouncils { get; }
        OperationState CurrentOperation { get; }
        bool IsInitialized { get; }

        Task InitializeAsync(CancellationToken cancellationToken = default);
        void OpenOverviewScreen();
        Task ProposeDecisionAsync(Guid councilId, string title, string description, string payload, CancellationToken cancellationToken = default);
        Task CastVoteAsync(Guid councilId, Guid decisionId, bool vote, CancellationToken cancellationToken = default);
        Task RequestTallyAndExecuteAsync(Guid councilId, Guid decisionId, CancellationToken cancellationToken = default);

        bool IsLoading { get; }

        ObservableCollection<WarDecisionDto> Decisions { get; }

        Task ExecuteProposeNewDecision();

        bool CanProposeNewDecision { get; }
    }
}
