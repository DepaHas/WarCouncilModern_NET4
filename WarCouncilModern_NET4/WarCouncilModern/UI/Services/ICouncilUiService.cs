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

        Task InitializeAsync(CancellationToken cancellationToken = default);
    }
}
