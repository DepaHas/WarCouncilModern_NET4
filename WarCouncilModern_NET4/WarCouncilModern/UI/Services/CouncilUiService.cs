using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using WarCouncilModern.Core.Events;
using WarCouncilModern.Core.Manager;
using WarCouncilModern.Models.Entities;
using WarCouncilModern.UI.Dto;
using WarCouncilModern.UI.Platform;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.UI.Services
{
    public class CouncilUiService : ICouncilUiService
    {
        private readonly IWarCouncilManager _manager;
        private readonly IUiInvoker _uiInvoker;
        private readonly IModLogger _logger;
        private readonly object _lock = new object();

        private readonly ObservableCollection<WarCouncilDto> _allCouncils = new();
        public ObservableCollection<WarCouncilDto> AllCouncils => _allCouncils;

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            private set { _isLoading = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLoading))); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public CouncilUiService(
            IWarCouncilManager manager,
            IUiInvoker uiInvoker,
            IModLogger logger)
        {
            _manager = manager;
            _uiInvoker = uiInvoker;
            _logger = logger;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            CouncilEvents.OnCouncilCreated += OnCouncilCreated_Background;
            await Task.Run(() => RebuildAllFromBackend(), cancellationToken);
        }

        private void RebuildAllFromBackend()
        {
            lock (_lock)
            {
                var councils = _manager.Councils.ToList();
                var dtos = councils.Select(ToDto).ToList();
                _uiInvoker.InvokeOnUi(() =>
                {
                    _allCouncils.Clear();
                    foreach (var d in dtos) _allCouncils.Add(d);
                });
            }
        }

        private void OnCouncilCreated_Background(WarCouncil council)
        {
            Task.Run(() =>
            {
                var dto = ToDto(council);
                _uiInvoker.InvokeOnUi(() =>
                {
                    if (!_allCouncils.Any(c => c.SaveId == new Guid(dto.SaveId)))
                        _allCouncils.Add(dto);
                });
            });
        }

        private WarCouncilDto ToDto(WarCouncil council)
        {
            return new WarCouncilDto
            {
                SaveId = new Guid(council.SaveId),
                KingdomId = council.KingdomStringId,
                Title = council.Name ?? $"{council.KingdomStringId} Council",
                MemberCount = council.MemberHeroIds?.Count ?? 0,
                ActiveDecisionsCount = council.Decisions?.Count(d => d.Status == "Proposed" || d.Status == "VotingOpen") ?? 0,
                CreatedAt = council.CreatedAt,
                Status = "Active"
            };
        }

        public void Dispose()
        {
            CouncilEvents.OnCouncilCreated -= OnCouncilCreated_Background;
        }
    }
}
