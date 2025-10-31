using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using WarCouncilModern.Core.Events;
using WarCouncilModern.Core.Manager;
using WarCouncilModern.Core.Services;
using WarCouncilModern.Models.Entities;
using WarCouncilModern.UI.Dto;
using WarCouncilModern.UI.Platform;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.UI.Services
{
    public class CouncilUiService : ICouncilUiService
    {
        private readonly IWarCouncilManager _manager;
        private readonly IWarDecisionService _warDecisionService;
        private readonly IUiInvoker _uiInvoker;
        private readonly IModLogger _logger;
        private readonly object _lock = new object();

        private readonly ObservableCollection<WarCouncilDto> _allCouncils = new();
        public ObservableCollection<WarCouncilDto> AllCouncils => _allCouncils;

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            private set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }

        private bool _isProposing;
        public bool IsProposing
        {
            get => _isProposing;
            private set { _isProposing = value; OnPropertyChanged(nameof(IsProposing)); }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public CouncilUiService(
            IWarCouncilManager manager,
            IWarDecisionService warDecisionService,
            IUiInvoker uiInvoker,
            IModLogger logger)
        {
            _manager = manager;
            _warDecisionService = warDecisionService;
            _uiInvoker = uiInvoker;
            _logger = logger;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            IsLoading = true;
            CouncilEvents.OnCouncilCreated += OnCouncilCreated_Background;
            CouncilEvents.OnDecisionProposed += OnDecisionProposed_Background;
            await Task.Run(() => RebuildAllFromBackend(), cancellationToken);
            IsLoading = false;
        }

        public async Task ProposeDecisionAsync(Guid councilId, string title, string description, string payload, CancellationToken cancellationToken = default)
        {
            IsProposing = true;
            try
            {
                await Task.Run(() =>
                {
                    var council = _manager.FindCouncilById(councilId);
                    if (council == null)
                    {
                        _logger.Warn($"[CouncilUiService] ProposeDecisionAsync failed: Council with id {councilId} not found.");
                        return;
                    }

                    // In a real scenario, the proposer would be the player hero.
                    _warDecisionService.ProposeDecision(council, title, description, Hero.MainHero, payload);
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error($"[CouncilUiService] ProposeDecisionAsync failed: {ex.Message}");
            }
            finally
            {
                IsProposing = false;
            }
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

        private void OnDecisionProposed_Background(WarCouncil council, WarDecision decision)
        {
            Task.Run(() =>
            {
                _uiInvoker.InvokeOnUi(() =>
                {
                    var councilDto = _allCouncils.FirstOrDefault(c => c.SaveId == new Guid(council.SaveId));
                    if (councilDto != null)
                    {
                        councilDto.ActiveDecisionsCount++;
                    }
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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            CouncilEvents.OnCouncilCreated -= OnCouncilCreated_Background;
            CouncilEvents.OnDecisionProposed -= OnDecisionProposed_Background;
        }
    }
}
